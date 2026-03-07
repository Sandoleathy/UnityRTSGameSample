using UnityEngine;

public class UIManager: MonoBehaviour
{
    public static UIManager Instance;
    // 管理所有的UI类
    public BattleResourceUI battleResourceUI;
    public UnitProductionModuleUI unitProductionModulePanel;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}