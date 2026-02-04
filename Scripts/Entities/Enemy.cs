using CosmicDoom.Scripts.Context;
using CosmicDoom.Scripts.Interfaces;
using CosmicDoom.Scripts.Items;
using CosmicDoom.Scripts.Objects;
using CosmicDoom.Scripts.Registry;
using Godot;
using static Godot.GD;

namespace CosmicDoom.Scripts.Entities;

public enum EnemyType {
    Destroyer
}

public partial class Enemy : Character, IControllable {
    [Export] public EnemyType Type;
    private RWeapon _weaponData;
    private Weapon _weapon;
    private Timer _cooldownTimer;

    public override void _Ready() {
        _weaponData = WeaponRegistry.INSTANCE.Get(WeaponType.PlasmaGun);
        _weapon = GetNode<Weapon>("Weapon");
        _weapon.Equip(_weaponData);
        _cooldownTimer = GetNode<Timer>("CooldownTimer");
        base._Ready();
    }

    public void Attack() {
        RAttackContext context = new(
            -Head.GlobalBasis.Z, 
            Ray, 
            _weaponData, 
            this
        );
        _weapon.Use(context);
        _cooldownTimer.Start();
    }

    public bool CanAttack() {
        return _cooldownTimer.IsStopped();
    }

    public void Jump() {
        throw new System.NotImplementedException();
    }

    public void Look(Vector2 relative) {
        throw new System.NotImplementedException();
    }

    public void Move(Vector3 direction) {
        throw new System.NotImplementedException();
    }

    public void NextWeapon() {
        throw new System.NotImplementedException();
    }

    public void PrevWeapon() {
        throw new System.NotImplementedException();
    }
}