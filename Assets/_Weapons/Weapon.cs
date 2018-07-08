using UnityEngine;

/// <summary>
/// Both skill and weapon have stat values. Weapon holds the base values and skill will add to these values once they have been increased
/// </summary>
[CreateAssetMenu(menuName = "RPG/Weapons/Weapon")]
public class Weapon : ScriptableObject 
{
    [SerializeField] private float attackRange;
    public float AttackRange
    {
        get
        {
            return attackRange;
        }

        set
        {
            attackRange = value;
        }
    }

    [SerializeField] private GameObject weaponPrefab = null;
    public GameObject WeaponObject
    {
        get
        {
            return weaponPrefab;
        }
    }

    [SerializeField] private string animationName = string.Empty;
    public string AnimationName
    {
        get { return animationName; }
    }

    [SerializeField] private bool isProjectile = false;
    public bool IsProjectile
    {
        get { return isProjectile; }
    }
}
