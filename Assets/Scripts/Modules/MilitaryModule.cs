using System.Collections.Generic;
using UnityEngine;

public class MilitaryModule : MonoBehaviour{
    public List<Weapon> weapons;
    public List<Turrent> turrents;

    public bool isCeaseFire;

    protected IAlertAlgorithm alertAlgorithm;

    public RTSUnit owner;
    private RTSUnit currentTargetEnemy;
    private bool _isLoggedNoWeapon = false;
    private NavigationModule _navigationModule;

    public void Init(RTSUnit owner){
        this.owner = owner;
        _navigationModule = GetComponent<NavigationModule>();
    }
    public void SetAlertAlgorithm(IAlertAlgorithm algorithm)
    {
        alertAlgorithm = algorithm;
    }

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
            // 为NavigationModule设置旋转方向
            _navigationModule?.SetTargetRotation(Quaternion.LookRotation(enemy.transform.position - transform.position, Vector3.up));
        }
        if(weapons.Count > 0){
            foreach(Weapon weapon in weapons){
                if(weapon.Attack(enemy)) Debug.Log($"{owner.unitName} 使用 {weapon.name} 对 {enemy.unitName} 发起攻击！");   
            }
        }
        else{
            if(turrents.Count == 0)if(!_isLoggedNoWeapon) {Debug.Log($"{owner.unitName} 没有武器");_isLoggedNoWeapon = true;}
        }
        // TODO: 攻击实现
    }

    void Update()
    {
        RTSUnit enemy = null;
        // 检测敌人
        if(!isCeaseFire){
            enemy = alertAlgorithm?.DetectEnemy(owner);
        }
        //如果发现敌人并且单位是静止的，就可以攻击
        //如果允许移动攻击（canAttackWhileMove == true），那就算在移动也可以攻击。
        if (enemy != null && (!_navigationModule.isMoving || owner.canAttackWhileMove))
        {
            if(!enemy.isAlive){
                currentTargetEnemy = null;
                return;
            }
            currentTargetEnemy = enemy;
            AttemptToAttack(currentTargetEnemy);
        }
    }
}