using UnityEngine;

public class Gimmick_ProfEyeOnPlayer : BasicGimmick
{
    [Header("기믹 식별 설정")]
    // Define.GimmickLV1 에 profLook 등을 미리 추가해 주세요.
    public Define.GimmickLV2 myGimmick = Define.GimmickLV2.profEyeOnPlayer;

    [Header("제어 타겟 연결")]
    [Tooltip("하이어라키에서 mixamorig:Neck 오브젝트를 연결하세요.")]
    public Transform neckBone;

    [Tooltip("플레이어 오브젝트(Player 또는 Main Camera)를 연결하세요.")]
    public Transform playerTarget;

    [Header("축 보정 (목이 이상하게 꺾일 경우 조절)")]
    [Tooltip("Mixamo 모델링은 축이 다를 수 있습니다. 귀나 정수리로 플레이어를 본다면 이 값을 x, y, z 중 90이나 -90으로 조절하세요.")]
    public Vector3 rotationOffset = new Vector3(0, 0, 0);

    private bool _isGimmickActive = false;

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
        Debug.Log("[Gimmick] 교수 시선 고정 기믹 발동!");
        _isGimmickActive = true;
    }

    protected override void ResetGimmick()
    {
        _isGimmickActive = false;
    }

    // 애니메이션이 적용된 직후에 뼈대 각도를 덮어씌워야 하므로 LateUpdate를 사용합니다.
    private void LateUpdate()
    {
        if (_isGimmickActive && neckBone != null && playerTarget != null)
        {
            // 1. 플레이어 방향을 바라보게 합니다.
            neckBone.LookAt(playerTarget.position);

            // 2. 모델링 고유의 축이 틀어져 있을 경우를 대비한 보정값을 더해줍니다.
            neckBone.Rotate(rotationOffset);
        }
    }
}
