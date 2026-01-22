namespace CosmicDoom.Scripts.Interfaces;
using Context;
using Godot;


public interface IProjectile {
    void SetContext(RAttackContext context);
    IProjectile Spawn();
}

public interface IProjectile<TProjectile> : IProjectile where TProjectile : Node3D, IProjectile {
    new TProjectile Spawn(); 
}