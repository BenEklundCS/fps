using CosmicDoom.Scripts.Entities;
using CosmicDoom.Scripts.Interfaces;
using Godot;

namespace CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Spider;
//TODO:: Jump away logic
public class SpiderActionJumpAway : IAction {
    private bool _inPanic = false;
    public float Score(IEnemyControllable enemy) {
        if (enemy is not Enemy node) return 0.0f;
        if (node.HEALTH <= 30) {
            return 1.0f;
        }
        else {
            _inPanic = false;
            return 0.0f;
        }
    }

    public void Execute(IEnemyControllable enemy, double delta) {
        if (_inPanic) return;
        if (enemy is not Enemy node) return;
        var moveTargetPos = AiUtils.GetMovePositionWhereHidden(node);
        node.MoveTo(moveTargetPos);
        node.TargetReached += OnTargetReached;
        _inPanic = true;
    }

    private void OnTargetReached() {
        _inPanic = false;
        
    }
}