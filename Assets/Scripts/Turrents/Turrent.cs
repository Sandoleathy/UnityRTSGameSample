using System.Collections.Generic;
using UnityEngine;

public class Turrent : MonoBehaviour
{
    public TurrentConfig config;
    public Weapon weapon;

    private void Start() {
        
    }

    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            config.maxRotateSpeed * Time.deltaTime
        );
}

    private bool IsAlignWithTarget(Vector3 targetPosition, float angleThreshold = 5f)
    {
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToTarget);
        return angle <= angleThreshold;
    }

    public bool AimAndAttack(RTSUnit enemy)
    {
        if(IsAlignWithTarget(enemy.transform.position))
        {
            if (weapon != null)
            {
                return weapon.Attack(enemy);
            }
            else return false;
        }
        else
        {
            RotateTowards(enemy.transform.position);
            return false;
        }
    }
}