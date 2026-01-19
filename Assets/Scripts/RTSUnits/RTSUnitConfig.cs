using UnityEngine;

[CreateAssetMenu( menuName = "RTSUnit/RTSUnitConfig")]
public class RTSUnitConfig: ScriptableObject{
    [Header("基础信息")]
    public string id;
    public string unitName;
    [Header("属性")]
    public float maxHP;
    public float maxMoveSpeed;
    public float maxRotateSpeed;
    public bool canAttackWhileMove;
    public float viewRange;
    public float alertRange;
    [Header("单位类型")]
    public RTSUnitType type;

}

public enum RTSUnitType
{
    GroundUnit,
    AirUnit,
}