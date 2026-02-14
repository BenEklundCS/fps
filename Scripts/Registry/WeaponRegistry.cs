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
            PickupType.None,
            0,
            false,
            0,
            0,
            0.5f,
            Utils.LoadTrimmed("res://Assets/Sprites/Weapons/weapon_knife.png"),
            null,
            Utils.LoadTrimmed("res://Assets/Sprites/Icons/knife_icon_1.png"),
            [],
            new MeleeStrategy()
        ),
        [WeaponType.MachineGun] = new RWeapon(
            WeaponType.MachineGun,
            PickupType.Bullets,
            10,
            true,
            100,
            300,
            0.5f,
            Utils.LoadTrimmed("res://Assets/Sprites/Weapons/weapon_machinegun.png"),
            Utils.LoadTrimmed("res://Assets/Sprites/Effects/effect_fireball.png"),
            Utils.LoadTrimmed("res://Assets/Sprites/Icons/submachinegun_icon_5.png"),
            [],
            new HitscanStrategy()
        ),
        [WeaponType.PlasmaGun] = new RWeapon(
            WeaponType.PlasmaGun,
            PickupType.Plasma,
            10,
            true,
            30,
            120,
            0.1f,
            Utils.LoadTrimmed("res://Assets/Sprites/Weapons/weapon_plasmagun.png"),
            Utils.LoadTrimmed("res://Assets/Sprites/Effects/effect_fireball.png"),
            Utils.LoadTrimmed("res://Assets/Sprites/Icons/submachinegun_icon_1.png"),
            [
                Load<AudioStreamWav>("res://Sounds/Guns/Gunshots/243 Rifle/243 Rifle A 001.wav"),
                Load<AudioStreamWav>("res://Sounds/Guns/Gunshots/243 Rifle/243 Rifle A 002.wav"),
                Load<AudioStreamWav>("res://Sounds/Guns/Gunshots/243 Rifle/243 Rifle A 003.wav"),
            ],
            new ProjectileStrategy(projectile: new PlasmaBall(), projectileVelocity: 25f)
        ),
        [WeaponType.RocketLauncher] = new RWeapon(
            WeaponType.RocketLauncher,
            PickupType.Rockets,
            10,
            true,
            1,
            5,
            2.0f,
            Utils.LoadTrimmed("res://Assets/Sprites/Weapons/weapon_rocketlauncher.png"),
            Utils.LoadTrimmed("res://Assets/Sprites/Effects/effect_fireball.png"),
            Utils.LoadTrimmed("res://Assets/Sprites/Icons/rocket_launcher_icon_1.png"),
            [],
            new ProjectileStrategy(projectile: new Rocket(), projectileVelocity: 20f)
        ),
        [WeaponType.Shotgun] = new RWeapon(
            WeaponType.Shotgun,
            PickupType.Bullets,
            10,
            true,
            8,
            32,
            1.0f,
            Utils.LoadTrimmed("res://Assets/Sprites/Weapons/weapon_shotgun.png"),
            Utils.LoadTrimmed("res://Assets/Sprites/Effects/effect_fireball.png"),
            Utils.LoadTrimmed("res://Assets/Sprites/Icons/shotgun_icon_1.png"),
            [
                Load<AudioStreamWav>("res://Sounds/Guns/Gunshots/Dragonsbreath Shotgun/Dragonsbreath Shotgun A 001.wav"),
                Load<AudioStreamWav>("res://Sounds/Guns/Gunshots/Dragonsbreath Shotgun/Dragonsbreath Shotgun A 002.wav"),
                Load<AudioStreamWav>("res://Sounds/Guns/Gunshots/Dragonsbreath Shotgun/Dragonsbreath Shotgun A 003.wav"),
            ],
            new HitscanStrategy(shotCount: 10, spreadDegrees: 5.0f)
        ),
        [WeaponType.Solution] = new RWeapon(
            WeaponType.Solution,
            PickupType.Rockets,
            100,
            true,
            1,
            2,
            2.0f,
            Utils.LoadTrimmed("res://Assets/Sprites/Weapons/weapon_solution.png"),
            null,
            Utils.LoadTrimmed("res://Assets/Sprites/Icons/hand_grenade_icon_1.png"),
            [],
            new ProjectileStrategy(projectile: new Laser(), projectileVelocity: 5f)
        ),
        [WeaponType.None] = new RWeapon(
            WeaponType.None,
            PickupType.None,
            0,
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