namespace CosmicDoom.Scripts.Entities;

using System;
using System.Collections.Generic;
using Context;
using Interfaces;
using Items;
using Registry;
using Godot;
using Objects;
using UI;


public partial class Player : Character, IControllable {
    private readonly WeaponType[] _defaultWeapons = new[] {
        WeaponType.None,
        WeaponType.PlasmaGun,
        WeaponType.Shotgun,
        WeaponType.RocketLauncher
    };
    private readonly List<RWeapon> _weaponWheel = new();
    
    private int _weaponIndex;
    private readonly float _maxPitch = Mathf.DegToRad(85f);
    private Camera3D _camera;
    private Weapon _weapon;
    private HealthBar _healthBar;
    
    [Export] public float MouseSensitivity = 0.005f;

    public override void _Ready() {
        _weapon = GetNode<Weapon>("Weapon");
        _camera = GetNode<Camera3D>("Head/Camera3D");
        GetNode<Label>("Label").QueueFree();
        _healthBar = new HealthBar();
        AddChild(_healthBar);
        ReadyWeapons();
        
        AddToGroup("players");
        
        base._Ready();
    }

    public override void _Process(double delta) {
        _healthBar.SetHealth(HEALTH, 100);
        base._Process(delta);
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
    
    public override void Hit(int damage) {
        _weapon.FlashIcon();
        base.Hit(damage);
    }

    public void Attack() {
        var weapon = _weaponWheel[_weaponIndex];
        RAttackContext context = new(
            -Head.GlobalBasis.Z, 
            Ray, 
            weapon, 
            this
        );
        _weapon.Use(context);
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
        var weapon = _weaponWheel[weaponIndex];
        _weapon.Equip(weapon);
    }
    
    private void ReadyWeapons() {
        foreach (var weaponType in _defaultWeapons) _weaponWheel.Add(WeaponRegistry.INSTANCE.Get(weaponType));
        EquipWeapon(_weaponIndex);
    }
}