using CosmicDoom.Scripts.Registry;

namespace CosmicDoom.Scripts.Objects;

using Godot;
using Items;

public partial class Pickup : CharacterBody3D {
    [Export] public PickupType Type;
    public PickupCategory Category;
    
    private Sprite3D _sprite;
    

    public override void _Ready() {
        var pickupData = PickupRegistry.INSTANCE.Get(Type);
        Category = pickupData.CATEGORY;
        _sprite = GetNode<Sprite3D>("Sprite3D");
        _sprite.Texture = pickupData.TEXTURE;
    }
}
