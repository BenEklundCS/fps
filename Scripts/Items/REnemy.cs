namespace CosmicDoom.Scripts.Items;

using Godot;
using Strategies;

public enum EnemyType {
    Destroyer
}

public record REnemy(
    EnemyType Type,
    CompressedTexture2D Sprite,
    IEnemyAiStrategy Strategy,
    WeaponType WeaponType
);
