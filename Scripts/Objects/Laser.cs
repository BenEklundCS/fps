using Godot;
using System;

public partial class Laser : Node3D {
    [Export] public float Duration = 2.0f;

    private Timer _timer;
    
    public override void _Ready() {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;
    }

    private void OnTimerTimeout() {
        QueueFree();
    }
}
