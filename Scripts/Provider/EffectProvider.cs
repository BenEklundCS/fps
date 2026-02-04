using System.Collections.Generic;

namespace CosmicDoom.Scripts.Registry;

using Godot;
using System;

using Effects;
using Interfaces;

public partial class EffectProvider : Node
{
    private readonly Dictionary<EffectType, PackedScene> _effectRegistry = new() {
        [EffectType.Explosion] = GD.Load<PackedScene>("res://addons/ExplosionVFX/explosion.tscn")
    };
        
    public static EffectProvider INSTANCE { get; private set; }

    public void SpawnEffectAt(EffectType effect, Vector3 position) {
        var explosion = _effectRegistry.GetValueOrDefault(effect).Instantiate<Node3D>();
        explosion.Position = position;
        GetTree().Root.AddChild(explosion);
    } 
    
    public override void _Ready() {
        INSTANCE = this;
    }
}
