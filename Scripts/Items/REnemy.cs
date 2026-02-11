namespace CosmicDoom.Scripts.Items;

using Godot;
using Strategies;

public enum EnemyType {
    Destroyer
}

public record REnemy(
    EnemyType TYPE,
    SpriteFrames SPRITE_FRAMES,
    IEnemyAiStrategy STRATEGY,
    WeaponType WEAPON_TYPE
);
