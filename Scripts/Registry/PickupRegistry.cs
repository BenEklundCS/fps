using System.Collections.Generic;
using CosmicDoom.Scripts.Interfaces;
using CosmicDoom.Scripts.Items;
using Godot;

namespace CosmicDoom.Scripts.Registry;

using static GD;

public partial class PickupRegistry : Node, IRegistry<PickupType, RPickup> {
    private readonly Dictionary<PickupType, RPickup> _pickupRegistry = new() {
        [PickupType.Bullets] = new RPickup(
            PickupType.Bullets,
            PickupCategory.Ammo,
            Utils.LoadTrimmed("res://Assets/Sprites/Items/item_bullets.png")
        ),
        [PickupType.Plasma] = new RPickup(
            PickupType.Plasma,
            PickupCategory.Ammo,
            Utils.LoadTrimmed("res://Assets/Sprites/Items/item_plasma.png")
        ),
        [PickupType.Rockets] = new RPickup(
            PickupType.Rockets,
            PickupCategory.Ammo,
            Utils.LoadTrimmed("res://Assets/Sprites/Items/item_rockets.png")
        ),
        [PickupType.Health] = new RPickup(
            PickupType.Health,
            PickupCategory.Life,
            Utils.LoadTrimmed("res://Assets/Sprites/Items/item_health.png")
        )
    };

    public static PickupRegistry INSTANCE { get; private set; }

    public RPickup Get(PickupType pickupType) {
        return _pickupRegistry.GetValueOrDefault(pickupType);
    }

    public IEnumerable<PickupType> GetKeys() {
        return _pickupRegistry.Keys;
    }

    public override void _Ready() {
        INSTANCE = this;
    }
}
