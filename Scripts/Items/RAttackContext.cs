using Godot;

namespace CosmicDoom.Scripts.Items;

using Entities;

public record RAttackContext(
    Vector3 FORWARD,
    RayCast3D RAY,
    RWeapon WEAPON,
    Character ATTACKER
);