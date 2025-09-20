using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    [Header("�ƶ����")]
    public float moveSpeed = 10f;          // ����ƶ��ٶ�
    public float edgeSize = 20f;           // ��굽��Ļ��Ե���ж����
    public float acceleration = 5f;        // �ƶ����ٶȣ�ƽ�����ɣ�
    private Vector3 currentVelocity;       // ��ǰƽ���ٶ�

    [Header("��ת���")]
    public float rotateSpeed = 100f;       // �����ק��ת�ٶ�

    [Header("�߽����� (��ѡ)")]
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

        // ��Ļ��Ե���
        Vector3 mousePos = Input.mousePosition;

        if (mousePos.x < edgeSize) targetDir += Vector3.left;
        if (mousePos.x > Screen.width - edgeSize) targetDir += Vector3.right;
        if (mousePos.y < edgeSize) targetDir += Vector3.back;
        if (mousePos.y > Screen.height - edgeSize) targetDir += Vector3.forward;

        // WASD ���̿���
        float h = Input.GetAxis("Horizontal"); // A,D
        float v = Input.GetAxis("Vertical");   // W,S
        targetDir += new Vector3(h, 0, v);

        // ת��������ķ��򣨺���y����ת��
        targetDir = Quaternion.Euler(0, transform.eulerAngles.y, 0) * targetDir;
        targetDir.Normalize();

        // ƽ����ֵ�ٶ�
        currentVelocity = Vector3.Lerp(currentVelocity, targetDir * moveSpeed, Time.deltaTime * acceleration);

        // �ƶ�
        transform.position += currentVelocity * Time.deltaTime;

        // ���Ʊ߽�
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, minBounds.y, maxBounds.y)
        );
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(2)) // �м��϶�
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up, mouseX * rotateSpeed * Time.deltaTime, Space.World);
        }

        // ���� Q / E ������ת
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
        }
    }

    // ====== �ⲿ�ɵ��ýӿ� ======
    public void SetCameraPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetCameraRotation(Vector3 eulerAngles)
    {
        transform.rotation = Quaternion.Euler(eulerAngles);
    }
}
