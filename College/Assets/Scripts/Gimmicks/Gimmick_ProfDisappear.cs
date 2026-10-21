using UnityEngine;

public class Gimmick_ProfDisappear : BasicGimmick
{
    [Header("기믹 식별 설정")]
    public Define.GimmickLV2 myGimmick = Define.GimmickLV2.profDisappear;

    [Header("제어 타겟 (교수 NPC 연결)")]
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
        Debug.Log("[Gimmick] 교수 NPC 사라짐 기믹 발동!");
        // 기믹 발동 시 교수가 복도에서 사라짐
        if (professorObject != null) professorObject.SetActive(false);
    }

    protected override void ResetGimmick()
    {
        // 정상 상태(기믹 미발동)일 때는 교수가 항상 서 있음
        if (professorObject != null) professorObject.SetActive(true);
    }
}
