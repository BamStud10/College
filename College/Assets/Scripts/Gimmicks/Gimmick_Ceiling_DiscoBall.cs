using UnityEngine;

public class Gimmick_Ceiling_DiscoBall : BasicGimmick
{
    [Header("기믹 식별 설정")]
    public Define.GimmickLV1 myGimmick = Define.GimmickLV1.discoBall;

    [Header("오브젝트 세팅")]
    [Tooltip("평소에 켜져 있을 정상 형광등 오브젝트")]
    public GameObject normalLight;

    [Tooltip("실제 미러볼 메쉬와 조명이 들어있는 하위 오브젝트")]
    public GameObject actualMirrorBall; // 자식 오브젝트를 여기에 연결

    [Header("미러볼 회전 설정")]
    public float rotationSpeed = 90f;

    private bool _isGimmickActive = false;

    private void Awake()
    {
        _code = (int)myGimmick;

        // 깜빡하고 하위 오브젝트를 안 넣었을 경우, 자동으로 첫 번째 자식을 찾아주는 센스 있는 코드
        if (actualMirrorBall == null && transform.childCount > 0)
        {
            actualMirrorBall = transform.GetChild(0).gameObject;
        }

        // 시작 시 무조건 정상 상태로 세팅 (미러볼 끄기)
        ResetGimmick();
    }

    protected override void ExecuteGimmick()
    {
        Debug.Log("[Gimmick] 미러볼 기믹 발동!");

        // 1. 정상 조명 끄기
        if (normalLight != null) normalLight.SetActive(false);

        // 2. 내 하위 오브젝트(실제 미러볼) 켜기
        if (actualMirrorBall != null) actualMirrorBall.SetActive(true);

        _isGimmickActive = true;
    }

    protected override void ResetGimmick()
    {
        // 1. 정상 조명 다시 켜기
        if (normalLight != null) normalLight.SetActive(true);

        // 2. 내 하위 오브젝트(실제 미러볼) 숨기기
        if (actualMirrorBall != null) actualMirrorBall.SetActive(false);

        _isGimmickActive = false;
    }

    private void Update()
    {
        // 기믹이 켜져 있을 때만 실제 미러볼 오브젝트를 회전시킴
        if (_isGimmickActive && actualMirrorBall != null)
        {
            actualMirrorBall.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }

    // 부모(BasicGimmick)의 기능을 유지하면서 새로운 이벤트 구독 추가
    protected override void OnEnable()
    {
        base.OnEnable();
        GimmickManager.OnGimmickTriggered += CheckAndReset;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GimmickManager.OnGimmickTriggered -= CheckAndReset;
    }

    // 매니저가 새로운 주사위 결과를 방송할 때마다 실행되는 함수
    private void CheckAndReset(int triggeredID)
    {
        // 방송된 기믹 ID가 내 번호가 아니면 (다른 기믹이 당첨되었거나, 0번 정상 상태면)
        if (triggeredID != _code)
        {
            ResetGimmick(); // 미러볼을 숨기고 정상 조명으로 복구
        }
    }
}
