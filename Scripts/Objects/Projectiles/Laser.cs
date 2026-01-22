namespace CosmicDoom.Scripts.Objects.Projectiles;
using Godot;

public partial class Laser : Projectile {
    private Timer _timer;
    [Export] public float Duration = 2.0f;

    public override void _Ready() {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;
    }

    private void OnTimerTimeout() {
        QueueFree();
    }
}