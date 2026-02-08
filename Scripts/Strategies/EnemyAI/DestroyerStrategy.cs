namespace CosmicDoom.Scripts.Strategies.EnemyAI;

using Godot;
using Entities;

public class DestroyerStrategy : IEnemyAiStrategy {
    private float moveTimer = 5.0f;
    public void Execute(Enemy enemy, double delta) {
        moveTimer -= (float)delta;
        
        var minDistance = float.MaxValue;
        Player nearestPlayer = null;
        FindNearestPlayer(enemy.GetTree().Root, enemy, ref minDistance, ref nearestPlayer);

        if (moveTimer <= 0.0f) {
            var moveTargetPos = GetRandomPosition(enemy);
            enemy.MoveTo(moveTargetPos);
            moveTimer = Utils.NextFloat(enemy.MoveThinkingTimeRange);
        }

        if (nearestPlayer != null) {
            var lookTargetPos = nearestPlayer.GlobalPosition;
            enemy.FaceTarget(lookTargetPos);
        }

        if (enemy.CanAttack()) {
            enemy.Attack();
        }
    }

    private static void FindNearestPlayer(Node node, Enemy enemy, ref float minDistance, ref Player nearestPlayer) {
        if (node is Player player) {
            var distance = enemy.GlobalPosition.DistanceTo(player.GlobalPosition);
            if (distance < minDistance) {
                minDistance = distance;
                nearestPlayer = player;
            }
        }
        foreach (var child in node.GetChildren()) {
            FindNearestPlayer(child, enemy, ref minDistance, ref nearestPlayer);
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