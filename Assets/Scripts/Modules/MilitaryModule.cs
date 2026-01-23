using System.Collections.Generic;
using UnityEngine;

public class MilitaryModule : IModule, IUpdatableModule
{
    public List<Weapon> weapons;
    public List<Turrent> turrents;

    private bool isOpenFire;

    private IAlertAlgorithm alertAlgorithm;

    private RTSUnit owner;
    private RTSUnit currentTargetEnemy;
    private bool _isLoggedNoWeapon = false;
    private NavigationModule navigationModule;

    public void Init(RTSUnit owner){
        this.owner = owner;
        navigationModule = owner.moduleContainer.Get<NavigationModule>();
        alertAlgorithm = owner.alertAlgorithm;

        // 咋还自动获取武器了
        weapons = new List<Weapon>(owner.GetComponentsInChildren<Weapon>());
        turrents = new List<Turrent>(owner.GetComponentsInChildren<Turrent>());
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
            if(navigationModule != null) navigationModule.SetTargetRotation(Quaternion.LookRotation(enemy.transform.position - owner.transform.position, Vector3.up));
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

    public void Tick(float dt)
    {
        RTSUnit enemy = null;
        // 检测敌人
        if(!isOpenFire){
            enemy = alertAlgorithm?.DetectEnemy(owner);
        }
        //如果发现敌人并且单位是静止的，就可以攻击
        //如果允许移动攻击（canAttackWhileMove == true），那就算在移动也可以攻击。
        if (enemy != null && (!navigationModule.isMoving || owner.canAttackWhileMove))
        {
            if(!enemy.isAlive){
                currentTargetEnemy = null;
                return;
            }
            currentTargetEnemy = enemy;
            AttemptToAttack(currentTargetEnemy);
        }
    }
    public string GetName()
    {
        return "MilitaryModule";
    }

    public void SetIsOpenFire(bool isOpenFire)
    {
        this.isOpenFire = isOpenFire;
    }

}