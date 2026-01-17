using System;
using System.Collections.Generic;
using CosmicDoom.Scripts.Interfaces;
using CosmicDoom.Scripts.Items;
using CosmicDoom.Scripts.Registry;
using Godot;

namespace CosmicDoom.Scripts.Entities;

public partial class Player : Character, IControllable {
    private readonly WeaponType[] _defaultWeapons = new[] {
        WeaponType.None,
        WeaponType.PlasmaGun,
        WeaponType.Shotgun
    };

    private readonly float _maxPitch = Mathf.DegToRad(85f);
    private readonly List<RWeapon> _weaponWheel = new();

    private Camera3D _camera;
    private int _weaponIndex;
    private TextureRect _weaponTextureRect;
    [Export] public float MouseSensitivity = 0.005f;
    [Export] public string MoveDownKeybind = "ui_down";
    [Export] public string MoveLeftKeybind = "ui_left";
    [Export] public string MoveRightKeybind = "ui_right";
    [Export] public string MoveUpKeybind = "ui_up";

    public override void _Ready() {
        _camera = GetNode<Camera3D>("Head/Camera3D");
        _weaponTextureRect = GetNode<TextureRect>("UIBar/WeaponTextureRect");
        ReadyWeapons();
        base._Ready();
    }
    
    public void Move(Vector3 movement) {
        var direction = Transform.Basis * movement;
        
        Velocity = new Vector3(
            direction.X * Speed,
            Velocity.Y,
            direction.Z * Speed
        );
    }

    public void Jump() {
        if (IsOnFloor())
            Velocity = new Vector3(Velocity.X, JumpSpeed, Velocity.Z);
    }
    
    public void Look(Vector2 relative) {
        RotateY(-relative.X * MouseSensitivity);
        Head.RotateX(-relative.Y * MouseSensitivity);
        var clampedX = Math.Clamp(
            Head.Rotation.X,
            -_maxPitch,
            _maxPitch
        );
        Head.Rotation = new Vector3(clampedX, Head.Rotation.Y, Head.Rotation.Z);
    }
    
    public new void Attack() {
        var weapon = _weaponWheel[_weaponIndex];
        RAttackContext context = new(-Head.GlobalBasis.Z, Ray, weapon, this);
        weapon.STRATEGY.Execute(context);
        CooldownTimer.Start(weapon.COOLDOWN);
    }

    public bool CanAttack() {
        return CooldownTimer.IsStopped();
    }

    public void NextWeapon() {
        _weaponIndex = Mathf.PosMod(_weaponIndex + 1, _weaponWheel.Count);
        EquipWeapon(_weaponIndex);
    }

    public void PrevWeapon() {
        _weaponIndex = Mathf.PosMod(_weaponIndex - 1, _weaponWheel.Count);
        EquipWeapon(_weaponIndex);
    }

    private void EquipWeapon(int weaponIndex) {
        _weaponTextureRect.Texture = _weaponWheel[weaponIndex].TEXTURE;
       CooldownTimer?.Stop();
    }
    
    private void ReadyWeapons() {
        foreach (var weaponType in _defaultWeapons) _weaponWheel.Add(WeaponRegistry.INSTANCE.Get(weaponType));
        EquipWeapon(_weaponIndex);
    }
}