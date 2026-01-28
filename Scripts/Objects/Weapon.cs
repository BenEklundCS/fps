using System.Collections.Generic;
using System.Threading;

namespace CosmicDoom.Scripts.Objects;

using Godot;
using static Godot.GD;
using Interfaces;
using Items;
using Context;

public partial class Weapon : Node3D, IWeapon {
    private Dictionary<WeaponType, LinkedListNode<Magazine>> _currentMagazines = new ();
    
    private AudioStreamPlayer3D _audio;
    private TextureRect _weaponRect;
    private TextureRect _onUseRect;
    private RWeapon _rWeapon;
    private Timer _cooldownTimer;
    private Timer _reloadTimer;
    private Timer _onUseRectVisibilityTimer;
    private Label _ammoLabel;
    
    public override void _Ready() {
        _audio = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");
        _weaponRect = GetNode<TextureRect>("UI/WeaponTexture");
        _onUseRect = GetNode<TextureRect>("UI/OnUseTexture");
        _cooldownTimer = GetNode<Timer>("CooldownTimer");
        _reloadTimer = GetNode<Timer>("ReloadTimer");
        _reloadTimer.Timeout += Reload;
        _onUseRectVisibilityTimer = GetNode<Timer>("OnUseRectVisibilityTimer");
        _onUseRectVisibilityTimer.Timeout += () => { _onUseRect.Visible = false; };
        _ammoLabel = GetNode<Label>("UI/Ammo");
    }

    public void Equip(RWeapon rWeapon) {
        if (!_reloadTimer.IsStopped()) {
            Print("Cannot switch weapons while reloading!");
            return;
        }
        
        _rWeapon = rWeapon;
        _weaponRect.Texture = rWeapon.TEXTURE;
        _onUseRect.Texture = rWeapon.ON_USE_TEXTURE;
        
        // initialize ammo if first equip
        if (!_currentMagazines.ContainsKey(rWeapon.TYPE)) {
            var newMagazineFeed = new LinkedList<Magazine>();
            var maxAmmo = rWeapon.MAX_AMMO;
            var ammoPerMagazine = rWeapon.AMMO;
            while (maxAmmo >= ammoPerMagazine && maxAmmo > 0) {
                maxAmmo -= ammoPerMagazine;
                var magazine = new Magazine(ammoPerMagazine, ammoPerMagazine);
                newMagazineFeed.AddLast(magazine);
            }
            _currentMagazines[rWeapon.TYPE] = newMagazineFeed.First;
        }
        
        UpdateAmmo(false);

        var randomizer = new AudioStreamRandomizer();
        for (int i = 0; i < _rWeapon.AUDIO_STREAMS.Length; i++) {
            randomizer.AddStream(i, _rWeapon.AUDIO_STREAMS[i]);
        }
        _audio.Stream = randomizer;
        
        _cooldownTimer.Stop();
        _cooldownTimer.SetWaitTime(rWeapon.COOLDOWN);
    }

    public void Use(RAttackContext context) {
        var ammo = GetAmmo(_rWeapon.TYPE);
        
        if (ammo > 0) {
            if (_cooldownTimer.IsStopped()) {
                _rWeapon.STRATEGY.Execute(context);
                _cooldownTimer.Start();
                _onUseRectVisibilityTimer.Start();
                _onUseRect.Visible = true;
                UpdateAmmo(true);
            }
        }
        else if (_reloadTimer.IsStopped()) {
            Print("Reloading!");
            _reloadTimer.Start();
        }
    }

    private void Reload() {
        var node = _currentMagazines[_rWeapon.TYPE];
        if (node == null) return;
        var nextNode = node.Next;
        if (nextNode != null) {
            _currentMagazines[_rWeapon.TYPE] = nextNode;
            UpdateAmmo(false);
        }
        else {
            Print("Out of ammo!");
        }
    }

    private int GetAmmo(WeaponType weaponType) {
        var node = _currentMagazines[_rWeapon.TYPE];
        if (node == null) return 0;
        return node.Value.Mag;
    }

    private int GetRemainingMagCount(LinkedListNode<Magazine> node) {
        int count = 0;
        while (node.Next != null) {
            count += 1;
            node = node.Next;
        }
        return count;
    }

    private void UpdateAmmo(bool shot) {
        var node = _currentMagazines[_rWeapon.TYPE];
        if (node == null) return;
        if (shot) {
            node.ValueRef.Mag -= 1;
        }
        _ammoLabel.Text = $"{node.Value.Mag} / {GetRemainingMagCount(node)}";
    }
}