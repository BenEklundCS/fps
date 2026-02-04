using Godot;
using System;
using System.Collections.Generic;
using CosmicDoom.Scripts.Entities;
using CosmicDoom.Scripts.Interfaces;
using CosmicDoom.Scripts.Strategies;
using CosmicDoom.Scripts.Strategies.EnemyAI;

public partial class EnemyAiRegistry : Node, IRegistry<EnemyType, IEnemyAiStrategy> {
    private readonly Dictionary<EnemyType, IEnemyAiStrategy> _enemyAiRegistry = new() {
        [EnemyType.Destroyer] = new DefaultStrategy()
    };
        
    public static EnemyAiRegistry INSTANCE { get; private set; }
        
    public IEnemyAiStrategy Get(EnemyType enemyType) {
        return _enemyAiRegistry.GetValueOrDefault(enemyType);
    }

    public IEnumerable<EnemyType> GetKeys() {
        return _enemyAiRegistry.Keys;
    }
    
    public override void _Ready() {
        INSTANCE = this;
    }
}
