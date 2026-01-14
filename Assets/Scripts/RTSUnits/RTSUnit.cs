using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(OutlineObject))]
public class RTSUnit : MonoBehaviour
{
    [Header("单位配置")]
    public RTSUnitConfig config;
    [Header("武器")]    
    /// <summary>
    /// 武器
    /// </summary>
    public Weapon weapon;
    
    [Header("阵营")]
    public int camp;

    [Header("当前属性")]
    public float currentMoveSpeed = 0f; // 当前移动速度（可用于加速/减速）
    public float HP = 0f;
    public bool isCeaseFire = true;     // 是否停止开火
    public bool isAlive = true;
    protected Vector3 moveTargetPosition;   // 移动目标位置
    public bool isMoving = false;

    protected IMoveAlgorithm moveAlgorithm;
    protected IAlertAlgorithm alertAlgorithm;
    [Header("锁定的敌人单位")]
    public RTSUnit currentTargetEnemy;

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
        SetMoveAlgorithm(new NavMeshMoveAlgorithm(GetComponent<NavMeshAgent>()));
        SetAlertAlgorithm(new RangeAlertAlgorithm());
        if(HP == 0){
            HP = config.maxHP;
        }  
    }

    // ====== 基础方法 ======

    public void SetMoveAlgorithm(IMoveAlgorithm algorithm)
    {
        moveAlgorithm = algorithm;
    }
    public void SetAlertAlgorithm(IAlertAlgorithm algorithm)
    {
        alertAlgorithm = algorithm;
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
    /// 设置移动目标点
    /// </summary>
    public void MoveTo(Vector3 destination, float speed = -1f)
    {
        Debug.Log("Move to");
        isMoving = true;
        moveTargetPosition = destination;
        currentMoveSpeed = (speed > 0) ? Mathf.Min(speed, config.maxMoveSpeed) : config.maxMoveSpeed;
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public void StopMove()
    {
        isMoving = false;
        currentMoveSpeed = 0f;
    }

    /// <summary>
    /// 死亡时触发
    /// </summary>
    public void OnDead()
    {
        isAlive = false;
        gameObject.layer = LayerMask.NameToLayer("DeadLayer");
        //TODO: 死亡效果
        Debug.Log($"{name} HP耗尽, 死亡了!");
        Destroy(gameObject, 2f);
    }
    /// <summary>
    /// 攻击敌人
    /// </summary>
    /// <param name="enemy">选定的敌人单位</param>
    protected void AttemptToAttack(RTSUnit enemy)
    {
        if(weapon != null){
            if(weapon.Attack(enemy)) Debug.Log($"{config.name} 使用 {weapon.name} 对 {enemy.config.name} 发起攻击！");
        }
        else{
            Debug.Log($"{config.name} 没有武器，无法攻击！");
        }
        // TODO: 攻击实现
    }
    /// <summary>
    /// 受到伤害
    /// </Summary>
    public void TakeDamage(float damage)
    {
        HP -= damage;
        StartCoroutine(HitEffect());    // 临时受击特效
        Debug.Log($"{name} 受到 {damage} 点伤害, 当前HP: {HP}");
        if (HP <= 0)
        {
            OnDead();
        }
    }

    private IEnumerator HitEffect()
    {
        if (unitRenderer != null)
        {
            unitRenderer.material.color = Color.red; // 变红
            yield return new WaitForSeconds(0.2f);  // 持续0.2秒
            unitRenderer.material.color = originalColor; // 恢复原色
        }
    }

    protected void Update()
    {
        if(!isAlive) return; //死了就不要更新了
        RTSUnit enemy = null;
        // 检测敌人
        if(!isCeaseFire){
            enemy = alertAlgorithm?.DetectEnemy(this);
        }
        //如果发现敌人并且单位是静止的，就可以攻击
        //如果允许移动攻击（canAttackWhileMove == true），那就算在移动也可以攻击。
        if (enemy != null && (!isMoving || config.canAttackWhileMove))
        {
            currentTargetEnemy = enemy;
            AttemptToAttack(currentTargetEnemy);
            return;
        }
        if (moveAlgorithm != null)
        {
            Vector3 delta = moveAlgorithm.GetMoveDelta(
                transform.position,
                moveTargetPosition,
                currentMoveSpeed,
                Time.deltaTime
            );

            if (delta != Vector3.zero)
            {
                isMoving = true;
                // 移动
                transform.position += delta;

                // 旋转朝向移动方向
                Quaternion targetRotation = Quaternion.LookRotation(delta, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
            else
            {
                isMoving = false;
            }
            // if (Vector3.Distance(transform.position, moveTargetPosition) < 0.5f)
            // {
            //     Debug.Log($"{name} 停止移动");
            //     StopMove();
            // }
        }
        if (HP <= 0)
        {
            OnDead();
        }
    }

}
