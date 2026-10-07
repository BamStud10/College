using UnityEngine;

public class Gimmick_ProfDisappear : BasicGimmick
{
    [Header("기믹 식별 설정")]
    public Define.GimmickLV2 myGimmick = Define.GimmickLV2.profDisappear;

    [Header("제어 타겟 (테스트용 큐브 연결)")]
    public GameObject professorObject;

    private void Awake()
    {
        _code = (int)myGimmick;
        ResetGimmick(); // 씬 시작 시 교수님이 무조건 방에 존재하도록 보장
    }

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

    // 다른 기믹이 당첨되거나 정상 상태면 교수님을 다시 등장시킴
    private void CheckAndReset(int triggeredID)
    {
        if (triggeredID != _code)
        {
            ResetGimmick();
        }
    }

    protected override void ExecuteGimmick()
    {
        Debug.Log("[Gimmick] 교수님 실종 발생! (오브젝트 비활성화)");
        if (professorObject != null)
        {
            professorObject.SetActive(false);
        }
    }

    protected override void ResetGimmick()
    {
        if (professorObject != null)
        {
            professorObject.SetActive(true);
        }
    }
}
