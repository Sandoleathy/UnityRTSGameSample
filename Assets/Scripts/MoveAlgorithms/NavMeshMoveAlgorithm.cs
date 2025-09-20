using UnityEngine;
using UnityEngine.AI;

public class NavMeshMoveAlgorithm : IMoveAlgorithm
{
    private NavMeshAgent agent;

    public NavMeshMoveAlgorithm(NavMeshAgent navMeshAgent)
    {
        agent = navMeshAgent;
        agent.updatePosition = false; // 由我们手动控制位置
        agent.updateRotation = false;
    }

    public Vector3 GetMoveDelta(Vector3 currentPosition, Vector3 targetPosition, float currentSpeed, float deltaTime)
    {
        // 如果目标点改变，就更新导航路径
        if (agent.destination != targetPosition)
        {
            agent.SetDestination(targetPosition);
        }

        // 让 NavMeshAgent 按速率更新
        agent.speed = currentSpeed;
        agent.nextPosition = currentPosition;

        // 计算期望速度 (NavMesh 内部计算出的方向)
        Vector3 desiredVelocity = agent.desiredVelocity;

        // 根据速度和时间计算位移
        return desiredVelocity.normalized * currentSpeed * deltaTime;
    }
}
