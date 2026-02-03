using Godot;
using System;
using CosmicDoom.Scripts.Interfaces;

public partial class Barrel : StaticBody3D, IHittable {
    public void Hit(int damage) {
        Explode();
    }

    private void Explode() {
        
    }
}
