using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "ScriptableObjects/UnitStats", order = 1)]
public class UnitStats : ScriptableObject
{

    [Header("Movement")]
    public int movementRange;
    public float movementSpeed;

    [Header("Combat")]
    public GameObject killParticleFx;
    public int attackRange;
    public DamageIcon damageIconPrefab;
    public int numberOfAttacks;

    [Header("Stats")]
    public int health;
    public int armor;
    public int attackDamage;
    public int defenseDamage;

}
