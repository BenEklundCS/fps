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
    [Export] public int HEALTH { get; private set; } = 100;
    [Export] public int DAMAGE { get; private set; } = 10;
    
    protected CollisionShape3D CollisionShape;
    protected Node3D Head;
    protected RayCast3D Ray;
    
    private float _gravity;

    public void Hit(int damage) {
        HEALTH -= damage;
        if (!(HEALTH <= 0)) return;
        
        Print($"{Name} died.");
        EmitSignalOnDeath();
        QueueFree();
    }

    public override void _Ready() {
        Head = GetNode<Node3D>("Head");
        Ray = GetNode<RayCast3D>("Head/RayCast3D");
        CollisionShape = GetNode<CollisionShape3D>("CollisionShape3D");
        _gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
        base._Ready();
    }

    public override void _Process(double delta) {
        Move(delta);
        base._Process(delta);
    }

    private void Move(double delta) {
        Velocity = new Vector3(
            Velocity.X,
            Velocity.Y - _gravity * (float)delta,
            Velocity.Z
        );
        MoveAndSlide();
    }
}