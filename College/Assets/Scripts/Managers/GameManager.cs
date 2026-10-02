using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int CurrentStage { get; private set; } = 1;
    public int MaxStage = 8; // 8스테이지 도달 시 클리어

    [Header("매니저 연결")]
    public GimmickManager gimmickManager;

    [Header("플레이어 및 스폰 설정")]
    public Transform playerTransform; // 플레이어 오브젝트
    public Transform spawnPoint;      // 복도 시작점 (루프 텔레포트 위치)

    // 현재 복도에 기믹이 켜져 있는지 기억하는 변수
    private bool _isAnomalyPresent = false;
    private Rigidbody _playerRb;

    private void Start()
    {
        _playerRb = playerTransform.GetComponent<Rigidbody>();

        // 게임 시작 시 첫 번째 루프 굴리기
        StartNewLoop();
    }

    /// <summary>
    /// 출구/입구 트리거에 닿았을 때 호출되는 핵심 함수
    /// isForward가 true면 직진(출구), false면 후퇴(입구)
    /// </summary>
    public void ProcessLoop(bool isForward)
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
            TestClear();
            return;
        }

        // 2. 판정이 끝났으니 다음 루프 준비
        StartNewLoop();
    }

    public GameObject _testPopUp;
    private void TestClear()
    {
        RectTransform rect = _testPopUp.GetComponent<RectTransform>();
        Vector2 newPos = rect.anchoredPosition;
        newPos.y = -96;
        rect.anchoredPosition = newPos;
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
}
