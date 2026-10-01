using UnityEngine;

/// <summary>
/// RoadChange 기믹 (ID: 1단계 기믹 범위)
/// - 부모 오브젝트(컨트롤러): 스크립트 부착 및 항상 Active 유지 (매니저 이벤트 수신 보장)
/// - 자식 오브젝트(비주얼): 머티리얼을 변경할 대상 MeshRenderer
/// </summary>
public class Gimmick_Road_RoadChange : BasicGimmick
{
    [Header("기믹 설정")]
    [Tooltip("Define.cs에 정의된 기믹 Enum (예: roadChange)")]
    public Define.GimmickLV1 myGimmick = Define.GimmickLV1.roadChange;

    [Header("머티리얼 변경 설정")]
    [Tooltip("머티리얼을 변경할 대상 도로의 MeshRenderer (자식 오브젝트)")]
    [SerializeField] private MeshRenderer _roadRenderer;

    [Tooltip("정상 상태일 때의 머티리얼")]
    [SerializeField] private Material _normalMaterial;

    [Tooltip("기믹 발동(이상 현상) 시 변경될 머티리얼 (예: 흙길 등)")]
    [SerializeField] private Material _anomalyMaterial;

    private void Awake()
    {
        _code = (int)myGimmick;
        ResetGimmick();
    }

    /// <summary>
    /// 기믹 발동: 이상 현상 머티리얼로 교체
    /// </summary>
    protected override void ExecuteGimmick()
    {
        Debug.Log($"[Gimmick_RoadChange] 기믹 활성화: 도로 머티리얼 변경 (ID: {_code})");

        if (_roadRenderer != null && _anomalyMaterial != null)
        {
            _roadRenderer.material = _anomalyMaterial;
        }
    }

    /// <summary>
    /// 초기화: 기본 머티리얼로 원복
    /// </summary>
    protected override void ResetGimmick()
    {
        if (_roadRenderer != null && _normalMaterial != null)
        {
            _roadRenderer.material = _normalMaterial;
        }
    }
}