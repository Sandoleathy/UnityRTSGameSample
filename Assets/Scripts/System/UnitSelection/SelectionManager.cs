using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance;
    public LayerMask selectorLayer;

    private List<RTSUnit> selectedUnits = new List<RTSUnit>();
    public UnityEvent<List<RTSUnit>> onSelectionChange;

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

    public void SelectUnit(RTSUnit unit)
    {
        if (!selectedUnits.Contains(unit))
        {
            selectedUnits.Add(unit);
            unit.OnSelected();
            onSelectionChange?.Invoke(selectedUnits);
        }
    }

    public void ClearSelection()
    {
        foreach (var unit in selectedUnits)
        {
            unit.DeSelected();
        }
        selectedUnits.Clear();
        onSelectionChange?.Invoke(selectedUnits);
    }

    public List<RTSUnit> GetSelectedUnits()
    {
        return selectedUnits;
    }
}

