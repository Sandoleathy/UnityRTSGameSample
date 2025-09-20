using UnityEngine;

public interface IMoveAlgorithm
{
    /// <summary>
    /// 计算移动增量
    /// </summary>
    /// <param name="currentPosition">单位当前位置</param>
    /// <param name="targetPosition">目标位置</param>
    /// <param name="currentSpeed">当前速度</param>
    /// <param name="deltaTime">帧间隔</param>
    /// <returns>应该施加的位移向量</returns>
    Vector3 GetMoveDelta(Vector3 currentPosition, Vector3 targetPosition, float currentSpeed, float deltaTime);
}