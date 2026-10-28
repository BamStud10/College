using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rb; // 또는 CharacterController

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 이동 입력값이나 속도를 계산해서 Animator 파라미터에 전달
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        bool isMoving = (Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f);

        // 예: Animator에 만든 'IsMoving'이라는 불리언(Bool) 파라미터 조작
        _animator.SetBool("IsMoving", isMoving);

        // 또는 속도(Float)를 넘겨줄 경우:
        // float speed = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z).magnitude;
        // _animator.SetFloat("Speed", speed);
    }
}