using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [SerializeField] Weapon[] characterWeapons;

    public Weapon GetWeaponAtIndex(int index)
    {
        return characterWeapons[index];
    }
}