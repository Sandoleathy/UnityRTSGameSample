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
    protected Vector3 moveTargetPosition;   // 移动目标位置
    protected bool isMoving = false;

    protected IMoveAlgorithm moveAlgorithm;
    protected IAlertAlgorithm alertAlgorithm;
    [Header("锁定的敌人单位")]
    public RTSUnit currentTargetEnemy;

    private OutlineObject outline;

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
        moveTargetPosition = destination;
        isMoving = true;
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
        Debug.Log($"{name} HP耗尽, 死亡了!");
        Destroy(this, 2f);
    }
    /// <summary>
    /// 攻击敌人
    /// </summary>
    /// <param name="enemy">选定的敌人单位</param>
    protected void Attack(RTSUnit enemy)
    {
        Debug.Log($"{name} 攻击 {enemy.name}");
        // TODO: 攻击实现
    }
    /// <summary>
    /// 受到伤害
    /// </Summary>
    public void takeDamage(float damage)
    {
        HP -= damage;
        Debug.Log($"{name} 受到 {damage} 点伤害, 当前HP: {HP}");
        if (HP <= 0)
        {
            OnDead();
        }
    }

    protected void Update()
    {
        // 检测敌人
        RTSUnit enemy = alertAlgorithm?.DetectEnemy(this);
        //如果发现敌人并且单位是静止的，就可以攻击
        //如果允许移动攻击（canAttackWhileMove == true），那就算在移动也可以攻击。
        if (enemy != null && (!isMoving || config.canAttackWhileMove))
        {
            currentTargetEnemy = enemy;
            Attack(currentTargetEnemy);
            return;
        }
        if (isMoving && moveAlgorithm != null)
        {
            Vector3 delta = moveAlgorithm.GetMoveDelta(
                transform.position,
                moveTargetPosition,
                currentMoveSpeed,
                Time.deltaTime
            );

            if (delta != Vector3.zero)
            {
                // 移动
                transform.position += delta;

                // 旋转朝向移动方向
                Quaternion targetRotation = Quaternion.LookRotation(delta, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }

            if (Vector3.Distance(transform.position, moveTargetPosition) < 0.1f)
            {
                StopMove();
            }
        }
        if (HP <= 0)
        {
            OnDead();
        }
    }

}
