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
    public List<Weapon> weapons;
    public List<Turrent> turrents;
    private bool _isLoggedNoWeapon = false;
    
    [Header("阵营")]
    public int camp;

    [Header("当前属性")]
    public float targetSpeed = 0f; // 当前移动速度（可用于加速/减速）
    public float HP = 0f;
    public bool isCeaseFire = true;     // 是否停止开火
    public bool isAlive = true;
    // ====== 移动与警戒相关 ======
    [Header("移动相关")]
    protected Vector3 moveTargetPosition;   // 移动目标位置
    public bool isMoving = false;
    public float stopEpsilon = 0.5f; // 停止移动的误差范围

    protected IMoveAlgorithm moveAlgorithm;
    protected IAlertAlgorithm alertAlgorithm;
    [Header("锁定的敌人单位")]
    public RTSUnit currentTargetEnemy;
    protected Quaternion targetRotation;

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
    /// 可以指定每次移动的移动速度
    /// </summary>
    public void MoveTo(Vector3 destination, float speed = -1f)
    {
        isMoving = true;
        moveTargetPosition = destination;
        targetSpeed = (speed > 0) ? Mathf.Min(speed, config.maxMoveSpeed) : config.maxMoveSpeed;
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public void StopMove()
    {
        Debug.Log($"{name} 停止移动");
        isMoving = false;
        targetSpeed = 0f;
        moveTargetPosition = transform.position;
    }

    /// <summary>
    /// 死亡时触发
    /// </summary>
    public void OnDead()
    {
        isAlive = false;
        gameObject.layer = LayerMask.NameToLayer("DeadLayer");
        //TODO: 死亡效果
        StartCoroutine(HitEffect(2f));    // 临时受击特效
        Debug.Log($"{name} HP耗尽, 死亡了!");
        Destroy(gameObject, 2f);
    }
    /// <summary>
    /// 攻击敌人
    /// </summary>
    /// <param name="enemy">选定的敌人单位</param>
    protected void AttemptToAttack(RTSUnit enemy)
    {
        if (turrents.Count > 0)
        {
            // 使用炮塔攻击
            foreach (Turrent turrent in turrents)
            {
                turrent.AimAndAttack(enemy);
            }
        }
        else
        {
            // 旋转单位朝向敌人
            targetRotation = Quaternion.LookRotation(enemy.transform.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * config.maxRotateSpeed);
        }
        if(weapons.Count > 0){
            foreach(Weapon weapon in weapons){
                if(weapon.Attack(enemy)) Debug.Log($"{config.unitName} 使用 {weapon.name} 对 {enemy.config.unitName} 发起攻击！");   
            }
        }
        else{
            if(turrents.Count == 0)if(!_isLoggedNoWeapon) {Debug.Log($"{config.unitName} 没有武器");_isLoggedNoWeapon = true;}
        }
        // TODO: 攻击实现
    }
    /// <summary>
    /// 受到伤害
    /// </Summary>
    public void TakeDamage(float damage)
    {
        HP -= damage;
        Debug.Log($"{name} 受到 {damage} 点伤害, 当前HP: {HP}");
        if (HP <= 0)
        {
            OnDead();
        }
        else
        {
            StartCoroutine(HitEffect());    // 临时受击特效
        }
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
            if(enemy.isAlive == false){
                currentTargetEnemy = null;
                return;
            }
            currentTargetEnemy = enemy;
            AttemptToAttack(currentTargetEnemy);
        }
        if (moveAlgorithm != null && moveTargetPosition != null)
        {
            Vector3 delta = moveAlgorithm.GetMoveDelta(
                transform.position,
                moveTargetPosition,
                targetSpeed,
                Time.deltaTime
            );

            if (delta != Vector3.zero)
            {
                isMoving = true;
                // 移动
                transform.position += delta;

                // 旋转朝向移动方向
                targetRotation = Quaternion.LookRotation(delta, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * config.maxRotateSpeed);
            }
            else
            {
                isMoving = false;
            }
            if ((moveTargetPosition - transform.position).sqrMagnitude < stopEpsilon * stopEpsilon && isMoving)
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
