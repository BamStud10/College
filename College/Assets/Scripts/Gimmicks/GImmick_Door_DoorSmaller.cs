using UnityEngine;

/// <summary>
/// DoorSmaller 기믹 (ID: 103 / Define.GimmickLV2.doorSmaller)
/// - 플레이어가 문에 가까워질수록 자식 문(Door Visual)의 크기가 점점 작아지는 기믹
/// - 컨트롤러 부모 오브젝트는 항시 활성화되어 매니저 이벤트를 수신함
/// </summary>
public class Gimmick_Door_DoorSmaller : BasicGimmick
{
    [Header("기믹 설정")]
    [Tooltip("Define.cs의 LV2 기믹 (doorSmaller = 103)")]
    public Define.GimmickLV2 myGimmick = Define.GimmickLV2.doorSmaller;

    [Header("자식 비주얼 및 플레이어 연결")]
    [Tooltip("축소시킬 자식 문 오브젝트 Transform")]
    [SerializeField] private Transform _doorVisualTarget;

    [Tooltip("거리 계산용 플레이어 Transform (미지정 시 자동 탐색)")]
    [SerializeField] private Transform _playerTransform;

    [Header("거리 및 축소 범위 설정")]
    [Tooltip("축소가 시작되는 최대 거리 (이 거리보다 가까워지면 줄어들기 시작)")]
    [SerializeField] private float _maxDistance = 10f;

    [Tooltip("최대 축소 거리 (이 거리 이하로 접근하면 최소 스케일 도달)")]
    [SerializeField] private float _minDistance = 2f;

    [Tooltip("플레이어가 가장 가까이 갔을 때의 최소 스케일 비율 (0.1 = 10% 크기)")]
    [SerializeField] private float _minScaleRatio = 0.1f;

    [Tooltip("스케일 변화의 부드러움 (Lerp 속도)")]
    [SerializeField] private float _smoothSpeed = 5f;

    // 원본 스케일 및 기믹 상태 저장
    private Vector3 _initialLocalScale = Vector3.one;
    private bool _isGimmickActive = false;
    private Vector3 _targetScale;

    private void Awake()
    {
        _code = (int)myGimmick;

        // 자식 비주얼의 원래 초기 스케일 저장
        if (_doorVisualTarget != null)
        {
            _initialLocalScale = _doorVisualTarget.localScale;
        }

        _targetScale = _initialLocalScale;
        ResetGimmick();
    }

    private void Update()
    {
        if (!_isGimmickActive || _doorVisualTarget == null) return;

        // 플레이어 찾기 (초기 지정이 안 되어있을 경우 자동으로 PlayerSample 탐색)
        if (_playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                player = GameObject.Find("PlayerSample");
            }

            if (player != null)
            {
                _playerTransform = player.transform;
            }
            else
            {
                return; // 플레이어를 찾지 못하면 거리 계산 중단
            }
        }

        // 플레이어와의 거리 계산 (XZ 평면 또는 3D 거리)
        float distance = Vector3.Distance(_playerTransform.position, _doorVisualTarget.position);

        // 거리를 0 ~ 1 사이 비율로 정규화 (minDistance 근처면 0, maxDistance 이상이면 1)
        float t = Mathf.InverseLerp(_minDistance, _maxDistance, distance);

        // t가 0에 가까울수록(가까워질수록) minScaleRatio 적용, t가 1이면(멀어지면) 원래 스케일
        float currentScaleMultiplier = Mathf.Lerp(_minScaleRatio, 1f, t);

        _targetScale = _initialLocalScale * currentScaleMultiplier;

        // 프레임별 부드러운 스케일 보간
        _doorVisualTarget.localScale = Vector3.Lerp(_doorVisualTarget.localScale, _targetScale, Time.deltaTime * _smoothSpeed);
    }

    /// <summary>
    /// 기믹 발동: 플레이어 거리 감지 루프 시작
    /// </summary>
    protected override void ExecuteGimmick()
    {
        Debug.Log($"[Gimmick_DoorSmaller] 기믹 활성화: 문 접근 시 축소 시작 (ID: {_code})");
        _isGimmickActive = true;
    }

    /// <summary>
    /// 초기화: 문 스케일을 원래 상태로 즉시 복원
    /// </summary>
    protected override void ResetGimmick()
    {
        _isGimmickActive = false;

        if (_doorVisualTarget != null)
        {
            _doorVisualTarget.localScale = _initialLocalScale;
            _targetScale = _initialLocalScale;
        }
    }
}