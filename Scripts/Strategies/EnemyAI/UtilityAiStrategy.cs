namespace CosmicDoom.Scripts.Strategies.EnemyAI;

using Interfaces;

public class UtilityAiStrategy(params IAction[] actions) : IEnemyAiStrategy {
    public void Execute(IEnemyControllable enemy, double delta) {
        IAction best = null;
        var bestScore = float.MinValue;

        foreach (var action in actions) {
            var score = action.Score(enemy);
            if (score > bestScore) {
                bestScore = score;
                best = action;
            }
        }

        best?.Execute(enemy, delta);
    }
}
