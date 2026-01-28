using CosmicDoom.Scripts.Interfaces;
using Godot;

namespace CosmicDoom.Scripts.Items;
using Strategies;

public enum WeaponType {
    Knife,
    MachineGun,
    PlasmaGun,
    RocketLauncher,
    Shotgun,
    Solution,
    None
}

public record RWeapon(
    WeaponType TYPE,
    int DAMAGE,
    int SHOT_COUNT, // shots per round
    int AMMO,
    int MAX_AMMO,
    float SPREAD_DEGREES, // degrees
    float COOLDOWN,
    CompressedTexture2D TEXTURE,
    CompressedTexture2D ON_USE_TEXTURE,
    AudioStreamWav[] AUDIO_STREAMS,
    IProjectile PROJECTILE,
    IWeaponStrategy STRATEGY
);