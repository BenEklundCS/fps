using CosmicDoom.Scripts.Entities;
using CosmicDoom.Scripts.Interfaces;
using Godot;
using static Godot.GD;

namespace CosmicDoom.Scripts.Components;

public partial class Controller : Node {
    #nullable enable
    private IControllable? _controlTarget;
    [Export] public required NodePath Target;

    private static bool IS_ACTIVE => Input.MouseMode == Input.MouseModeEnum.Captured;

    public override void _Ready() {
        _controlTarget = GetNode<IControllable>(Target);
        if (_controlTarget is Character character) {
            character.OnDeath += OnCharacterDeath;
        }
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    public override void _Process(double delta) {
        if (Input.IsActionPressed("click")) {
            if (!IS_ACTIVE) {
                Input.MouseMode = Input.MouseModeEnum.Captured;
                return;
            }
            _controlTarget?.Attack();
        }
    }

    public override void _PhysicsProcess(double delta) {
        if (!IS_ACTIVE) return;

        var input = Input.GetVector("left", "right", "up", "down");
        _controlTarget?.Move(new Vector3(input.X, 0, input.Y));
    }

    public override void _Input(InputEvent @event) {
        if (@event is InputEventMouseMotion motion) {
            if (IS_ACTIVE) _controlTarget?.Look(motion.Relative);
            return;
        }

        if (@event.IsActionPressed("escape")) {
            Input.MouseMode = Input.MouseModeEnum.Visible;
            return;
        }

        if (!IS_ACTIVE) return;

        if (@event.IsActionPressed("jump")) _controlTarget?.Jump();
        if (@event.IsActionPressed("wheel_up")) _controlTarget?.NextWeapon();
        if (@event.IsActionPressed("wheel_down")) _controlTarget?.PrevWeapon();
    }

    private void OnCharacterDeath() {
        _controlTarget = null;
    }
}