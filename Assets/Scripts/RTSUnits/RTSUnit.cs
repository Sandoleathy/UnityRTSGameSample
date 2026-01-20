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

//----------- 视觉效果相关（暂时） --------
    private Renderer unitRenderer;
    private Color originalColor;

    private void Awake() {
        unitRenderer = GetComponentInChildren<Renderer>();
        if (unitRenderer != null)
            originalColor = unitRenderer.material.color;
    }

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

    /// <summary>
    /// 死亡时触发
    /// </summary>
    public void OnDead()
    {
        StartCoroutine(HitEffect(2f));    // 临时受击特效
    }
    
    /// <summary>
    /// 受到伤害
    /// </Summary>
    public void TakeDamage()
    {
        StartCoroutine(HitEffect());    // 临时受击特效
    }

    private IEnumerator HitEffect(float duration = 0.2f)
    {
        if (unitRenderer != null)
        {
            unitRenderer.material.color = Color.red; // 变红
            yield return new WaitForSeconds(duration);  // 持续0.2秒
            unitRenderer.material.color = originalColor; // 恢复原色
        }
    }

}
