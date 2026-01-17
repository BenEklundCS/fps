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
    float SPREAD_DEGREES, // degrees
    float COOLDOWN,
    CompressedTexture2D TEXTURE,
    IWeaponStrategy STRATEGY
);