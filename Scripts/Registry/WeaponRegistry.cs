using System.Collections.Generic;
using CosmicDoom.Scripts.Interfaces;
using CosmicDoom.Scripts.Items;
using CosmicDoom.Scripts.Objects.Projectiles;
using CosmicDoom.Scripts.Strategies.Weapon;
using Godot;

namespace CosmicDoom.Scripts.Registry;

using static GD;

public partial class WeaponRegistry : Node, IRegistry<WeaponType, RWeapon> {
    private readonly Dictionary<WeaponType, RWeapon> _weaponRegistry = new() {
        [WeaponType.Knife] = new RWeapon(
            WeaponType.Knife,
            10,
            false,
            0,
            0,
            0.5f,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_knife.png"),
            null,
            Load<CompressedTexture2D>("res://Assets/Sprites/Icons/knife_icon_1.png"),
            [],
            new MeleeStrategy()
        ),
        [WeaponType.MachineGun] = new RWeapon(
            WeaponType.MachineGun,
            10,
            true,
            100,
            300,
            0.5f,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_machinegun.png"),
            Load<CompressedTexture2D>("res://Assets/Sprites/Effects/effect_fireball.png"),
            Load<CompressedTexture2D>("res://Assets/Sprites/Icons/submachinegun_icon_5.png"),
            [],
            new HitscanStrategy()
        ),
        [WeaponType.PlasmaGun] = new RWeapon(
            WeaponType.PlasmaGun,
            10,
            true,
            30,
            120,
            0.1f,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_plasmagun.png"),
            Load<CompressedTexture2D>("res://Assets/Sprites/Effects/effect_fireball.png"),
            Load<CompressedTexture2D>("res://Assets/Sprites/Icons/submachinegun_icon_1.png"),
            [
                Load<AudioStreamWav>("res://Sounds/Guns/Gunshots/243 Rifle/243 Rifle A 001.wav"),
                Load<AudioStreamWav>("res://Sounds/Guns/Gunshots/243 Rifle/243 Rifle A 002.wav"),
                Load<AudioStreamWav>("res://Sounds/Guns/Gunshots/243 Rifle/243 Rifle A 003.wav"),
            ],
            new ProjectileStrategy(projectile: new PlasmaBall(), projectileVelocity: 25f)
        ),
        [WeaponType.RocketLauncher] = new RWeapon(
            WeaponType.RocketLauncher,
            10,
            true,
            1,
            5,
            2.0f,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_rocketlauncher.png"),
            Load<CompressedTexture2D>("res://Assets/Sprites/Effects/effect_fireball.png"),
            Load<CompressedTexture2D>("res://Assets/Sprites/Icons/rocket_launcher_icon_1.png"),
            [],
            new ProjectileStrategy(projectile: new Rocket(), projectileVelocity: 10f)
        ),
        [WeaponType.Shotgun] = new RWeapon(
            WeaponType.Shotgun,
            10,
            true,
            8,
            32,
            1.0f,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_shotgun.png"),
            Load<CompressedTexture2D>("res://Assets/Sprites/Effects/effect_fireball.png"),
            Load<CompressedTexture2D>("res://Assets/Sprites/Icons/shotgun_icon_1.png"),
            [
                Load<AudioStreamWav>("res://Sounds/Guns/Gunshots/Dragonsbreath Shotgun/Dragonsbreath Shotgun A 001.wav"),
                Load<AudioStreamWav>("res://Sounds/Guns/Gunshots/Dragonsbreath Shotgun/Dragonsbreath Shotgun A 002.wav"),
                Load<AudioStreamWav>("res://Sounds/Guns/Gunshots/Dragonsbreath Shotgun/Dragonsbreath Shotgun A 003.wav"),
            ],
            new HitscanStrategy(shotCount: 10, spreadDegrees: 5.0f)
        ),
        [WeaponType.Solution] = new RWeapon(
            WeaponType.Solution,
            10,
            true,
            1,
            2,
            2.0f,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_solution.png"),
            null,
            Load<CompressedTexture2D>("res://Assets/Sprites/Icons/hand_grenade_icon_1.png"),
            [],
            new ProjectileStrategy(projectile: new Laser(), projectileVelocity: 5f)
        ),
        [WeaponType.None] = new RWeapon(
            WeaponType.None,
            10,
            false,
            0,
            0,
            0.1f,
            null,
            null,
            null,
            [],
            new DefaultStrategy()
        )
    };

    public static WeaponRegistry INSTANCE { get; private set; }

    public RWeapon Get(WeaponType weaponType) {
        return _weaponRegistry.GetValueOrDefault(weaponType);
    }

    public IEnumerable<WeaponType> GetKeys() {
        return _weaponRegistry.Keys;
    }

    public override void _Ready() {
        INSTANCE = this;
    }
}