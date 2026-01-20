using UnityEngine;

public class NavigationModule: MonoBehaviour
{
    private RTSUnit owner;
    public bool isMoving = false;
    private Vector3 moveTargetPosition;
    public float maxMoveSpeed;
    public float targetSpeed;
    private IMoveAlgorithm moveAlgorithm;
    protected Quaternion targetRotation;
    public float stopEpsilon = 0.5f;
    public float maxRotateSpeed;


    public void Init(RTSUnit owner)
    {
        this.owner = owner;
        maxMoveSpeed = owner.config.maxMoveSpeed;
        maxRotateSpeed = owner.config.maxRotateSpeed;
    }
    public void MoveTo(Vector3 destination, float speed = -1f)
    {
        isMoving = true;
        moveTargetPosition = destination;
        targetSpeed = (speed > 0) ? Mathf.Min(speed, maxMoveSpeed) : maxMoveSpeed;
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

    public void Update()
    {
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
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * maxRotateSpeed);
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
    }

    public void SetTargetRotation(Quaternion rotation)
    {
        targetRotation = rotation;
    }
    public Quaternion GetTargetRotation()
    {
        return targetRotation;
    }
    public void SetMoveAlgorithm(IMoveAlgorithm algorithm)
    {
        moveAlgorithm = algorithm;
    }
}