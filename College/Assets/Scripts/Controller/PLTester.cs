using UnityEngine;

public class PLTester : MonoBehaviour
{
    [Header("이동 설정")]
    public float walkSpeed = 5f;

    [Header("마우스 회전 설정")]
    public float lookSensitivity = 2f;
    public Transform cameraTransform;

    private float _xRotation = 0f;

    // 리지드바디와 이동 방향을 저장할 변수 추가
    private Rigidbody _rb;
    private Vector3 _moveDirection;

    private void Start()
    {
        // 시작할 때 내게 붙어있는 리지드바디를 가져옵니다.
        _rb = GetComponent<Rigidbody>();

        if (cameraTransform == null)
        {
            cameraTransform = GetComponentInChildren<Camera>().transform;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        LookAround();

        // 1. Update에서 이동할 방향 미리 계산 (GetAxisRaw로 미끄러짐 방지)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        _moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized;
    }

    private void FixedUpdate()
    {
        // 2. FixedUpdate에서 리지드바디에 속도 적용
        Vector3 targetVelocity = _moveDirection * walkSpeed;

        // y축 속도(중력)는 리지드바디의 원래 값을 유지하여 바닥으로 잘 떨어지게 합니다.
        targetVelocity.y = _rb.linearVelocity.y;

        _rb.linearVelocity = targetVelocity;
    }

    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
