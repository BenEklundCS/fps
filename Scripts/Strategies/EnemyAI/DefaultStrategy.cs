using System.Linq;

namespace CosmicDoom.Scripts.Strategies.EnemyAI;

using Godot;
using Entities;

public class DefaultStrategy : IEnemyAiStrategy {
    public void Execute(Enemy enemy, double delta) {
        var minDistance = float.MaxValue;

        var nearestPlayer = enemy
            .GetTree()
            .GetNodesInGroup("players")
            .Cast<Player>()
            .MinBy(player => enemy.GlobalPosition.DistanceTo(player.GlobalPosition));

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
