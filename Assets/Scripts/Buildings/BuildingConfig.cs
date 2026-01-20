using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Buildings/BuildingConfig")]
public class BuildingConfig : ScriptableObject
{
    public string id;
    public string buildingName;
    public float maxHP;
    public float buildTime;

    //将炮塔或武器的prefab绑定到配置文件中即可
    public List<Turrent> turrents;
    public List<Weapon> weapons;
}

public enum BuildingType
{
    Resource,
    Defense,
    Production,
    Utility
}