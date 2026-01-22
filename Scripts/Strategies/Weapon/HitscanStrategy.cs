namespace CosmicDoom.Scripts.Strategies.Weapon;

using Godot;
using Context;
using Interfaces;

public class HitscanStrategy : IWeaponStrategy {
    public void Execute(RAttackContext context) {
        var shots = context.WEAPON.SHOT_COUNT;
        var spreadDegrees = context.WEAPON.SPREAD_DEGREES;
        var ray = context.RAY;
        var originalGlobalTransform = ray.GlobalTransform;
    
        for (int i = 0; i < shots; i++) {
            var spreadQuaternion = Utils.GetSpreadQuaternion(spreadDegrees);
            ray.GlobalTransform = originalGlobalTransform;
            ray.GlobalBasis = new Basis(new Quaternion(ray.GlobalBasis) * spreadQuaternion);
            var collider = ray.GetCollider();
            if (collider is IHittable hittable) hittable.Hit(context.WEAPON.DAMAGE);
        }
    
        ray.GlobalTransform = originalGlobalTransform;
    }
}
