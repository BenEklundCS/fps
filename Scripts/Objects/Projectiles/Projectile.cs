namespace CosmicDoom.Scripts.Objects.Projectiles;

using Godot;
using static Godot.GD;
using Interfaces;
using Context;

public partial class Projectile : Node3D, IProjectile<Projectile> {
    private RAttackContext _context;
    
    public void SetContext(RAttackContext context) {
        _context = context;
    }
    
    public Projectile Spawn() {
        return (Projectile)Load<PackedScene>("res://Scenes/Objects/Projectiles/laser.tscn").Instantiate();
    }
    IProjectile IProjectile.Spawn() => Spawn();
}
