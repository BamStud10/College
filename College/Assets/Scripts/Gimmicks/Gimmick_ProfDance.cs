using UnityEngine;

public class Gimmick_ProfDance : BasicGimmick
{
    [Header("기믹 식별 설정")]
    public Define.GimmickLV1 myGimmick = Define.GimmickLV1.profDance;

    [Header("제어 타겟 (교수 오브젝트 연결)")]
    public Animator profAnimator;

    private void Awake()
    {
        _code = (int)myGimmick;
        ResetGimmick();
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

    private void CheckAndReset(int triggeredID)
    {
        if (triggeredID != _code)
        {
            ResetGimmick();
        }
    }

    protected override void ExecuteGimmick()
    {
        Debug.Log("[Gimmick] 교수 댄스 기믹 발동!");
        if (profAnimator != null)
        {
            // 기믹 발동 시 춤 상태로 전환
            profAnimator.SetBool("isDancing", true);
        }
    }

    protected override void ResetGimmick()
    {
        if (profAnimator != null)
        {
            // 평상시(루프)에는 다시 걷기(Walking) 상태로 복구
            profAnimator.SetBool("isDancing", false);
        }
    }
}
