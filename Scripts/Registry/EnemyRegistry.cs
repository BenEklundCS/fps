using CosmicDoom.Scripts.Strategies.EnemyAI.Actions;
using CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Destroyer;

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
            GetSpriteFrames(EnemyType.Destroyer),
            () => {
                return new UtilityAiStrategy(
                    new IBackgroundAction[] { new FacePlayer() },
                    new DestroyerActionMove(),
                    new DestroyerActionAttack(),
                    new DestroyerActionPanic()
                );
            },
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

    private static SpriteFrames GetSpriteFrames(EnemyType enemyType) {
        var spriteFrames = new SpriteFrames();
        spriteFrames.RemoveAnimation("default");
        spriteFrames.AddAnimation("idle");
        spriteFrames.AddAnimation("walk");
        spriteFrames.AddAnimation("attack");

        var enemyName = enemyType.ToString().ToLower();

        foreach (var animationName in spriteFrames.GetAnimationNames()) {
            var path = $"res://Assets/Sprites/Monsters/monster_{enemyName}_{animationName}.png";
            spriteFrames.AddFrame(animationName, Utils.LoadTrimmed(path));
        }

        return spriteFrames;
    }
}
