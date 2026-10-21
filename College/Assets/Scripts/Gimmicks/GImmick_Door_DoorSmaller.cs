using UnityEngine;

/// <summary>
/// DoorSmaller 기믹 (ID: 103 / Define.GimmickLV2.doorSmaller)
/// - 플레이어가 문에 가까워질수록 자식 문(Door Visual)의 크기가 바닥 기준으로 부드럽게 작아지는 기믹
/// - 피벗이 바닥에 위치한 문 기준: 상/좌/우 3면 여백 메움 실시간 연산
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

    [Header("3면 여백 메움 트랜스폼 (Top, Left, Right)")]
    [Tooltip("상단 여백 메움 큐브")]
    [SerializeField] private Transform _fillerTop;

    [Tooltip("좌측 여백 메움 큐브")]
    [SerializeField] private Transform _fillerLeft;

    [Tooltip("우측 여백 메움 큐브")]
    [SerializeField] private Transform _fillerRight;

    [Header("거리 및 축소 범위 설정")]
    [Tooltip("축소가 시작되는 최대 거리 (이 거리보다 가까워지면 줄어들기 시작)")]
    [SerializeField] private float _maxDistance = 12f;

    [Tooltip("최대 축소 거리 (이 거리 이하로 접근하면 최소 스케일 도달)")]
    [SerializeField] private float _minDistance = 2f;

    [Tooltip("플레이어가 가장 가까이 갔을 때의 최소 스케일 비율 (0.1 = 10% 크기)")]
    [SerializeField] private float _minScaleRatio = 0.1f;

    [Tooltip("스케일 변화의 부드러움 (Lerp 속도, 낮을수록 천천히 줄어듦)")]
    [SerializeField] private float _smoothSpeed = 2.5f;

    // 원본 스케일 및 기믹 상태 저장
    private Vector3 _initialLocalScale = Vector3.one;
    private bool _isGimmickActive = false;
    private Vector3 _targetScale;

    private void Awake()
    {
        _code = (int)myGimmick;

        if (_doorVisualTarget != null)
        {
            _initialLocalScale = _doorVisualTarget.localScale;
        }

        _targetScale = _initialLocalScale;
        SetFillersActive(false);
        ResetGimmick();
    }

    private void Update()
    {
        if (!_isGimmickActive || _doorVisualTarget == null) return;

        // 플레이어 자동 탐색
        if (_playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null) player = GameObject.Find("PlayerSample");

            if (player != null) _playerTransform = player.transform;
            else return;
        }

        // 1. 플레이어와의 거리 계산
        float distance = Vector3.Distance(_playerTransform.position, _doorVisualTarget.position);

        // 2. 0~1 비율 정규화 및 SmoothStep 곡선 적용
        float t = Mathf.InverseLerp(_minDistance, _maxDistance, distance);
        t = Mathf.SmoothStep(0f, 1f, t);

        float currentScaleMultiplier = Mathf.Lerp(_minScaleRatio, 1f, t);
        _targetScale = _initialLocalScale * currentScaleMultiplier;

        // 3. 프레임별 부드러운 스케일 보간
        _doorVisualTarget.localScale = Vector3.Lerp(_doorVisualTarget.localScale, _targetScale, Time.deltaTime * _smoothSpeed);

        // 4. 바닥 피벗 기준 상/좌/우 3면 메움 스케일/위치 실시간 연산
        UpdateWallFillers(_doorVisualTarget.localScale);
    }

    /// <summary>
    /// 바닥 피벗 문 기준 상/좌/우 여백 큐브 계산
    /// </summary>
    private void UpdateWallFillers(Vector3 currentDoorScale)
    {
        float fullWidth = _initialLocalScale.x;
        float fullHeight = _initialLocalScale.y;

        float curWidth = currentDoorScale.x;
        float curHeight = currentDoorScale.y;

        float fullHalfWidth = fullWidth / 2f;
        float curHalfWidth = curWidth / 2f;

        // 1. 상단 메움 (Top): 원본 문 상단과 축소된 문 상단 사이의 빈 공간
        if (_fillerTop != null)
        {
            float topHeight = fullHeight - curHeight;
            _fillerTop.localScale = new Vector3(fullWidth, topHeight, _initialLocalScale.z);
            _fillerTop.localPosition = new Vector3(0f, curHeight + (topHeight / 2f), 0f);
        }

        // 2. 좌측 메움 (Left): 바닥(0)부터 줄어든 문 높이까지의 좌측 빈 공간
        if (_fillerLeft != null)
        {
            float leftWidth = fullHalfWidth - curHalfWidth;
            _fillerLeft.localScale = new Vector3(leftWidth, curHeight, _initialLocalScale.z);
            _fillerLeft.localPosition = new Vector3(-curHalfWidth - (leftWidth / 2f), curHeight / 2f, 0f);
        }

        // 3. 우측 메움 (Right): 바닥(0)부터 줄어든 문 높이까지의 우측 빈 공간
        if (_fillerRight != null)
        {
            float rightWidth = fullHalfWidth - curHalfWidth;
            _fillerRight.localScale = new Vector3(rightWidth, curHeight, _initialLocalScale.z);
            _fillerRight.localPosition = new Vector3(curHalfWidth + (rightWidth / 2f), curHeight / 2f, 0f);
        }
    }

    private void SetFillersActive(bool isActive)
    {
        if (_fillerTop != null) _fillerTop.gameObject.SetActive(isActive);
        if (_fillerLeft != null) _fillerLeft.gameObject.SetActive(isActive);
        if (_fillerRight != null) _fillerRight.gameObject.SetActive(isActive);
    }

    protected override void ExecuteGimmick()
    {
        Debug.Log($"[Gimmick_DoorSmaller] 기믹 활성화: 문 접근 시 축소 시작 (ID: {_code})");
        _isGimmickActive = true;
        SetFillersActive(true);
    }

    protected override void ResetGimmick()
    {
        _isGimmickActive = false;

        if (_doorVisualTarget != null)
        {
            _doorVisualTarget.localScale = _initialLocalScale;
            _targetScale = _initialLocalScale;
        }

        SetFillersActive(false);
    }
}