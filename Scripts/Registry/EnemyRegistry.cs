namespace CosmicDoom.Scripts.Registry;

using Godot;
using System.Collections.Generic;
using Interfaces;
using Items;
using Strategies.EnemyAI;

using static Godot.GD;

public partial class EnemyRegistry : Node, IRegistry<EnemyType, REnemy> {
    private readonly Dictionary<EnemyType, REnemy> _registry = new() {
        [EnemyType.Destroyer] = new REnemy(
            EnemyType.Destroyer,
            Load<CompressedTexture2D>("res://Assets/Sprites/Monsters/monster_destroyer_idle.png"),
            new DestroyerStrategy(),
            WeaponType.PlasmaGun
        )
    };

    public static EnemyRegistry INSTANCE { get; private set; }

    public REnemy Get(EnemyType key) {
        return _registry.GetValueOrDefault(key);
    }

    public IEnumerable<EnemyType> GetKeys() {
        return _registry.Keys;
    }

    public override void _Ready() {
        INSTANCE = this;
    }
}
