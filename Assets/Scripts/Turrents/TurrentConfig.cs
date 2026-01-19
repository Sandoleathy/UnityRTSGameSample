using UnityEngine;

[CreateAssetMenu( menuName = "Turrent/TurrentConfig")]
public class TurrentConfig: ScriptableObject{
    [Header("基础信息")]
    public string id;
    public string turrentName;
    [Header("属性")]
    public float maxRotateSpeed;
}