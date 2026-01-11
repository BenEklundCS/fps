namespace CosmicDoom.Scripts;

using Godot;
using System;

public abstract partial class Character : CharacterBody3D {
    [Export] public int Health = 100;
    [Export] public float Speed = 5.0f;
    [Export] public float JumpSpeed = 5.0f;
    
    protected RayCast3D _ray;
    protected CollisionShape3D _collisionShape;

    public override void _Ready() {
        _ray = GetNode<RayCast3D>("RayCast3D");
        _collisionShape = GetNode<CollisionShape3D>("CollisionShape3D");
        base._Ready();
    }
}
