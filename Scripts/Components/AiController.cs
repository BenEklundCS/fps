namespace CosmicDoom.Scripts.Components;

using Godot;
using Entities;
using Registry;
using Strategies;

public partial class AiController : Node {
    #nullable enable
    private Enemy? _enemy;
    private IEnemyAiStrategy? _strategy;

    public override void _Ready() {
        _enemy = GetParent<Enemy>();
        _strategy = EnemyRegistry.INSTANCE.Get(_enemy.Type).STRATEGY();
        _enemy.OnDeath += OnEnemyDeath;
    }

    public override void _PhysicsProcess(double delta) {
        if (_enemy == null) return;
        if (!_enemy.Enabled) return;
        _strategy?.Execute(_enemy, delta);
    }

    private void OnEnemyDeath() {
        _enemy = null;
    }
}
