namespace CosmicDoom.Scripts.Objects;

using CosmicDoom.Scripts.Interfaces;
using CosmicDoom.Scripts.Items;
using Godot;
using static Godot.GD;

public partial class Laser : Node3D, IProjectile, ISpawnable<Laser> {
    private Timer _timer;
    [Export] public float Duration = 2.0f;
    
    private RAttackContext _context;

    public override void _Ready() {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;
    }

    public Laser Spawn() {
        return (Laser)Load<PackedScene>("res://Scenes/Objects/laser.tscn").Instantiate();
    }

    public void SetContext(RAttackContext context) {
        _context = context;
    }

    private void OnTimerTimeout() {
        QueueFree();
    }
}