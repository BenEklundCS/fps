using Godot;

namespace CosmicDoom.Scripts.Items;

public enum PickupCategory {
    Ammo, // Bullets/Plasma/Rockets
    Life, // Health/Shield
}

public enum PickupType {
    None,
    // Ammo
    Bullets,
    Plasma,
    Rockets,
    // Life
    Health
}

public record RPickup(
    PickupType TYPE,
    PickupCategory CATEGORY,
    Texture2D TEXTURE
);