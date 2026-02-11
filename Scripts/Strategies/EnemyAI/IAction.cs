namespace CosmicDoom.Scripts.Strategies.EnemyAI;

using Interfaces;

public interface IAction {
    float Score(IEnemyControllable enemy);
    void Execute(IEnemyControllable enemy, double delta);
}
