using System.Collections.Generic;
using CosmicDoom.Scripts.Interfaces;

namespace CosmicDoom.Scripts.Registry;

using Godot;
using static Godot.GD;
using CosmicDoom.Scripts.Items;

public partial class WeaponRegistry : Node, IRegistry<WeaponType, RWeapon> {
    public static WeaponRegistry INSTANCE { get; private set; }

    private readonly System.Collections.Generic.Dictionary<WeaponType, RWeapon> _weaponRegistry = new() {
        [WeaponType.Knife] = new RWeapon(
            WeaponType.Knife,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_knife.png")
        ),
        [WeaponType.MachineGun] = new RWeapon(
            WeaponType.MachineGun,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_machinegun.png")
        ),
        [WeaponType.PlasmaGun] = new RWeapon(
            WeaponType.PlasmaGun,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_plasmagun.png")
        ),
        [WeaponType.RocketLauncher] = new RWeapon(
            WeaponType.RocketLauncher,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_rocketlauncher.png")
        ),
        [WeaponType.Shotgun] = new RWeapon(
            WeaponType.Shotgun,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_shotgun.png")
        ),
        [WeaponType.Solution] = new RWeapon(
            WeaponType.Solution,
            Load<CompressedTexture2D>("res://Assets/Sprites/Weapons/weapon_solution.png")
        ),
        [WeaponType.None] = new RWeapon(
            WeaponType.None,
            null
        ),
    };

    public override void _Ready() {
        INSTANCE = this;
    }

    public RWeapon Get(WeaponType weaponType) {
        return _weaponRegistry.GetValueOrDefault(weaponType);
    }
}
