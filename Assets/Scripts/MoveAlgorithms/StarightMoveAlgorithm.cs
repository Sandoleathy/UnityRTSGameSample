using UnityEngine;

public class StraightMoveAlgorithm : IMoveAlgorithm
{
    public Vector3 GetMoveDelta(Vector3 currentPosition, Vector3 targetPosition, float currentSpeed, float deltaTime)
    {
        Vector3 dir = (targetPosition - currentPosition).normalized;
        return dir * currentSpeed * deltaTime;
    }
}