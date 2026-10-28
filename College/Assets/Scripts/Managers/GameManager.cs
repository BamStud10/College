using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int CurrentStage { get; private set; } = 1;
    public int MaxStage = 8; // 8스테이지 도달 시 클리어

    [Header("매니저 연결")]
    public GimmickManager gimmickManager;

    [Header("UI 연결")]
    [Tooltip("페이드 연출을 담당하는 스크립트")]
    public Fader _fader;

    [Tooltip("ESC를 눌렀을 때 띄울 일시정지(설정) 팝업창")]
    public GameObject pausePopup;

    // 현재 일시정지 상태인지 추적하는 변수
    private bool _isPaused = false;

    [Header("플레이어 및 스폰 설정")]
    public Transform playerTransform; // 플레이어 오브젝트
    public Transform spawnPoint;      // 복도 시작점 (루프 텔레포트 위치)

    // 현재 복도에 기믹이 켜져 있는지 기억하는 변수
    private bool _isAnomalyPresent = false;
    private Rigidbody _playerRb;

    private void Start()
    {
        _playerRb = playerTransform.GetComponent<Rigidbody>();

        if (_fader != null)
        {
            _fader.FadeIn();
        }

        // 게임 시작 시 팝업창은 무조건 꺼둔 상태로 시작
        if (pausePopup != null)
        {
            pausePopup.SetActive(false);
        }

        // 게임 시작 시 첫 번째 루프 굴리기
        StartNewLoop();
    }

    private void Update()
    {
        // ESC 키를 누르면 일시정지 토글
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;

        // 1. 팝업창 켜기/끄기
        if (pausePopup != null)
        {
            pausePopup.SetActive(_isPaused);
        }

        // 2. 게임 시간 정지/재개 (0이면 정지, 1이면 정상 속도)
        Time.timeScale = _isPaused ? 0f : 1f;

        // 3. 마우스 커서 제어 (UI 슬라이더를 조작해야 하므로 커서를 켜줍니다)
        if (_isPaused)
        {
            Cursor.lockState = CursorLockMode.None; // 커서 잠금 해제
            Cursor.visible = true;                  // 커서 보이기
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // 커서 화면 중앙에 잠금
            Cursor.visible = false;                   // 커서 숨기기
        }
    }

    /// <summary>
    /// 출구/입구 트리거에 닿았을 때 호출되는 핵심 함수
    /// isForward가 true면 직진(출구), false면 후퇴(입구)
    /// </summary>
    public void ProcessLoop(bool isForward)
    {
        // 페이드 아웃 -> 텔레포트 -> 페이드 인 흐름을 코루틴으로 실행
        StartCoroutine(LoopTransitionRoutine(isForward));
    }

    // 텔레포트와 페이드 연출의 타이밍을 맞추기 위한 코루틴 함수
    private IEnumerator LoopTransitionRoutine(bool isForward)
    {
        // 1. 정답 판정 (8번출구 핵심 로직)
        if (_isAnomalyPresent)
        {
            if (!isForward) // 이상현상을 보고 도망감 (정답)
            {
                CurrentStage++;
                Debug.Log($"🟢 정답! 이상현상을 피했습니다. (현재 스테이지: {CurrentStage})");
            }
            else // 이상현상인데 직진함 (오답)
            {
                CurrentStage = 1;
                Debug.Log("🔴 오답! 이상현상을 놓쳤습니다. 스테이지 초기화 (1)");
            }
        }
        else
        {
            if (isForward) // 정상이라서 직진함 (정답)
            {
                CurrentStage++;
                Debug.Log($"🟢 정답! 무사히 통과했습니다. (현재 스테이지: {CurrentStage})");
            }
            else // 정상인데 쫄아서 도망감 (오답)
            {
                CurrentStage = 1;
                Debug.Log("🔴 오답! 정상 상태인데 돌아갔습니다. 스테이지 초기화 (1)");
            }
        }

        // 클리어 체크
        if (CurrentStage >= MaxStage)
        {
            Debug.Log("🎉 게임 클리어! 대학교 탈출 성공!");

            if (_fader != null)
            {
                _fader.FadeOut();
                // 화면이 완전히 까매질 때까지 기다림
                yield return new WaitForSeconds(_fader.fadeDuration);
            }

            // 화면이 까매진 상태에서 엔딩 씬으로 이동
            SceneManager.LoadScene("Ending");

            yield break;
        }

        // 2. 다음 루프로 넘어가기 전, 화면 까맣게 페이드 아웃
        if (_fader != null)
        {
            _fader.FadeOut();
            // 페이드 효과가 끝날 때까지 대기 (화면이 완전히 꺼질 때까지 기다림)
            yield return new WaitForSeconds(_fader.fadeDuration);
        }

        // 3. 화면이 완전히 까만 상태에서 몰래 플레이어를 텔레포트 시키고 기믹 세팅
        StartNewLoop();

        // 4. 텔레포트가 완료되었으니 다시 화면 밝아짐 (페이드 인)
        if (_fader != null)
        {
            _fader.FadeIn();
        }
    }

    private void StartNewLoop()
    {
        // A. 이전 복도의 기믹들 원상복귀 (이벤트 발송)
        gimmickManager.ResetAllGimmicks();

        // B. 플레이어 텔레포트 (리지드바디 속도를 죽여야 텔레포트 시 미끄러지지 않음)
        if (_playerRb != null) _playerRb.linearVelocity = Vector3.zero;
        playerTransform.position = spawnPoint.position;

        playerTransform.rotation = spawnPoint.rotation;

        // C. 새로운 복도의 기믹 굴리기
        int generatedGimmickID = gimmickManager.GenerateRandomGimmick(CurrentStage);

        // D. 이번 복도에 기믹이 존재하는지 저장 (다음 판정을 위해)
        _isAnomalyPresent = (generatedGimmickID != GimmickManager.NORMAL_ID);
    }

    public void Exit()
    {
        // 1. 빌드된 실제 게임(.exe)에서 프로그램 종료
        Application.Quit();

        // 2. 유니티 에디터에서 테스트 중일 때 플레이 모드 종료 (프로토타입 꿀팁)
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
