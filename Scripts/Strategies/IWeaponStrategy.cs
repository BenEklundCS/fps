namespace CosmicDoom.Scripts.Strategies;
using Context;

public interface IWeaponStrategy {
    public void Execute(RAttackContext context);
}