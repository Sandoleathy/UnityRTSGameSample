using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(OutlineObject))]
public class RTSUnit : MonoBehaviour
{
    [Header("单位配置")]
    public RTSUnitConfig config;
    public string unitName;
    public bool canAttackWhileMove;
    [Header("模块")]
    public HealthModule healthModule;
    public MilitaryModule militaryModule;
    public NavigationModule navigationModule;
    
    [Header("阵营")]
    public int camp;

    [Header("当前属性")]
    public bool isAlive = true;
    protected IMoveAlgorithm moveAlgorithm;
    protected IAlertAlgorithm alertAlgorithm;

    private OutlineObject outline;

//--------------------------------------

    void Start()
    {   
        navigationModule?.Init(this);
        militaryModule?.Init(this);
        healthModule?.Init(this);
        SetMoveAlgorithm(new NavMeshMoveAlgorithm(GetComponent<NavMeshAgent>()));
        SetAlertAlgorithm(new RangeAlertAlgorithm());
        unitName = config.unitName;
        canAttackWhileMove = config.canAttackWhileMove;
    }

    // ====== 基础方法 ======

    // 这两个方法时临时的
    public void SetMoveAlgorithm(IMoveAlgorithm algorithm)
    {
        moveAlgorithm = algorithm;
        navigationModule.SetMoveAlgorithm(algorithm);
    }
    public void SetAlertAlgorithm(IAlertAlgorithm algorithm)
    {
        alertAlgorithm = algorithm;
        militaryModule.SetAlertAlgorithm(algorithm);
    }

    /// <summary>
    /// 单位被鼠标点击选中
    /// </summary>
    public void OnSelected()
    {
        Debug.Log($"{name} 被选中！");

        if (outline == null)
        {
            outline = gameObject.AddComponent<OutlineObject>();
        }

        outline.enabled = true;
    }

    public void DeSelected()
    {
        Debug.Log($"{name} 取消选中");
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
}
