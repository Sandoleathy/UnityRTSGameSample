using UnityEngine;

[CreateAssetMenu( menuName = "RTSUnit/RTSUnitConfig")]
public class RTSUnitConfig: ScriptableObject{
    [Header("基础信息")]
    public string id;
    public string unitName;
    [Header("属性")]
    public bool canAttackWhileMove;
    public float viewRange;
    public float alertRange;
    public float productTime;
    [Header("单位类型")]
    public RTSUnitType type;
    [Header("预制件")]
    public GameObject prefab;

}

public enum RTSUnitType
{
    GroundUnit,
    AirUnit,
}