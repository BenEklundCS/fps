using CosmicDoom.Scripts.Interfaces;
using CosmicDoom.Scripts.Registry;
using Godot;

namespace CosmicDoom.Scripts.Components;

public partial class Controller : Node {
    private IControllable _controlTarget;
    [Export] public NodePath Target;

    public override void _Ready() {
        _controlTarget = GetNode<IControllable>(Target);
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    public override void _PhysicsProcess(double delta) {
        var context = ControllerStrategyRegistry.INSTANCE;
        foreach (var strategy in context.GetContinuousStrategies()) {
            strategy.Execute(null, _controlTarget);
        }
    }

    public override void _Input(InputEvent @event) {
        var context = ControllerStrategyRegistry.INSTANCE;

        if (@event is InputEventMouseMotion motion) {
            context.Get("look").Execute(@event, _controlTarget);
            return;
        }
        
        foreach (var action in context.GetKeys()) {
            if (!@event.IsActionPressed(action)) continue;
            var controllerStrategy = context.Get(action);
            if (controllerStrategy is null) return;
            controllerStrategy.Execute(@event, _controlTarget);
        }
    }
}