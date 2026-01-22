namespace CosmicDoom.Scripts.Strategies;
using Context;

public interface IWeaponStrategy {
    #nullable enable
    public void Execute(RAttackContext context);
}