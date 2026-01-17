using CosmicDoom.Scripts.Interfaces;
using Godot;

namespace CosmicDoom.Scripts.Components;

public partial class AIController : Node {
    private IControllable _controlTarget;
    private Vector3 _moveDirection = Vector3.Zero;

    [Export] public NodePath Target;

    public override void _Ready() {
        _controlTarget = GetNode<IControllable>(Target);
    }

    public override void _PhysicsProcess(double delta) {
        // TODO: AI behavior logic (state machine, behavior tree, etc.)

        _controlTarget.Move(_moveDirection);
    }

    public void SetMoveDirection(Vector3 direction) {
        _moveDirection = direction;
    }

    public void LookAt(Vector3 targetPosition) {
        // TODO: Compute look delta to face target
    }

    public void Attack() {
        _controlTarget.Attack();
    }

    public void Jump() {
        _controlTarget.Jump();
    }
}
