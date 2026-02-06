using UnityEngine;

public class NavigationModule: MonoBehaviour, IModule, IUpdatableModule
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
    [SerializeField]
    private bool isEnable = true;


    public void Init(RTSUnit owner)
    {
        this.owner = owner;
        moveAlgorithm = owner.moveAlgorithm;
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
        Debug.Log($"{owner.unitName} 停止移动");
        isMoving = false;
        targetSpeed = 0f;
        moveTargetPosition = owner.transform.position;
    }

    public void Tick(float dt)
    {
        if (moveAlgorithm != null && moveTargetPosition != null)
        {
            Vector3 delta = moveAlgorithm.GetMoveDelta(
                owner.transform.position,
                moveTargetPosition,
                targetSpeed,
                dt
            );

            if (delta != Vector3.zero)
            {
                isMoving = true;
                // 移动
                owner.transform.position += delta;

                // 旋转朝向移动方向
                targetRotation = Quaternion.LookRotation(delta, Vector3.up);
            }
            else
            {
                isMoving = false;
            }
            owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, dt * maxRotateSpeed);
            if ((moveTargetPosition - owner.transform.position).sqrMagnitude < stopEpsilon * stopEpsilon && isMoving)
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

    public string GetName()
    {
        return "NavigationModule";
    }
    public void Disable()
    {
        isEnable = false;
    }
    public void Enable()
    {
        isEnable = true;
    }
    public bool IsEnable(){return isEnable;}
}