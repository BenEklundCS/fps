using CosmicDoom.Scripts.Strategies.EnemyAI.Actions;
using CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Destroyer;
using CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Ender;
using CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Exploder;
using CosmicDoom.Scripts.Strategies.EnemyAI.Actions.PlasmaBot;
using CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Spider;
using CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Turret;
using CosmicDoom.Scripts.Strategies.EnemyAI.Actions.Warrior;

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
                    new Attack(),
                    new DestroyerActionPanic()
                );
            },
            WeaponType.PlasmaGun
        ),
        [EnemyType.Turret] = new REnemy(
            EnemyType.Turret,
            GetSpriteFrames(EnemyType.Turret),
            () => {
                return new UtilityAiStrategy(
                    new IBackgroundAction[] { new FacePlayer() },
                    new TurretActionAttack()
                );
            },
            WeaponType.PlasmaGun
        ),
        [EnemyType.Spider] = new REnemy(
            EnemyType.Spider,
            GetSpriteFrames(EnemyType.Spider),
            () => {
                return new UtilityAiStrategy(
                    new IBackgroundAction[] { new FacePlayer() },
                    new Attack(),
                    new SpiderActionMove(),
                    new SpiderActionJumpAway()
                );
            },
            WeaponType.RocketLauncher
        ),
        [EnemyType.Ender] = new REnemy(
            EnemyType.Ender,
            GetSpriteFrames(EnemyType.Ender),
            () => {
                return new UtilityAiStrategy(
                    new IBackgroundAction[] { new FacePlayer() },
                    new Attack(),
                    new EnderActionMove()
                );
            },
            WeaponType.PlasmaGun
        ),
        [EnemyType.Exploder] = new REnemy(
            EnemyType.Exploder,
            GetSpriteFrames(EnemyType.Exploder),
            () => {
                return new UtilityAiStrategy(
                    new IBackgroundAction[] { new FacePlayer() },
                    new ExploderActionAttack(),
                    new ExploderActionMove()
                );
            },
            WeaponType.None
        ),
        [EnemyType.PlasmaBot] = new REnemy(
            EnemyType.PlasmaBot,
            GetSpriteFrames(EnemyType.PlasmaBot),
            () => {
                return new UtilityAiStrategy(
                    new IBackgroundAction[] { new FacePlayer() },
                    new Attack(),
                    new PlasmaBotActionMove()
                );
            },
            WeaponType.PlasmaGun
        ),
        [EnemyType.Warrior] = new REnemy(
            EnemyType.Warrior,
            GetSpriteFrames(EnemyType.Warrior),
            () => {
                return new UtilityAiStrategy(
                    new IBackgroundAction[] { new FacePlayer() },
                    new Attack(),
                    new WarriorActionMove()
                );
            },
            WeaponType.Knife
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
            if (!ResourceLoader.Exists(path)) continue;
            spriteFrames.AddFrame(animationName, Utils.LoadTrimmed(path));
        }

        return spriteFrames;
    }
}
