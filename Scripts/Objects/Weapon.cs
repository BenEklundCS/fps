using System.Collections.Generic;
using System.Threading;
using CosmicDoom.Scripts.Entities;

namespace CosmicDoom.Scripts.Objects;

using Godot;
using static Godot.GD;
using Interfaces;
using Items;
using Components;
using Context;

public partial class Weapon : Node3D, IWeapon {
    private readonly Dictionary<WeaponType, LinkedListNode<Magazine>> _currentMagazines = new ();
    private readonly Dictionary<WeaponType, AudioStreamRandomizer> _audioStreamRandomizers = new ();
    
    private AudioStreamPlayer3D _audio;
    private TextureRect _weaponRect;
    private TextureRect _onUseRect;
    private TextureRect _weaponIconRect;
    private RWeapon _rWeapon;
    private Timer _cooldownTimer;
    private Timer _reloadTimer;
    private Timer _onUseRectVisibilityTimer;
    private Label _ammoLabel;
    private ColorRect _ammoBg;
    private FlashRed _iconFlash;
    private bool _hasUi;

    public override void _Ready() {
        _audio = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");
        _cooldownTimer = GetNode<Timer>("CooldownTimer");
        _reloadTimer = GetNode<Timer>("ReloadTimer");
        _reloadTimer.Timeout += Reload;

        _hasUi = GetParent() is Player;
        var ui = GetNodeOrNull<Control>("UI");
        if (_hasUi) {
            _weaponRect = GetNode<TextureRect>("UI/WeaponTexture");
            _onUseRect = GetNode<TextureRect>("UI/OnUseTexture");
            _weaponIconRect = GetNode<TextureRect>("UI/BoxContainer/WeaponIcon");
            _onUseRectVisibilityTimer = GetNode<Timer>("OnUseRectVisibilityTimer");
            _onUseRectVisibilityTimer.Timeout += () => { _onUseRect.Visible = false; };
            _ammoLabel = GetNode<Label>("UI/Ammo");
            _ammoLabel.LabelSettings = new LabelSettings {
                FontSize = 16,
                OutlineSize = 3,
                OutlineColor = Colors.Black
            };
            _ammoLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _ammoLabel.VerticalAlignment = VerticalAlignment.Center;

            _ammoBg = new ColorRect();
            _ammoBg.Color = new Color(0.1f, 0.1f, 0.1f, 0.7f);
            _ammoBg.MouseFilter = Control.MouseFilterEnum.Ignore;
            _ammoBg.AnchorTop = 1f;
            _ammoBg.AnchorBottom = 1f;
            _ammoBg.OffsetLeft = 20f;
            _ammoBg.OffsetTop = _ammoLabel.OffsetTop;
            _ammoBg.OffsetRight = 220f;
            _ammoBg.OffsetBottom = _ammoLabel.OffsetTop + 24f;
            ui.AddChild(_ammoBg);
            ui.MoveChild(_ammoBg, _ammoLabel.GetIndex());
            _iconFlash = GetNode<FlashRed>("UI/BoxContainer/WeaponIcon/FlashRed");
        } else {
            ui?.QueueFree();
            GetNodeOrNull<Timer>("OnUseRectVisibilityTimer")?.QueueFree();
        }
    }

    public void FlashIcon() {
        _iconFlash?.Trigger();
    }

    public void Equip(RWeapon rWeapon) {
        if (!_reloadTimer.IsStopped()) {
            Print("Cannot switch weapons while reloading!");
            return;
        }
        
        _rWeapon = rWeapon;
        
        UpdateGunTexture();
        EnsureInitialized();
        UpdateCurrentMagazine(false);
        UpdateCooldown();
    }

