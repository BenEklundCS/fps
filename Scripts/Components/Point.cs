using Godot;
using System;

public partial class Point : Node3D {
    public override void _Ready() {
        AddToGroup("points");
    }
}
