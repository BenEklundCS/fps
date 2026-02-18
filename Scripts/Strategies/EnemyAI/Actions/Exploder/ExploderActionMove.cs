using System.Linq;
namespace CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Exploder;

using Godot;

using Interfaces;
using Entities;

public class ExploderActionMove : IAction {
    private float _moveTimer = 2.0f;
    public float Score(IEnemyControllable enemy) {
        return 0.7f;
    }

    public void Execute(IEnemyControllable enemy, double delta) {
        if (enemy is not Enemy node) return;

        _moveTimer -= (float)delta;

        if (_moveTimer <= 0.0f) {
            var moveTargetPos = AiUtils.GetMovePositionWherePlayerVisible(node);
            enemy.MoveTo(moveTargetPos);
            _moveTimer = Utils.INSTANCE.NextFloat(node.MoveThinkingTimeRange);
        }
    }
}