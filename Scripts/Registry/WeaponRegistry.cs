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
            1,
            0f,
            0.5f,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_knife.png"),
            [],
            new Laser(),
            new MeleeStrategy()
        ),
        [WeaponType.MachineGun] = new RWeapon(
            WeaponType.MachineGun,
            10,
            1,
            0f,
            0.5f,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_machinegun.png"),
            [],
            new Laser(),
            new HitscanStrategy()
        ),
        [WeaponType.PlasmaGun] = new RWeapon(
            WeaponType.PlasmaGun,
            10,
            1,
            0f,
            0.1f,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_plasmagun.png"),
            [
                Load<AudioStreamWav>("res://Sounds/Guns/Gunshots/243 Rifle/243 Rifle A 001.wav"),
                Load<AudioStreamWav>("res://Sounds/Guns/Gunshots/243 Rifle/243 Rifle A 002.wav"),
                Load<AudioStreamWav>("res://Sounds/Guns/Gunshots/243 Rifle/243 Rifle A 003.wav"),
            ],
            new Laser(),
            new HitscanStrategy()
        ),
        [WeaponType.RocketLauncher] = new RWeapon(
            WeaponType.RocketLauncher,
            10,
            1,
            0f,
            2.0f,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_rocketlauncher.png"),
            [],
            new Laser(),
            new ProjectileStrategy()
        ),
        [WeaponType.Shotgun] = new RWeapon(
            WeaponType.Shotgun,
            10,
            10,
            5.0f,
            1.0f,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_shotgun.png"),
            [
                Load<AudioStreamWav>("res://Sounds/Guns/Gunshots/Dragonsbreath Shotgun/Dragonsbreath Shotgun A 001.wav"),
                Load<AudioStreamWav>("res://Sounds/Guns/Gunshots/Dragonsbreath Shotgun/Dragonsbreath Shotgun A 002.wav"),
                Load<AudioStreamWav>("res://Sounds/Guns/Gunshots/Dragonsbreath Shotgun/Dragonsbreath Shotgun A 003.wav"),
            ],
            new Laser(),
            new HitscanStrategy()
        ),
        [WeaponType.Solution] = new RWeapon(
            WeaponType.Solution,
            10,
            1,
            0f,
            2.0f,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_solution.png"),
            [],
            new Laser(),
            new ProjectileStrategy()
        ),
        [WeaponType.None] = new RWeapon(
            WeaponType.None,
            10,
            1,
            0f,
            0.1f,
            null,
            [],
            new Laser(),
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