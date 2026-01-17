using System;
using CosmicDoom.Scripts.Interfaces;
using CosmicDoom.Scripts.Items;
using CosmicDoom.Scripts.Objects;
using Godot;
using static Godot.GD;

namespace CosmicDoom.Scripts.Strategies.Weapon;

public class HitscanStrategy : IWeaponStrategy {
    private Laser _laserFactory = new();
    
    public void Execute(RAttackContext context) {
        var shots = context.WEAPON.SHOT_COUNT;
        var spreadDegrees = context.WEAPON.SPREAD_DEGREES;
        var ray = context.RAY;
        var originalGlobalTransform = ray.GlobalTransform;
    
        for (int i = 0; i < shots; i++) {
            var laser = _laserFactory.Spawn();
            var spreadQuaternion = GetSpreadQuaternion(spreadDegrees);
            
            ray.GlobalTransform = originalGlobalTransform;
            ray.GlobalBasis = new Basis(new Quaternion(ray.GlobalBasis) * spreadQuaternion);
        
            var collider = ray.GetCollider();

            laser.SetContext(context);
            ray.GetTree().Root.AddChild(laser);

            laser.GlobalTransform = ray.GlobalTransform;
            if (collider is IHittable hittable) hittable.Hit(context.WEAPON.DAMAGE);
        }
    
        ray.GlobalTransform = originalGlobalTransform;
    }

    private static Quaternion GetSpreadQuaternion(float spreadDegrees) {
        var twist = RandRange(0, Mathf.Tau);
        var axis = new Vector3((float)Mathf.Cos(twist), (float)Mathf.Sin(twist), 0);
        var angle = Mathf.DegToRad(spreadDegrees * Mathf.Sqrt(Randf()));
        return new Quaternion(axis, angle);
    }
}
