using CosmicDoom.Scripts.Entities;
using CosmicDoom.Scripts.Interfaces;

using static Godot.GD;

namespace CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Turret;

public class TurretActionAttack : IAction {
    public float Score(IEnemyControllable enemy) {
        return enemy.CanAttack() ? 1.0f : 0.0f;
    }

    public void Execute(IEnemyControllable enemy, double delta) {
        if (enemy.CanAttack()) {
            enemy.Attack();
        }
    }
}