using CosmicDoom.Scripts.Entities;
using CosmicDoom.Scripts.Interfaces;

using static Godot.GD;

namespace CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Destroyer;

public class DestroyerActionAttack : IAction {
    public float Score(IEnemyControllable enemy) {
        if (enemy is not Enemy node) return 0;
        return node.CanAttack() ? 1.0f : 0.0f;
    }

    public void Execute(IEnemyControllable enemy, double delta) {
        if (enemy.CanAttack()) {
            enemy.Attack();
        }
    }
}