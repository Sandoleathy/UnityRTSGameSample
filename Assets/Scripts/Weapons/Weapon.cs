using UnityEngine;

public class Weapon : MonoBehaviour {
    [Header("武器配置")]
    public WeaponConfig weaponConfig;

    [Header("当前状态")]
    public bool isCoolingDown = false;
    public float currentCooldownTime = 0f;

    //返回一个布尔值，表示是否成功发起攻击
    public bool Attack(RTSUnit target){
        //计算冷却
        if (isCoolingDown)
        {
            currentCooldownTime = currentCooldownTime - Time.deltaTime;
            if(currentCooldownTime >= 0)
            {
                return false; //仍在冷却中，无法攻击
            }
            else
            {
                isCoolingDown = false; //冷却结束
            }
        }
        switch(weaponConfig.attackType){
            case AttackType.SingleDirect:
                if(!SingleDirectAttack(target)) return false;
                break;
            case AttackType.SingleTrack:
                break;
            case AttackType.AreaTrack:
                break;
            case AttackType.AreaDirect:
                break;
            default:
                Debug.LogError("未知的攻击类型");
                break;
        }
        //重置攻击间隔冷却
        if(weaponConfig.attackSpeed != 0)
        {
            currentCooldownTime = 1/weaponConfig.attackSpeed;
            isCoolingDown = true;
        }
        Debug.Log($"{name} 攻击 {target.name}");
        return true;
    }
    private bool SingleDirectAttack(RTSUnit target){
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if(distance < weaponConfig.attackRange){
            
            HealthModule healthModule = target.moduleContainer.Get<HealthModule>();
            // 直接命中目标，造成伤害
            healthModule?.OnTakeDamage(weaponConfig.damage);

            Debug.Log($"{name} 攻击 {target.name} 造成了 {weaponConfig.damage} 伤害");
            return true;
        }else{
            Debug.Log($"{name} 攻击 {target.name} 距离太远，无法攻击");
            return false;
        }
    }
}