namespace CosmicDoom.Scripts.Items;

public struct Magazine {
    public Magazine(int mag, int capacity) {
        Mag = mag;
        Capacity = capacity;
    }

    public int Mag { get; set; }
    public int Capacity { get; private set; }

    public override string ToString() => $"{Mag}";
}