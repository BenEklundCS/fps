using CosmicDoom.Scripts.Entities;
using CosmicDoom.Scripts.Interfaces;
using Godot;

namespace CosmicDoom.Scripts.Objects.Projectiles;

public partial class PlasmaBall : Projectile {
    private Area3D _collisionBox;
    private MeshInstance3D _mesh;
    private OmniLight3D _light;
    private Color _color = new Color(0.2f, 0.8f, 1.0f); // Default cyan plasma

    public Color PlasmaColor {
        get => _color;
        set {
            _color = value;
            ApplyColor();
        }
    }

    public override void _Ready() {
        _collisionBox = GetNode<Area3D>("CollisionBox");
        _collisionBox.BodyEntered += OnBodyEntered;
        _mesh = GetNode<MeshInstance3D>("PlasmaMesh");
        _light = GetNode<OmniLight3D>("PlasmaLight");
        ApplyColor();
    }

    public override void _Process(double delta) {
        GlobalPosition += Velocity * (float)delta;
    }

    public override PlasmaBall Spawn() {
        return (PlasmaBall)GD.Load<PackedScene>("res://Scenes/Objects/Projectiles/plasma_ball.tscn").Instantiate();
    }

    private void ApplyColor() {
        if (_mesh == null || _light == null) return;

        var mat = _mesh.GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
        if (mat != null) {
            mat.AlbedoColor = _color;
            mat.Emission = _color;
        }
        _light.LightColor = _color;
    }

    private void OnBodyEntered(Node3D body) {
        if (body == Context.ATTACKER) return;
        Hit(body);
    }

    private void Hit(Node3D body) {
        // Deal damage on hit
        if (body is IHittable hittable) {
            hittable.Hit(Context.WEAPON.DAMAGE);
        }
        QueueFree();
    }
}
