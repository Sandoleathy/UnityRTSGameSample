using UnityEngine;

public class StraightMoveAlgorithm : IMoveAlgorithm
{
    public Vector3 GetMoveDelta(Vector3 currentPosition, Vector3 targetPosition, float targetSpeed, float deltaTime, float eplison = 0.1f)
    {
        Vector3 dir = (targetPosition - currentPosition).normalized;
        return dir * targetSpeed * deltaTime;
    }
}