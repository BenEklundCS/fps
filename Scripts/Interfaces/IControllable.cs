using CosmicDoom.Scripts.Items;
using Godot;

namespace CosmicDoom.Scripts.Interfaces;

public interface IControllable {
    public void Attack();
    public bool CanAttack();
    public void Look(Vector2 relative);
    public void Move(Vector3 direction);
    public void Jump();
    public void NextWeapon();
    public void PrevWeapon();
}