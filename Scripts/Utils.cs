using System;

namespace CosmicDoom.Scripts;
using Godot;
using static Godot.GD;

public partial class Utils : Node3D {
    private static Random _random = new ();
    
    public static Quaternion GetSpreadQuaternion(float spreadDegrees) {
        var twist = RandRange(0, Mathf.Tau);
        var axis = new Vector3((float)Mathf.Cos(twist), (float)Mathf.Sin(twist), 0);
        var angle = Mathf.DegToRad(spreadDegrees * Mathf.Sqrt(Randf()));
        return new Quaternion(axis, angle);
    }
    
    public static float NextFloat(Vector2 range)
    {
        return range.X + (float)(_random.NextDouble() * (range.Y - range.X));
    }
    
    
}