using System.Linq;

namespace CosmicDoom.Scripts.Strategies.EnemyAI;

using Godot;
using Entities;
using Interfaces;

public class DefaultStrategy : IEnemyAiStrategy {
    public void Execute(IEnemyControllable enemy, double delta) {
        if (enemy is not Enemy node) return;

        var nearestPlayer = node
            .GetTree()
            .GetNodesInGroup("players")
            .Cast<Player>()
            .MinBy(player => node.GlobalPosition.DistanceTo(player.GlobalPosition));

        if (nearestPlayer != null) {
            var targetPos = nearestPlayer.GlobalPosition;
            enemy.MoveTo(targetPos);
            enemy.FaceTarget(targetPos);
        }

        if (enemy.CanAttack()) {
            enemy.Attack();
        }
    }
}
