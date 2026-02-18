using Godot;
using System;

[Tool]
public partial class TriggerableDoor : Node3D {
    [Export] public float MoveDistance = 2.1f;
    [Export] public float MoveDuration = 1.0f;
    
    private MeshInstance3D _mesh;
    private bool _isOpen;

    private Vector3 _meshStart;
    private Vector3 _meshEnd;

    public override void _Ready() {
        _mesh = GetNode<MeshInstance3D>("MeshInstance3D");
        _meshStart = _mesh.GlobalPosition;
        _meshEnd = new Vector3(_meshStart.X, _meshStart.Y + MoveDistance, _meshStart.Z);
        SetDoorOpen(true);
    }

    public void SetDoorOpen(bool open) {
        _isOpen = open;

        var tween = CreateTween();
        var targetPosition = open ? _meshEnd : _meshStart;

        tween.TweenProperty(_mesh, "global_position", targetPosition, MoveDuration);
    }
}
