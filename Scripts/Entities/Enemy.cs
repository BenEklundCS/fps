namespace CosmicDoom.Scripts.Entities;

using Godot;
using static Godot.GD;
using Context;
using Interfaces;
using Items;
using Components;
using Objects;
using Registry;

public enum EnemyState { Idle, Walking, Attacking }

public partial class Enemy : Character, IEnemyControllable {
    [Signal] public delegate void TargetReachedEventHandler();

    [Export] public bool Enabled = true;
    [Export] public EnemyType Type;
    [Export] public float MoveRange = 500.0f;
    [Export] public Vector2 MoveThinkingTimeRange = new (1.0f, 5.0f);
    [Export] public float AttackDuration = 0.08f;
    [Export] public float BobAmplitude = 0.06f;
    [Export] public float BobFrequency = 10.0f;
    [Export] public int MagazineSize = 3;
    [Export] public int MaxAmmo = 300;
    private RWeapon _weaponData;
    private Weapon _weapon;
    private NavigationAgent3D _navigationAgent;
    private AnimatedSprite3D _animatedSprite;
    private FlashRed _flashRed;
    private Timer _attackTimer;
    private float _bobTime = 0.0f;
    private float _spriteBaseY;
    private EnemyState _state = EnemyState.Idle;

    public override void _Ready() {
        var data = EnemyRegistry.INSTANCE.Get(Type);
        var baseWeapon = WeaponRegistry.INSTANCE.Get(data.WEAPON_TYPE);
        _weaponData = baseWeapon with { AMMO = MagazineSize, MAX_AMMO = MaxAmmo };
        _weapon = GetNode<Weapon>("Weapon");
        _weapon.Equip(_weaponData);
        _navigationAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
        _navigationAgent.TargetReached += OnTargetReached;
        _animatedSprite = GetNode<AnimatedSprite3D>("AnimatedSprite3D");
        _animatedSprite.SpriteFrames = data.SPRITE_FRAMES;
        _animatedSprite.Play("idle");
        _spriteBaseY = _animatedSprite.Position.Y;
        _flashRed = GetNode<FlashRed>("FlashRed");
        _attackTimer = GetNode<Timer>("AttackTimer");
        _attackTimer.SetWaitTime(AttackDuration);
        _attackTimer.Timeout += OnAttackTimerTimeout;
        
        AddToGroup("enemies");

        base._Ready();
    }

    public override void _PhysicsProcess(double delta) {
        if (_navigationAgent == null) return;
        if (_navigationAgent.IsNavigationFinished()) {
            Velocity = new Vector3(0, Velocity.Y, 0);
            _bobTime = 0.0f;
            var pos = _animatedSprite.Position;
            pos.Y = _spriteBaseY;
            _animatedSprite.Position = pos;
            return;
        }

        var nextPosition = _navigationAgent.GetNextPathPosition();
        var direction = (nextPosition - GlobalPosition).Normalized();
        Velocity = new Vector3(direction.X * Speed, Velocity.Y, direction.Z * Speed);

        HandleBob(delta);
        
        base._PhysicsProcess(delta);
    }

    public void MoveTo(Vector3 position) {
        _navigationAgent.TargetPosition = position;
        SetState(EnemyState.Walking);
    }

    public void FaceTarget(Vector3 position) {
        var target = position;
        target.Y = GlobalPosition.Y;
        LookAt(target);
    }

    public void Attack() {
        RAttackContext context = new(
            -Head.GlobalBasis.Z,
            Ray,
            _weaponData,
            this
        );

        if (_weapon.Use(context) && _state != EnemyState.Attacking) {
            SetState(EnemyState.Attacking);
            _attackTimer.Start();
        }
    }

    public bool CanAttack() {
        var playerInLineOfSight = Ray.GetCollider() is Player;
        return playerInLineOfSight && !_weapon.OnCooldown();
    }

    public override void Hit(int damage) {
        _flashRed.Trigger();
        base.Hit(damage);
    }

    private void OnAttackTimerTimeout() {
        SetState(_navigationAgent.IsNavigationFinished() ? EnemyState.Idle : EnemyState.Walking);
    }

    private void OnTargetReached() {
        if (_state != EnemyState.Attacking)
            SetState(EnemyState.Idle);
        EmitSignalTargetReached();
    }

    private void SetState(EnemyState newState) {
        _state = newState;
        _animatedSprite.Play(newState switch {
            EnemyState.Walking => "walk",
            EnemyState.Attacking => "attack",
            _ => "idle"
        });
    }

    private void HandleBob(double delta) {
        if (_state == EnemyState.Walking) {
            _bobTime += (float)delta;
            var bobOffset = Mathf.Sin(_bobTime * BobFrequency) * BobAmplitude;
            var spritePos = _animatedSprite.Position;
            spritePos.Y = _spriteBaseY + bobOffset;
            _animatedSprite.Position = spritePos;
        } else {
            _bobTime = 0.0f;
            var pos = _animatedSprite.Position;
            pos.Y = _spriteBaseY;
            _animatedSprite.Position = pos;
        }
    }
}
