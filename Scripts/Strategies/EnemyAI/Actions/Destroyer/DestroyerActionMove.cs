using System.Linq;
namespace CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Destroyer;

using Godot;

using Interfaces;
using Entities;

public class DestroyerActionMove : IAction {
    private float _moveTimer = 2.0f;
    public float Score(IEnemyControllable enemy) {
        return 0.7f;
    }

    public void Execute(IEnemyControllable enemy, double delta) {
        if (enemy is not Enemy node) return;
        
        _moveTimer -= (float)delta;

        var nearestPlayer = node
            .GetTree()
            .GetNodesInGroup("players")
            .Cast<Player>()
            .MinBy(player => node.GlobalPosition.DistanceTo(player.GlobalPosition));

        if (_moveTimer <= 0.0f) {
            var moveTargetPos = GetMovePosition(node);
            enemy.MoveTo(moveTargetPos);
            _moveTimer = Utils.INSTANCE.NextFloat(node.MoveThinkingTimeRange);
        }

        if (nearestPlayer != null) {
            var lookTargetPos = nearestPlayer.GlobalPosition;
            enemy.FaceTarget(lookTargetPos);
        }
    }
    
    private static Vector3 GetMovePosition(Enemy enemy) {
        var points = enemy.GetTree().GetNodesInGroup("points").Cast<Point>().ToArray();
        var player = (Player)enemy.GetTree().GetNodesInGroup("players")[0];

        if (player == null || points.Length == 0) {
            return enemy.GlobalPosition;
        }

        var pointsWherePlayerIsVisible = points.Where(point => {
            var result = Utils.INSTANCE.IntersectRayOnPath(point.GlobalPosition, player.GlobalPosition);
            return result.Count > 0 && (Node)result["collider"] is Player;
        }).ToArray();

        if (pointsWherePlayerIsVisible.Length == 0) {
            return Utils.INSTANCE.RandomElement(points).GlobalPosition;
        }

        return pointsWherePlayerIsVisible
            .MinBy(point => enemy.GlobalPosition.DistanceTo(point.GlobalPosition))!
            .GlobalPosition;
    }
}