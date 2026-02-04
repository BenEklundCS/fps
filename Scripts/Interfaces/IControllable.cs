using CosmicDoom.Scripts.Entities;
using CosmicDoom.Scripts.Items;
using Godot;

namespace CosmicDoom.Scripts.Interfaces;

public interface IControllable {
    public delegate void OnDeathEventHandler();
    public void Attack();
    public void Look(Vector2 relative);
    public void Move(Vector3 direction);
    public void Jump();
    public void NextWeapon();
    public void PrevWeapon();
}