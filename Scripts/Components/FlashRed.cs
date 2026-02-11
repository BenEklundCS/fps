namespace CosmicDoom.Scripts.Components;

using Godot;

public partial class FlashRed : Node {
    [Export] public NodePath Target;
    [Export] public int FlashTimes = 3;
    [Export] public float FlashInterval = 0.1f;

    private Node _target;
    private Timer _timer;
    private int _flashCount;

    public override void _Ready() {
        if (Target != null && !Target.IsEmpty) {
            _target = GetNode(Target);
        }
        _timer = new Timer { WaitTime = FlashInterval };
        _timer.Timeout += OnTimerTimeout;
        AddChild(_timer);
    }

    public void SetTarget(Node target) {
        _target = target;
    }

    public void Trigger() {
        _flashCount = 0;
        if (_timer.IsStopped()) {
            _timer.Start();
        }
    }

    private void OnTimerTimeout() {
        if (_flashCount >= FlashTimes) {
            _timer.Stop();
            SetModulate(Colors.White);
            return;
        }
        _flashCount++;
        SetModulate(GetModulate() == Colors.White ? Colors.Red : Colors.White);
    }

    private Color GetModulate() => _target switch {
        CanvasItem ci => ci.Modulate,
        SpriteBase3D s => s.Modulate,
        _ => Colors.White
    };

    private void SetModulate(Color color) {
        switch (_target) {
            case CanvasItem ci: ci.Modulate = color; break;
            case SpriteBase3D s: s.Modulate = color; break;
        }
    }
}
