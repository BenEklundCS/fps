using System;
using Godot.Collections;

namespace CosmicDoom.Scripts;
using Godot;
using static Godot.GD;

public partial class Utils : Node3D {
    private static Random _random = new ();
    public static Utils INSTANCE { get; private set; }

    public override void _Ready() {
        INSTANCE = this;
    }

    public Quaternion GetSpreadQuaternion(float spreadDegrees) {
        var twist = RandRange(0, Mathf.Tau);
        var axis = new Vector3((float)Mathf.Cos(twist), (float)Mathf.Sin(twist), 0);
        var angle = Mathf.DegToRad(spreadDegrees * Mathf.Sqrt(Randf()));
        return new Quaternion(axis, angle);
    }
    
    public float NextFloat(Vector2 range)
    {
        return range.X + (float)(_random.NextDouble() * (range.Y - range.X));
    }

    public T RandomElement<T>(T[] array) {
        return array[_random.Next(array.Length)];
    }

    public Dictionary IntersectRayOnPath(Vector3 from, Vector3 to) {
        var spaceState = GetWorld3D().DirectSpaceState;
        var query = PhysicsRayQueryParameters3D.Create(from, to);
        var result = spaceState.IntersectRay(query);

        return result;
    }
}