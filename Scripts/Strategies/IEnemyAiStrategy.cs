namespace CosmicDoom.Scripts.Strategies;

using Interfaces;

public interface IEnemyAiStrategy {
    public void Execute(IEnemyControllable enemy, double delta);
}
