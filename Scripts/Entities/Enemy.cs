using CosmicDoom.Scripts.Interfaces;
using Godot;

namespace CosmicDoom.Scripts.Entities;

public partial class Enemy : Character, IControllable {
    public void Attack() {
        throw new System.NotImplementedException();
    }

    public bool CanAttack() {
        throw new System.NotImplementedException();
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