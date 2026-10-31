using System.Collections;
using UnityEngine;

/// <summary>
/// ProfSecond 기믹 (ID: 107 / GimmickLV2.profSecond)
/// - TestReset(또는 GimmickManager)에서 E키로 해당 기믹 ID 발동 시 준비 상태 진입
/// - 이후 첫 번째 교수님의 가로 감지 콜라이더를 지나치면 3초 뒤 두 번째 Spline 교수님 등장
/// </summary>
public class Gimmick_Prof_ProfSecond : BasicGimmick
{
    [Header("기믹 설정")]
    [Tooltip("Define.cs에 정의된 기믹 Enum (profSecond = 107)")]
    public Define.GimmickLV2 myGimmick = Define.GimmickLV2.profSecond;

    [Header("두 번째 교수님(ProfMover) 설정")]
    [Tooltip("반대편에서 ProfSpline으로 걸어올 두 번째 교수님의 ProfMover 스크립트")]
    [SerializeField] private ProfMover _secondProfMover;

    [Header("지연 시간 설정")]
    [Tooltip("첫 번째 교수님과 교차 후 두 번째 교수님이 나타날 때까지의 시간(초)")]
    [SerializeField] private float _spawnDelay = 3.0f;

    private bool _isGimmickActive = false; // GimmickManager/TestReset에 의해 기믹이 발동되었는지 여부
    private bool _isTriggered = false;     // 첫 번째 교수님을 이미 지나쳤는지 여부
    private Coroutine _spawnCoroutine;

    private void Awake()
    {
        // Enum 값을 int형 ID로 설정 (BasicGimmick의 _code 할당)
        _code = (int)myGimmick;
    }

    // 부모 클래스(BasicGimmick)의 OnEnable / OnDisable을 반드시 호출해 이벤트를 구독해야 함
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    private void Start()
    {
        ResetGimmick();
    }

    /// <summary>
    /// 첫 번째 교수님의 가로 감지 콜라이더(Gimmick_ProfSecond_PassTrigger)에서 호출
    /// </summary>
    public void OnPlayerPassedFirstProf()
    {
        // E키로 기믹이 발동되지 않았거나, 이미 감지되어 스폰 중이라면 무시
        if (!_isGimmickActive || _isTriggered) return;

        _isTriggered = true;

        if (_spawnCoroutine != null) StopCoroutine(_spawnCoroutine);
        _spawnCoroutine = StartCoroutine(SpawnSecondProfRoutine());
    }

    private IEnumerator SpawnSecondProfRoutine()
    {
        Debug.Log($"[Gimmick_ProfSecond] 첫 번째 교수님 교차 감지! {_spawnDelay}초 후 두 번째 교수님 등장");

        yield return new WaitForSeconds(_spawnDelay);

        if (_isGimmickActive && _secondProfMover != null)
        {
            SetSecondProfVisuals(true);
            Debug.Log("[Gimmick_ProfSecond] 두 번째 교수님 등장!");
        }
    }

    /// <summary>
    /// TestReset/GimmickManager에서 OnGimmickTriggered 이벤트로 _code 번호가 호출될 때 실행
    /// </summary>
    protected override void ExecuteGimmick()
    {
        Debug.Log($"[Gimmick_ProfSecond] 기믹 활성화 신호 수신 (ID: {_code}). 교수님 교차 감지 대기 시작!");
        _isGimmickActive = true;
        _isTriggered = false;

        // E키를 눌러 기믹이 켜졌더라도 플레이어가 지나치기 전까지는 두 번째 교수님을 숨겨둠
        SetSecondProfVisuals(false);
    }

    /// <summary>
    /// OnGimmickReset 이벤트 발생 시 초기화
    /// </summary>
    protected override void ResetGimmick()
    {
        _isGimmickActive = false;
        _isTriggered = false;

        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }

        SetSecondProfVisuals(false);
    }

    /// <summary>
    /// 두 번째 교수님의 렌더러/자식 모델 켜고 끄기 제어
    /// </summary>
    private void SetSecondProfVisuals(bool active)
    {
        if (_secondProfMover != null)
        {
            for (int i = 0; i < _secondProfMover.transform.childCount; i++)
            {
                _secondProfMover.transform.GetChild(i).gameObject.SetActive(active);
            }
        }
    }
}