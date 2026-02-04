using CosmicDoom.Scripts.Entities;
using CosmicDoom.Scripts.Interfaces;
using CosmicDoom.Scripts.Strategies;
using Godot;

namespace CosmicDoom.Scripts.Components;

public partial class AiController : Node {
    #nullable enable
    private IControllable? _controlTarget;
    private Vector3 _moveDirection = Vector3.Zero;
    private IEnemyAiStrategy? _strategy;

    [Export] required public NodePath Target;

    public override void _Ready() {
        _controlTarget = GetNode<IControllable>(Target);
        _strategy = EnemyAiRegistry.INSTANCE.Get(((Enemy)_controlTarget).Type);
        if (_controlTarget is Character character) {
            character.OnDeath += OnCharacterDeath;
        }
    }

    public override void _PhysicsProcess(double delta) {
        if (_controlTarget == null) return;
        _strategy?.Execute(_controlTarget, delta);
    }

    private void OnCharacterDeath() {
        _controlTarget = null;
    }
}
