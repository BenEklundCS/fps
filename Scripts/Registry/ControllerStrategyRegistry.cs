using System.Collections.Generic;
using CosmicDoom.Scripts.Interfaces;
using CosmicDoom.Scripts.Strategies;
using CosmicDoom.Scripts.Strategies.Controller;
using Godot;

namespace CosmicDoom.Scripts.Registry;

public partial class ControllerStrategyRegistry : Node, IRegistry<StringName, IControllerStrategy> {
    private static readonly ControllerScrollStrategy ScrollStrategy = new();
    private static readonly ControllerLookStrategy LookStrategy = new();
    private readonly Dictionary<StringName, IControllerStrategy> _actionStrategies = new() {
        ["escape"] = new ControllerEscapeStrategy(),
        ["jump"] = new ControllerJumpStrategy(),
        ["click"] = new ControllerAttackStrategy(),
        ["wheel_up"] = ScrollStrategy,
        ["wheel_down"] = ScrollStrategy,
    };
    
    private readonly List<IControllerStrategy> _continuousStrategies = new() {
        new ControllerMoveStrategy("left", "right", "up", "down"),
    };

    public static ControllerStrategyRegistry INSTANCE { get; private set; }

    public IControllerStrategy Get(StringName key) {
        if (key == "look") {
            return LookStrategy;
        }
        return _actionStrategies.GetValueOrDefault(key);
    }

    public IEnumerable<StringName> GetKeys() {
        return _actionStrategies.Keys;
    }

    public IEnumerable<IControllerStrategy> GetContinuousStrategies() {
        return _continuousStrategies;
    }

    public override void _Ready() {
        INSTANCE = this;
    }
}