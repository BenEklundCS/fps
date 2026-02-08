namespace CosmicDoom.Scripts.Entities;

using Godot;
using static Godot.GD;
using Context;
using Interfaces;
using Items;
using Objects;
using Registry;

public partial class Enemy : Character, IEnemyControllable {
    [Export] public EnemyType Type;
    [Export] public float MoveRange = 500.0f;
    [Export] public Vector2 MoveThinkingTimeRange = new (3.0f, 8.0f);
    [Export] public int FlashTimes = 3;
    private RWeapon _weaponData;
    private Weapon _weapon;
    private Timer _cooldownTimer;
    private NavigationAgent3D _navigationAgent;
    private Sprite3D _sprite;
    private Timer _flashTimer;
    private int _flashedTimes = 0;

    public override void _Ready() {
        var data = EnemyRegistry.INSTANCE.Get(Type);
        _weaponData = WeaponRegistry.INSTANCE.Get(data.WeaponType);
        _weapon = GetNode<Weapon>("Weapon");
        _weapon.Equip(_weaponData);
        _navigationAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
        _cooldownTimer = GetNode<Timer>("CooldownTimer");
        _sprite = GetNode<Sprite3D>("Sprite3D");
        _sprite.Texture = data.Sprite;
        _flashTimer = GetNode<Timer>("FlashTimer");
        _flashTimer.Timeout += OnFlashTimerTimeout;
        base._Ready();
    }

    public override void _PhysicsProcess(double delta) {
        if (_navigationAgent.IsNavigationFinished()) {
            Velocity = new Vector3(0, Velocity.Y, 0);
            return;
        }

        var nextPosition = _navigationAgent.GetNextPathPosition();
        var direction = (nextPosition - GlobalPosition).Normalized();
        Velocity = new Vector3(direction.X * Speed, Velocity.Y, direction.Z * Speed);
    }

    public void MoveTo(Vector3 position) {
        _navigationAgent.TargetPosition = position;
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
        _weapon.Use(context);
        _cooldownTimer.Start();
    }

    public bool CanAttack() {
        var playerInLineOfSight = Ray.GetCollider() is Player;
        return _cooldownTimer.IsStopped() && playerInLineOfSight;
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
            _sprite.Modulate = Colors.White;
            return;
        }
        _flashedTimes += 1;
        _sprite.Modulate = _sprite.Modulate == Colors.White ? Colors.Red : Colors.White;
    }
}
