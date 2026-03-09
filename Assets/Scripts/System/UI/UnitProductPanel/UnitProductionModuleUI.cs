using System.Collections.Generic;
using UnityEngine;

public class UnitProductionModuleUI: MonoBehaviour
{
    private UnitProductionModule pModule;
    private Animator animator;
    public GameObject buttonPrefab;
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
                RefreshProductionList();
                return;            
            }
        }
        animator.SetBool("isClosed", true);
    }
    public void RefreshProductionList()
    {
        // 删除旧元素
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
        // 新元素
        if(pModule == null) return;
        foreach(var unit in pModule.GetProductableUnits())
        {
            ProductableUnitButton card = Instantiate(buttonPrefab, this.transform).GetComponent<ProductableUnitButton>();
            if(card != null)
            {
                card.desc.text = $"{unit.unitName}";
                card.button.onClick.AddListener(() => {
                    pModule.ProductEnqueue(unit);
                    RefreshProductionList();
                });
            }
        }
    }
}