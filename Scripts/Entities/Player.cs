using System.Collections.Generic;
using CosmicDoom.Scripts.Interfaces;
using CosmicDoom.Scripts.Items;
using CosmicDoom.Scripts.Registry;

namespace CosmicDoom.Scripts;

using Godot;
using static Godot.GD;
using System;

public partial class Player : Character {
    [Export] public string MoveLeftKeybind = "ui_left";
    [Export] public string MoveRightKeybind = "ui_right";
    [Export] public string MoveUpKeybind = "ui_up";
    [Export] public string MoveDownKeybind = "ui_down";
    [Export] public string JumpKeybind = "ui_accept";
    [Export] public float MouseSensitivity = 0.005f;

    private Camera3D _camera;
    private float _gravity;
    private readonly float _maxPitch = Mathf.DegToRad(85f);

    private readonly List<RWeapon> _weaponWheel = new ();
    private int _weaponIndex;
    private readonly WeaponType[] _defaultWeapons = new[] {
        WeaponType.Knife, 
        WeaponType.PlasmaGun, 
        WeaponType.Shotgun
    };
    private TextureRect _weaponTextureRect;

    public override void _Ready() {
        _camera = GetNode<Camera3D>("Head/Camera3D");
        Input.MouseMode = Input.MouseModeEnum.Visible;
        _gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
        ReadyWeapons();
        base._Ready();
    }

    public override void _PhysicsProcess(double delta) {
        HandleMovement(delta);
        base._PhysicsProcess(delta);
    }

    public override void _Input(InputEvent @event) {
        HandleEscape(@event);
        HandleClick(@event);
        HandleScroll(@event);
        HandleLook(@event);
    }

    private void ReadyWeapons() {
        _weaponTextureRect = GetNode<TextureRect>("UIBar/WeaponTextureRect");
        foreach (var weaponType in _defaultWeapons) {
            _weaponWheel.Add(WeaponRegistry.INSTANCE.Get(weaponType));
        }
        EquipWeapon(_weaponIndex);
    }

    private void EquipWeapon(int weaponIndex) {
        _weaponTextureRect.Texture = _weaponWheel[Mathf.PosMod(weaponIndex, _weaponWheel.Count)].Texture;
    }

    private void Shoot()
    {
        var laser = (Node3D)Load<PackedScene>("res://Scenes/Objects/laser.tscn").Instantiate();
        
        Callable.From(() => {
            GetTree().Root.AddChild(laser);
            // aligns laser to face player dir, dependent on laser being aligned across -z in its scene with one end at 0,0,0
            var forward = -_camera.GlobalTransform.Basis.Z;
            laser.GlobalPosition = _camera.GlobalPosition;
            laser.LookAt(laser.GlobalPosition + forward, Vector3.Up);
        }).CallDeferred();
        
        // raycast
        var collider = Ray.GetCollider();
        Print(collider);
        if (collider is IHittable hittable) {
            hittable.Hit(DAMAGE);
        }
    }
    
    private void HandleMovement(double delta) {
        Velocity = new Vector3(
            Velocity.X, 
            Velocity.Y - _gravity * (float)delta, 
            Velocity.Z
        );

        var input = Input.GetVector(
            MoveLeftKeybind, 
            MoveRightKeybind, 
            MoveUpKeybind, 
            MoveDownKeybind
        );

        var movementDir = Transform.Basis * new Vector3(input.X, 0, input.Y);
        Velocity = new Vector3(
            movementDir.X * Speed, 
            Velocity.Y, 
            movementDir.Z * Speed
        );

        MoveAndSlide();
        if (IsOnFloor() && Input.IsActionJustPressed(JumpKeybind)) {
            Velocity = new Vector3(Velocity.X, JumpSpeed, Velocity.Z);
        }
    }

    private void HandleLook(InputEvent @event) {
        if (Input.MouseMode != Input.MouseModeEnum.Captured) {
            return;
        }
        if (@event is InputEventMouseMotion mouseMotion) {
            RotateY(-mouseMotion.Relative.X * MouseSensitivity);
            Head.RotateX(-mouseMotion.Relative.Y * MouseSensitivity);
            var clampedX = Math.Clamp(
                Head.Rotation.X,
                -_maxPitch,
                _maxPitch
            );
            Head.Rotation = new Vector3(clampedX, Head.Rotation.Y, Head.Rotation.Z);
        }
    }

    private void HandleScroll(InputEvent @event) {
        if (Input.MouseMode != Input.MouseModeEnum.Captured) {
            return;
        }

        if (@event.IsActionPressed("weapon_wheel_up")) {
            EquipWeapon(++_weaponIndex);
        }
        else if (@event.IsActionPressed("weapon_wheel_down")) {
            EquipWeapon(--_weaponIndex);
        }
    }

    private void HandleClick(InputEvent @event) {
        if (@event.IsActionPressed("click")) {
            if (Input.MouseMode == Input.MouseModeEnum.Visible) {
                Input.MouseMode = Input.MouseModeEnum.Captured;
            }
            else {
                Shoot();
            }
        }
    }

    private void HandleEscape(InputEvent @event) {
        if (@event.IsActionPressed("ui_cancel")) {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
    }
}
