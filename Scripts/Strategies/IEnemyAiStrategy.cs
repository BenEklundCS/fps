using CosmicDoom.Scripts.Interfaces;

namespace CosmicDoom.Scripts.Strategies;
using Context;

public interface IEnemyAiStrategy {
    public void Execute(IControllable controllable, double delta);
}