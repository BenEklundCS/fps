using CosmicDoom.Scripts.Interfaces;
using CosmicDoom.Scripts.Items;
using Godot;

namespace CosmicDoom.Scripts.Strategies;

public interface IWeaponStrategy {
    #nullable enable
    public void Execute(RAttackContext context);
}