using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    [Header("移动相关")]
    public float moveSpeed = 10f;          // 最大移动速度
    public float edgeSize = 20f;           // 鼠标到屏幕边缘的判定宽度
    public float acceleration = 5f;        // 移动加速度（平滑过渡）
    private Vector3 currentVelocity;       // 当前平滑速度

    [Header("旋转相关")]
    public float rotateSpeed = 100f;       // 鼠标拖拽旋转速度

    [Header("边界限制 (可选)")]
    public Vector2 minBounds = new Vector2(-50, -50);
    public Vector2 maxBounds = new Vector2(50, 50);

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        Vector3 targetDir = Vector3.zero;

        // 屏幕边缘检测
        Vector3 mousePos = Input.mousePosition;

        if (mousePos.x < edgeSize) targetDir += Vector3.left;
        if (mousePos.x > Screen.width - edgeSize) targetDir += Vector3.right;
        if (mousePos.y < edgeSize) targetDir += Vector3.back;
        if (mousePos.y > Screen.height - edgeSize) targetDir += Vector3.forward;

        // WASD 键盘控制
        float h = Input.GetAxis("Horizontal"); // A,D
        float v = Input.GetAxis("Vertical");   // W,S
        targetDir += new Vector3(h, 0, v);

        // 转换到相机的方向（忽略y轴旋转）
        targetDir = Quaternion.Euler(0, transform.eulerAngles.y, 0) * targetDir;
        targetDir.Normalize();

        // 平滑插值速度
        currentVelocity = Vector3.Lerp(currentVelocity, targetDir * moveSpeed, Time.deltaTime * acceleration);

        // 移动
        transform.position += currentVelocity * Time.deltaTime;

        // 限制边界
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, minBounds.y, maxBounds.y)
        );
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(2)) // 中键拖动
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up, mouseX * rotateSpeed * Time.deltaTime, Space.World);
        }

        // 键盘 Q / E 控制旋转
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
        }
    }

    // ====== 外部可调用接口 ======
    public void SetCameraPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetCameraRotation(Vector3 eulerAngles)
    {
        transform.rotation = Quaternion.Euler(eulerAngles);
    }
}
