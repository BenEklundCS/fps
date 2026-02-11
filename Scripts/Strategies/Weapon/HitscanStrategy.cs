using CosmicDoom.Scripts.Objects.Projectiles;

namespace CosmicDoom.Scripts.Strategies.Weapon;

using Godot;
using Context;
using Interfaces;

public class HitscanStrategy(
    int shotCount = 1,
    float spreadDegrees = 0.0f
) : IWeaponStrategy {
    private IProjectile _debugProjectile = new Laser();

    public void Execute(RAttackContext context) {
        var damage = context.WEAPON.DAMAGE;
        var ray = context.RAY;
        var originalGlobalTransform = ray.GlobalTransform;

        for (int i = 0; i < shotCount; i++) {
            var projectile = _debugProjectile.Spawn();
            var spreadQuaternion = Utils.INSTANCE.GetSpreadQuaternion(spreadDegrees);

            ray.GlobalTransform = originalGlobalTransform;
            ray.GlobalBasis = new Basis(new Quaternion(ray.GlobalBasis) * spreadQuaternion);

            projectile.SetContext(context);
            ray.GetTree().Root.AddChild((Node3D)projectile);

            // Apply spawn offset in local space (relative to aim direction)
            var spawnTransform = ray.GlobalTransform;
            var offsetWorld = spawnTransform.Basis * context.WEAPON.ShotOffset;
            spawnTransform.Origin += offsetWorld;

            ((Node3D)projectile).GlobalTransform = spawnTransform;

            var collider = ray.GetCollider();
            if (collider is IHittable hittable) hittable.Hit(damage);
        }

        ray.GlobalTransform = originalGlobalTransform;
    }
}
