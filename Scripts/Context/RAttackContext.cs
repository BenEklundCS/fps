namespace CosmicDoom.Scripts.Context;
using Godot;
using Entities;
using Items;

public record RAttackContext(
    Vector3 FORWARD,
    RayCast3D RAY,
    RWeapon WEAPON,
    Character ATTACKER
);