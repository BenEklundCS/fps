namespace CosmicDoom.Scripts.Objects;

using Godot;
using static Godot.GD;
using Interfaces;
using Items;
using Context;

public partial class Weapon : Node3D, IWeapon {
    private AudioStreamPlayer3D _audio;
    private TextureRect _rect;
    private RWeapon _rWeapon;
    private Timer _cooldownTimer;
    public override void _Ready() {
        _audio = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");
        _rect = GetNode<TextureRect>("TextureRect");
        _cooldownTimer = GetNode<Timer>("CooldownTimer");
    }

    public void Equip(RWeapon rWeapon) {
        _rWeapon = rWeapon;
        _rect.Texture = rWeapon.TEXTURE;

        var randomizer = new AudioStreamRandomizer();
        for (int i = 0; i < _rWeapon.AUDIO_STREAMS.Length; i++) {
            randomizer.AddStream(i, _rWeapon.AUDIO_STREAMS[i]);
        }
        _audio.Stream = randomizer;
        Print(randomizer);
        
        _cooldownTimer.Stop();
        _cooldownTimer.SetWaitTime(rWeapon.COOLDOWN);
    }

    public void Use(RAttackContext context) {
        if (_cooldownTimer.IsStopped()) {
            _rWeapon.STRATEGY.Execute(context);
            _audio.Play();
            _cooldownTimer.Start();
        }
    }
}