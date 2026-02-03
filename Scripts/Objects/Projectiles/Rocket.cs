using CosmicDoom.Scripts.Entities;
using CosmicDoom.Scripts.Interfaces;
using Godot;

namespace CosmicDoom.Scripts.Objects.Projectiles;
using static Godot.GD;

public partial class Rocket : Projectile {
    private Area3D _collisionBox;
    private Area3D _explosionBox;
    private static readonly PackedScene ExplosionScene = GD.Load<PackedScene>("res://addons/ExplosionVFX/explosion.tscn");

    public override void _Ready() {
        _collisionBox = GetNode<Area3D>("CollisionBox");
        _collisionBox.BodyEntered += OnBodyEntered;
        _explosionBox = GetNode<Area3D>("ExplosionBox");
    }

    public override void _Process(double delta) {
        GlobalPosition += Velocity * (float)delta;
    }
    
    public override Rocket Spawn() {
        return (Rocket)Load<PackedScene>("res://Scenes/Objects/Projectiles/rocket.tscn").Instantiate();
    }

    private void OnBodyEntered(Node3D body) {
        if (body is Player) return;
        Explode();
    }

    private void Explode() {
        Print("Exploding");
        Velocity = Vector3.Zero;

        // Spawn explosion at rocket position
        var explosion = ExplosionScene.Instantiate<Node3D>();
        var spawnPos = GlobalPosition;
        explosion.Position = spawnPos; // Set position BEFORE adding to tree
        GetTree().Root.AddChild(explosion);
        // explosion auto-starts and auto-frees by default

        // Deal damage to overlapping bodies
        var bodies = _explosionBox.GetOverlappingBodies();
        foreach (var body in bodies) {
            if (body is Player) continue;
            if (body is IHittable hittable) hittable.Hit(Context.WEAPON.DAMAGE);
        }

        // Remove the rocket
        QueueFree();
    }

}
