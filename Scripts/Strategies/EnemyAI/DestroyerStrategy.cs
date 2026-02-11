using System.Linq;

namespace CosmicDoom.Scripts.Strategies.EnemyAI;

using Godot;
using Entities;

public class DestroyerStrategy : IEnemyAiStrategy {
    private float _moveTimer = 5.0f;
    public void Execute(Enemy enemy, double delta) {
        _moveTimer -= (float)delta;
        
        var nearestPlayer = enemy
            .GetTree()
            .GetNodesInGroup("players")
            .Cast<Player>()
            .MinBy(player => enemy.GlobalPosition.DistanceTo(player.GlobalPosition));

        if (_moveTimer <= 0.0f) {
            var moveTargetPos = GetRandomPosition(enemy);
            enemy.MoveTo(moveTargetPos);
            _moveTimer = Utils.NextFloat(enemy.MoveThinkingTimeRange);
        }

        if (nearestPlayer != null) {
            var lookTargetPos = nearestPlayer.GlobalPosition;
            enemy.FaceTarget(lookTargetPos);
        }

        if (enemy.CanAttack()) {
            enemy.Attack();
        }
    }

    private static Vector3 GetRandomPosition(Enemy enemy) {
        var range = enemy.MoveRange;
        var position = enemy.GlobalPosition;
        return new Vector3(
            Utils.NextFloat(new Vector2(position.X - range, position.X + range)),
            position.Y,
            Utils.NextFloat(new Vector2(position.Z - range, position.Z + range))
        );
    }
}