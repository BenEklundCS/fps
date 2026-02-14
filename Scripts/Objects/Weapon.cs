using System.Collections.Generic;
using System.Threading;
using CosmicDoom.Scripts.Entities;
using CosmicDoom.Scripts.Registry;

namespace CosmicDoom.Scripts.Objects;

using Godot;
using static Godot.GD;
using Interfaces;
using Items;
using Components;
using Context;

public partial class Weapon : Node3D, IWeapon {
    private readonly Dictionary<WeaponType, MagazineFeed> _magazineFeeds = new ();
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
            _ammoBg = GetNode<ColorRect>("UI/AmmoBg");
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
    
    public void InitializeFeed(RWeapon rWeapon) {
        if (!_magazineFeeds.ContainsKey(rWeapon.TYPE) && rWeapon.AMMO > 0) {
            _magazineFeeds[rWeapon.TYPE] = new MagazineFeed(rWeapon.AMMO, rWeapon.MAX_AMMO);
        }
    }

    private void EnsureInitialized() {
        InitializeFeed(_rWeapon);

        if (!_audioStreamRandomizers.ContainsKey(_rWeapon.TYPE)) {
            InitializeAudio();
        }

        _audio.Stream = _audioStreamRandomizers[_rWeapon.TYPE];
    }

    private void InitializeAudio() {
        var randomizer = new AudioStreamRandomizer();
        for (var i = 0; i < _rWeapon.AUDIO_STREAMS.Length; i++) {
            randomizer.AddStream(i, _rWeapon.AUDIO_STREAMS[i]);
        }
        _audioStreamRandomizers[_rWeapon.TYPE] = randomizer;
    }

    public void PickupAmmo(Pickup pickup) {
        foreach (var (weaponType, feed) in _magazineFeeds) {                                                                                                                                                                          
            if (WeaponRegistry.INSTANCE.Get(weaponType).AMMO_TYPE == pickup.Type) {
                feed.RefillReserve();
            }
        }
        UpdateCurrentMagazine(false);
    }
    
    private void Reload() {
        if (_magazineFeeds[_rWeapon.TYPE].Reload()) {
            UpdateCurrentMagazine(false);
        }
        else {
            Print("Out of ammo!");
        }
    }
    
    // updates the magazine on-equip or on-use of a weapon
    // if shot is true, deduct from the mag
    private void UpdateCurrentMagazine(bool shot) {
        if (!_magazineFeeds.TryGetValue(_rWeapon.TYPE, out var feed)) {
            UpdateAmmoLabel(0, 0, false);
            return;
        }

        if (shot) feed.Shoot();
        UpdateAmmoLabel(feed.AMMO_IN_MAG, feed.RESERVE_COUNT, true);
    }

    private int GetAmmo() {
        return _magazineFeeds.TryGetValue(_rWeapon.TYPE, out var feed) ? feed.AMMO_IN_MAG : 0;
    }

    private void UpdateGunTexture() {
        if (!_hasUi) return;
        _weaponRect.Texture = _rWeapon.TEXTURE;
        _onUseRect.Texture = _rWeapon.ON_USE_TEXTURE;
        _weaponIconRect.Texture = _rWeapon.ICON;
    }

    private void UpdateAmmoLabel(int ammo, int magCount, bool visible) {
        if (!_hasUi) return;
        _ammoLabel.Visible = visible;
        _ammoBg.Visible = visible;
        _ammoLabel.Text = $"{ammo} / {magCount}";
    }

    private void UpdateCooldown() {
        _cooldownTimer.Stop();
        _cooldownTimer.SetWaitTime(_rWeapon.COOLDOWN);
    }
}