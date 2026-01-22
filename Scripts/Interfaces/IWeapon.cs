namespace CosmicDoom.Scripts.Interfaces;

using Context;
using Items;

public interface IWeapon {
    public void Equip(RWeapon weapon);
    public void Use(RAttackContext context);
}