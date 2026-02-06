using UnityEngine;
using System.Collections.Generic;

public class ProductionDebugPanel: MonoBehaviour
{
    public RTSUnit owner;
    public ProductionModule productionModule;

    [Header("Debug: All Unit Configs")]
    public List<RTSUnitConfig> allUnitConfigs;


    private void OnGUI()
    {
        if (productionModule == null) return;

        GUILayout.BeginArea(new Rect(20, 20, 200, 400), "Production", GUI.skin.window);

        foreach (var unit in allUnitConfigs)
        {
            if (GUILayout.Button(unit.unitName))
            {
                productionModule.ProductEnqueue(unit);
            }
        }

        GUILayout.EndArea();
    }
}