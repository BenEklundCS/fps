namespace CosmicDoom.Scripts;
using Godot;
using static Godot.GD;

public class Utils {
    public static Quaternion GetSpreadQuaternion(float spreadDegrees) {
        var twist = RandRange(0, Mathf.Tau);
        var axis = new Vector3((float)Mathf.Cos(twist), (float)Mathf.Sin(twist), 0);
        var angle = Mathf.DegToRad(spreadDegrees * Mathf.Sqrt(Randf()));
        return new Quaternion(axis, angle);
    }
}