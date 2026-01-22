namespace CosmicDoom.Scripts.Strategies.Weapon;

using Godot;
using Context;

public class ProjectileStrategy : IWeaponStrategy {
    public void Execute(RAttackContext context) {
        var shots = context.WEAPON.SHOT_COUNT;
        var spreadDegrees = context.WEAPON.SPREAD_DEGREES;
        var ray = context.RAY;
        var originalGlobalTransform = ray.GlobalTransform;
    
        for (int i = 0; i < shots; i++) {
            var projectile = context.WEAPON.PROJECTILE.Spawn();
            var spreadQuaternion = Utils.GetSpreadQuaternion(spreadDegrees);
            
            ray.GlobalTransform = originalGlobalTransform;
            ray.GlobalBasis = new Basis(new Quaternion(ray.GlobalBasis) * spreadQuaternion);

            projectile.SetContext(context);
            ray.GetTree().Root.AddChild((Node3D)projectile);

            ((Node3D)projectile).GlobalTransform = ray.GlobalTransform;
        }
        ray.GlobalTransform = originalGlobalTransform;
    }
}
