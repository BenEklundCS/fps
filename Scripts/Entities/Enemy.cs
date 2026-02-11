namespace CosmicDoom.Scripts.Entities;

using Godot;
using static Godot.GD;
using Context;
using Interfaces;
using Items;
using Objects;
using Registry;

public enum EnemyState { Idle, Walking, Attacking }

public partial class Enemy : Character, IEnemyControllable {
    [Export] public EnemyType Type;
    [Export] public float MoveRange = 500.0f;
    [Export] public Vector2 MoveThinkingTimeRange = new (3.0f, 8.0f);
    [Export] public int FlashTimes = 3;
    [Export] public float AttackDuration = 0.08f;
    [Export] public float BobAmplitude = 0.06f;
    [Export] public float BobFrequency = 10.0f;
    [Export] public int MagazineSize = 3;
    [Export] public int MaxAmmo = 300;
    private RWeapon _weaponData;
    private Weapon _weapon;
    private NavigationAgent3D _navigationAgent;
    private AnimatedSprite3D _animatedSprite;
    private Timer _flashTimer;
    private Timer _attackTimer;
    private int _flashedTimes = 0;
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
        _flashTimer = GetNode<Timer>("FlashTimer");
        _flashTimer.Timeout += OnFlashTimerTimeout;
        _attackTimer = GetNode<Timer>("AttackTimer");
        _attackTimer.SetWaitTime(AttackDuration);
        _attackTimer.Timeout += OnAttackTimerTimeout;

        base._Ready();
    }

    public override void _PhysicsProcess(double delta) {
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
        return playerInLineOfSight;
    }

    public override void Hit(int damage) {
        _flashedTimes = 0;
        if (_flashTimer.IsStopped()) {
            _flashTimer.Start();
        }
        base.Hit(damage);
    }

    private void OnFlashTimerTimeout() {
        if (_flashedTimes == FlashTimes) {
            _flashTimer.Stop();
            _animatedSprite.Modulate = Colors.White;
            return;
        }
        _flashedTimes += 1;
        _animatedSprite.Modulate = _animatedSprite.Modulate == Colors.White ? Colors.Red : Colors.White;
    }

    private void OnAttackTimerTimeout() {
        SetState(_navigationAgent.IsNavigationFinished() ? EnemyState.Idle : EnemyState.Walking);
    }

    private void OnTargetReached() {
        if (_state != EnemyState.Attacking)
            SetState(EnemyState.Idle);
    }

    private void SetState(EnemyState newState) {
        _state = newState;
        _animatedSprite.Play(newState switch {
            EnemyState.Walking => "walk",
            EnemyState.Attacking => "attack",
            _ => "idle"
        });
    }
}
