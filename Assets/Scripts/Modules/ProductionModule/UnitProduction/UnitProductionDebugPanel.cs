using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class UnitProductionDebugPanel: MonoBehaviour
{
    public RTSUnit owner;
    public UnitProductionModule productionModule;

    [Header("Debug: All Unit Configs")]
    public List<RTSUnitConfig> allUnitConfigs;
    private bool isVisible = false;


    // —— 对外接口 ——
    public void Open(RTSUnit unit)
    {
        owner = unit;
        productionModule = owner.moduleContainer.Get<UnitProductionModule>();
        isVisible = productionModule != null;
    }

    public void Close()
    {
        isVisible = false;
        owner = null;
        productionModule = null;
    }

    private void OnGUI()
    {
        if (productionModule == null || !isVisible) return;

        GUILayout.BeginArea(new Rect(20, 20, 200, 400), "Production", GUI.skin.window);
        GUILayout.Label($"生产模块调试面板 - {owner.name}");

        GUILayout.Space(10);

        foreach (var unit in allUnitConfigs)
        {
            if (GUILayout.Button(unit.unitName))
            {
                productionModule.ProductEnqueue(unit);
            }
        }
        GUILayout.Space(10);
        GUILayout.Label("生产队列");
        
        int i = 0;
        foreach(var unit in productionModule.GetProductQueue())
        {
            i++;
            if(i == 1){ GUILayout.Label($"{unit.unitName}  {productionModule.getUnitProducingCountDown():F1}"); continue; }

            GUILayout.Label($"{unit.unitName}  {unit.productTime}");
        }

        if (GUILayout.Button("Close"))
        {
            Close();
            GUILayout.EndArea();
            return;
        }

        GUILayout.EndArea();
    }

}