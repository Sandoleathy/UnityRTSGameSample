using UnityEngine;

public class Weapon : MonoBehaviour {
    [Header("武器配置")]
    public WeaponConfig weaponConfig;

    public void Attack(RTSUnit target){
        switch(weaponConfig.attackType){
            case AttackType.SingleDirect:
                SingleDirectAttack(target);
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
        Debug.Log($"{name} 攻击 {target.name}");
    }
    private void SingleDirectAttack(RTSUnit target){
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if(distance < weaponConfig.attackRange){
            target.takeDamage(weaponConfig.damage);
            Debug.Log($"{name} 攻击 {target.name} 造成了 {weaponConfig.damage} 伤害");
        }else{
            Debug.Log($"{name} 攻击 {target.name} 距离太远，无法攻击");
            return;
        }
    }
}