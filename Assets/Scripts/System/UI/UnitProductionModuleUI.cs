using System.Collections.Generic;
using UnityEngine;

public class UnitProductionModuleUI: MonoBehaviour
{
    private UnitProductionModule pModule;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        SelectionManager.Instance.onSelectionChange.AddListener(OnUnitSelectionChange);
    }
    public void OnUnitSelectionChange(List<RTSUnit> units)
    {
        foreach(RTSUnit unit in units)
        {
            if(unit.GetComponent<UnitProductionModule>() != null)
            {
                // 显示生产界面
                animator.SetBool("isClosed", false);
                pModule = unit.GetComponent<UnitProductionModule>();
                return;            
            }
        }
        animator.SetBool("isClosed", true);
    }
}