using UnityEngine;

public class PL_AnimController : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        // 플레이어 오브젝트에 붙어있는 Animator 컴포넌트를 가져옵니다.
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 일시정지 상태일 때는 애니메이션 전환 로직도 멈춥니다.
        if (Time.timeScale == 0f) return;

        // PLTester와 동일하게 WASD 입력값을 감지합니다.
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // 입력값이 조금이라도 존재하면 이동 중(true), 아니면 대기(false)로 판정합니다.
        bool isMoving = (Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f);

        // 애니메이터에 설정할 'isMoving' 파라미터에 결과값을 넘겨줍니다.
        if (_animator != null)
        {
            _animator.SetBool("isMoving", isMoving);
        }
    }
}
