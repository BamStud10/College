using System.Collections;
using UnityEngine;

public class LoopDoor : MonoBehaviour
{
    [Header("매니저 연결")]
    public GameManager gameManager;

    // 연타 방지용 변수 (F키를 여러 번 눌러도 한 번만 작동하게 함)
    private bool _isInteracting = false;

    public void Interact()
    {
        if (_isInteracting) return;

        // 코루틴을 사용해 문 열림 연출 후 텔레포트 되도록 흐름 제어
        StartCoroutine(DoorOpenRoutine());
    }

    private IEnumerator DoorOpenRoutine()
    {
        _isInteracting = true;

        // 1. 문 열림 연출 (추후 애니메이터나 사운드 추가)
        Debug.Log("?? 끼이익... 문이 스르륵 열립니다.");

        // 2. 문이 열리는 시간(예: 1초) 동안 대기
        yield return new WaitForSeconds(1.0f);

        // 3. 태그를 확인하여 정답/오답 판별
        // (gameObject는 이 스크립트가 붙어있는 문 자신을 의미합니다)
        bool isForward = gameObject.CompareTag("Finish");

        // 4. 게임 매니저에 결과 전달 및 텔레포트 실행
        if (gameManager != null)
        {
            gameManager.ProcessLoop(isForward);
        }

        // 루프되어 돌아오면 다시 상호작용할 수 있도록 초기화
        _isInteracting = false;
    }
}
