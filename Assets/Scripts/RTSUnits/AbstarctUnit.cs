using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RTSUnit : MonoBehaviour
{
    [Header("单位基础属性")]
    public string unitName;
    public float HP;
    [Header("能否在移动中攻击")]
    public bool canAttackWhenMove;
    [Header("阵营")]
    public int camp;
    public float alertRange = 10f;

    [Header("移动相关参数")]
    public float maxMoveSpeed = 5f;     // 最大移动速度
    public float currentMoveSpeed = 0f; // 当前移动速度（可用于加速/减速）

    protected Vector3 moveTargetPosition;   // 移动目标位置
    protected bool isMoving = false;

    protected IMoveAlgorithm moveAlgorithm;
    protected IAlertAlgorithm alertAlgorithm;
    [Header("锁定的敌人单位")]
    public RTSUnit currentTargetEnemy;

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
    public virtual void OnSelected()
    {
        Debug.Log($"{name} 被选中！");
    }

    public virtual void DeSelected()
    {
        Debug.Log($"{name} 取消选中");
    }

    /// <summary>
    /// 设置移动目标点
    /// </summary>
    public virtual void MoveTo(Vector3 destination, float speed = -1f)
    {
        moveTargetPosition = destination;
        isMoving = true;
        currentMoveSpeed = (speed > 0) ? Mathf.Min(speed, maxMoveSpeed) : maxMoveSpeed;
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public virtual void StopMove()
    {
        isMoving = false;
        currentMoveSpeed = 0f;
    }

    /// <summary>
    /// 死亡时触发
    /// </summary>
    public virtual void onDead()
    {
        Debug.Log($"{name} HP耗尽, 死亡了!");
        Destroy(this, 2f);
    }
    /// <summary>
    /// 攻击敌人
    /// </summary>
    /// <param name="enemy">选定的敌人单位</param>
    protected virtual void Attack(RTSUnit enemy)
    {
        Debug.Log($"{name} 攻击 {enemy.name}");
        // TODO: 攻击实现
    }

    protected virtual void Update()
    {
        // 检测敌人
        RTSUnit enemy = alertAlgorithm?.DetectEnemy(this);
        //如果发现敌人并且单位是静止的，就可以攻击
        //如果允许移动攻击（canAttackWhileMove == true），那就算在移动也可以攻击。
        if (enemy != null && !isMoving || canAttackWhenMove)
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
            onDead();
        }
    }

}
