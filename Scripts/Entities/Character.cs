using System.Collections.Generic;
using CosmicDoom.Scripts.Interfaces;
using CosmicDoom.Scripts.Items;
using Godot;

namespace CosmicDoom.Scripts.Entities;

using static GD;

public abstract partial class Character : CharacterBody3D, IHittable {
    [Signal]
    public delegate void OnDeathEventHandler();
    
    [Export] public float JumpSpeed = 5.0f;
    [Export] public float Speed = 5.0f;
    [Export] public int MAX_HEALTH { get; private set; } = 100;
    [Export] public int HEALTH { get; protected set; }
    [Export] public int DAMAGE { get; private set; } = 10;
    
    protected CollisionShape3D CollisionShape;
    protected Node3D Head;
    protected RayCast3D Ray;
    
    public virtual void Hit(int damage) {
        HEALTH -= damage;
        if (!(HEALTH <= 0)) return;

        Print($"{Name} died.");
        EmitSignalOnDeath();
        Callable.From(() => {
            QueueFree();
        }).CallDeferred();
    }

    public override void _Ready() {
        Head = GetNode<Node3D>("Head");
        Ray = GetNode<RayCast3D>("Head/RayCast3D");
        CollisionShape = GetNode<CollisionShape3D>("CollisionShape3D");
        HEALTH = MAX_HEALTH;
        base._Ready();
    }
}