using UnityEngine;

public class PLTester : MonoBehaviour
{
    [Header("이동 설정")]
    public float walkSpeed = 3f;

    [Header("마우스 회전 설정")]
    public float lookSensitivity = 2f;
    public Transform cameraTransform;

    [Header("헤드 보빙(Head Bobbing) 설정")]
    [Tooltip("카메라가 흔들리는 속도")]
    public float bobbingSpeed = 8f;
    [Tooltip("카메라가 위아래로 흔들리는 폭(높낮이)")]
    public float bobbingAmount = 0.05f;

    private float _defaultPosY = 0f;
    private float _timer = 0f;

    private float _xRotation = 0f;
    private Rigidbody _rb;
    private Vector3 _moveDirection;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        if (cameraTransform == null)
        {
            cameraTransform = GetComponentInChildren<Camera>().transform;
        }

        // 1. 카메라의 초기 Y 위치(원래 눈높이)를 기억해 둡니다.
        if (cameraTransform != null)
        {
            _defaultPosY = cameraTransform.localPosition.y;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        LookAround();

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        _moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized;

        // 2. 매 프레임마다 입력값을 넘겨주어 헤드 보빙을 계산합니다.
        HandleHeadBobbing(moveX, moveZ);
    }

    private void FixedUpdate()
    {
        Vector3 targetVelocity = _moveDirection * walkSpeed;
        targetVelocity.y = _rb.linearVelocity.y; // 중력 유지 방식 유지
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

    private void HandleHeadBobbing(float moveX, float moveZ)
    {
        // 입력값이 있는지(키보드를 눌러 이동 중인지) 확인합니다.
        if (Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f)
        {
            // 이동 중이면 타이머를 증가시키고 Sin 함수로 Y 위치를 위아래로 부드럽게 흔듭니다.
            _timer += Time.deltaTime * bobbingSpeed;
            cameraTransform.localPosition = new Vector3(
                cameraTransform.localPosition.x,
                _defaultPosY + Mathf.Sin(_timer) * bobbingAmount,
                cameraTransform.localPosition.z
            );
        }
        else
        {
            // 멈춰있으면 타이머를 초기화하고, Lerp를 이용해 카메라를 원래 높이로 스르륵 복귀시킵니다.
            _timer = 0f;
            cameraTransform.localPosition = new Vector3(
                cameraTransform.localPosition.x,
                Mathf.Lerp(cameraTransform.localPosition.y, _defaultPosY, Time.deltaTime * bobbingSpeed),
                cameraTransform.localPosition.z
            );
        }
    }
}
