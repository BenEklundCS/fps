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
    PickupType AMMO_TYPE,
    int DAMAGE,
    bool RELOAD_ENABLED,
    int AMMO,
    int MAX_AMMO,
    float COOLDOWN,
    AtlasTexture TEXTURE,
    AtlasTexture ON_USE_TEXTURE,
    AtlasTexture ICON,
    AudioStreamWav[] AUDIO_STREAMS,
    IWeaponStrategy STRATEGY,
    Vector3? SHOT_OFFSET = null
) {
    // Default offset: down and forward from camera to gun barrel
    public Vector3 ShotOffset => SHOT_OFFSET ?? new Vector3(0, -0.3f, -0.5f);
}