using System.Collections.Generic;

namespace CosmicDoom.Scripts.Items;

public class MagazineFeed {
    private readonly Queue<int> _reserve = new();
    private int _ammoPerMagazine;
    private int _refillMax;
    public int AMMO_IN_MAG { get; private set; }
    public int RESERVE_COUNT => _reserve.Count;

    public MagazineFeed(int ammoPerMagazine, int maxAmmo) {
        _ammoPerMagazine = ammoPerMagazine;

        var maxMagazines = maxAmmo / ammoPerMagazine;
        var remaining = maxAmmo;

        if (remaining >= ammoPerMagazine) {
            AMMO_IN_MAG = ammoPerMagazine;
            remaining -= ammoPerMagazine;
        }

        while (remaining >= ammoPerMagazine && _reserve.Count < maxMagazines) {
            _reserve.Enqueue(ammoPerMagazine);
            remaining -= ammoPerMagazine;
        }

        _refillMax = _reserve.Count;
    }

    public void Shoot() => AMMO_IN_MAG--;

    public bool Reload() {
        if (_reserve.Count == 0) return false;
        AMMO_IN_MAG = _reserve.Dequeue();
        return true;
    }

    public void RefillReserve() {
        while (_reserve.Count < _refillMax) {
            _reserve.Enqueue(_ammoPerMagazine);
        }
    }
}
