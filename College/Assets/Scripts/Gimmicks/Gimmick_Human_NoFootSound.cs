using UnityEngine;

public class Gimmick_Human_NoFootSound : BasicGimmick
{
    [Header("기믹 설정")]
    public Define.GimmickLV3 myGimmick = Define.GimmickLV3.silenceWalk;

    public PLFootstep_Taesu _playerFootstep;

    private void Awake()
    {
        // 부모 클래스의 기믹 식별용 변수를 세팅합니다.
        _code = (int)myGimmick;
    }

    protected override void ExecuteGimmick()
    {
        if (_playerFootstep != null)
        {
            // 기믹 발동: 발소리를 관리하는 스크립트 자체를 꺼버립니다.
            _playerFootstep.enabled = false;
            Debug.Log("[Gimmick] 이상현상 발동: 플레이어의 발소리가 사라졌습니다.");
        }
    }

    protected override void ResetGimmick()
    {
        if (_playerFootstep != null)
        {
            // 루프 초기화: 다음 루프 정상 진행을 위해 발소리 스크립트를 다시 켭니다.[cite: 11]
            _playerFootstep.enabled = true;
        }
    }

}
