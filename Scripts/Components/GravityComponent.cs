using Godot;

namespace CosmicDoom.Scripts.Components;

public partial class GravityComponent : Node {
    private CharacterBody3D _body;
    private float _gravity;

    public override void _Ready() {
        _body = GetParent<CharacterBody3D>();
        _gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
    }

    public override void _PhysicsProcess(double delta) {
        _body.Velocity = new Vector3(
            _body.Velocity.X,
            _body.Velocity.Y - _gravity * (float)delta,
            _body.Velocity.Z
        );
        _body.MoveAndSlide();
    }
}