    public bool Use(RAttackContext context) {
        var ammo = GetAmmo();

        if (ammo > 0) {
            if (_cooldownTimer.IsStopped()) {
                _rWeapon.STRATEGY.Execute(context);
                _cooldownTimer.Start();
                if (_hasUi) {
                    _onUseRectVisibilityTimer.Start();
                    _onUseRect.Visible = true;
                }
                UpdateCurrentMagazine(true);
                return true;
            }
        }
        else if (_reloadTimer.IsStopped() && _rWeapon.RELOAD_ENABLED) {
            Print("Reloading!");
            _reloadTimer.Start();
        }
        return false;
    }

    public bool OnCooldown() {
        return !_cooldownTimer.IsStopped();
    }
    
    private void EnsureInitialized() {
        if (!_currentMagazines.ContainsKey(_rWeapon.TYPE)) {
            InitializeMagazines();
        }
    
        if (!_audioStreamRandomizers.ContainsKey(_rWeapon.TYPE)) {
            InitializeAudio();
        }
    
        _audio.Stream = _audioStreamRandomizers[_rWeapon.TYPE];
    }

    private void InitializeMagazines() {
        var newMagazineFeed = new LinkedList<Magazine>();
        var maxAmmo = _rWeapon.MAX_AMMO;
        var ammoPerMagazine = _rWeapon.AMMO;
        while (maxAmmo >= ammoPerMagazine && maxAmmo > 0) {
            maxAmmo -= ammoPerMagazine;
            var magazine = new Magazine(ammoPerMagazine, ammoPerMagazine);
            newMagazineFeed.AddLast(magazine);
        }
        _currentMagazines[_rWeapon.TYPE] = newMagazineFeed.First;
    }

    private void InitializeAudio() {
        var randomizer = new AudioStreamRandomizer();
        for (var i = 0; i < _rWeapon.AUDIO_STREAMS.Length; i++) {
            randomizer.AddStream(i, _rWeapon.AUDIO_STREAMS[i]);
        }
        _audioStreamRandomizers[_rWeapon.TYPE] = randomizer;
    }
    
    private void Reload() {
        var node = _currentMagazines[_rWeapon.TYPE];
        if (node == null) return;
        var nextNode = node.Next;
        if (nextNode != null) {
            _currentMagazines[_rWeapon.TYPE] = nextNode;
            UpdateCurrentMagazine(false);
        }
        else {
            Print("Out of ammo!");
        }
    }
    
    private int GetAmmo() {
        var node = _currentMagazines[_rWeapon.TYPE];
        return (node == null) ? 0 : node.Value.Mag;
    }


    // updates the magazine on-equip or on-use of a weapon
    // if shot is true, deduct from the mag
    private void UpdateCurrentMagazine(bool shot) {
        var node = _currentMagazines.GetValueOrDefault(_rWeapon.TYPE);
        if (node == null) {
            UpdateAmmoLabel(default, 0, false);
            return;
        }
        if (shot) {
            node.ValueRef.Mag -= 1;
        }
        var count = GetRemainingMagCount(node);
        UpdateAmmoLabel(node.Value, count, true);
    }

    private void UpdateGunTexture() {
        if (!_hasUi) return;
        _weaponRect.Texture = _rWeapon.TEXTURE;
        _onUseRect.Texture = _rWeapon.ON_USE_TEXTURE;
        _weaponIconRect.Texture = _rWeapon.ICON;
    }

    private void UpdateAmmoLabel(Magazine mag, int magCount, bool visible) {
        if (!_hasUi) return;
        _ammoLabel.Visible = visible;
        _ammoBg.Visible = visible;
        _ammoLabel.Text = $"{mag.Mag} / {magCount}";
    }

    private void UpdateCooldown() {
        _cooldownTimer.Stop();
        _cooldownTimer.SetWaitTime(_rWeapon.COOLDOWN);
    }
    
    private static int GetRemainingMagCount(LinkedListNode<Magazine> node) {
        var count = 0;
        while (node.Next != null) {
            count += 1;
            node = node.Next;
        }
        return count;
    }
}