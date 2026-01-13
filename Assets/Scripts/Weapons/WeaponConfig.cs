using UnityEngine;

[CreateAssetMenu( menuName = "Weapon/WeaponConfig")]
public class WeaponConfig : ScriptableObject {
    [Header("基础信息")]
    public string id;
    public string name;
    [Header("属性")]
    public float damage;
    public float attackRange;
    /// <summary>
    /// AttackInterval = 1 / AttackSpeed
    /// </summary>
    [Tooltip("攻击间隔 = 1 / 攻击速度")]
    public float attackSpeed;
    [Header("攻击类型")]
    public AttackType attackType;
}