using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimmick_Clock_ClockChange : BasicGimmick
{
    [Header("기믹 설정")]
    [Tooltip("Define.cs에 정의된 기믹 Enum")]
    public Define.GimmickLV2 myGimmick = Define.GimmickLV2.clockChange;

    [Header("초침 원본 연결")]
    [Tooltip("평소에 돌아가는 기준 기본 초침 (복제의 원본이 됨)")]
    [SerializeField] private Transform _baseSecondHand;

    [Header("분열 설정")]
    [Tooltip("기믹 발생 시 추가로 생성할 초침 개수 (예: 2개면 기본 1개 포함 총 3개)")]
    [SerializeField] private int _targetExtraHandCount = 2;

    [Tooltip("초침이 하나씩 추가되는 시간 간격 (초 단위)")]
    [SerializeField] private float _splitInterval = 1.0f;

    [Header("회전 속도 및 축")]
    [Tooltip("초침이 회전할 로컬 축 (시계 방향 기준)")]
    [SerializeField] private Vector3 _rotationAxis = Vector3.forward;

    [Tooltip("기본 정상 회전 속도 (초당 각도, 6도 = 1초에 1칸)")]
    [SerializeField] private float _normalSpeed = 6f;

    [Tooltip("초침 1개 추가될 때마다 증가할 가속도")]
    [SerializeField] private float _speedStep = 90f;

    private float _currentSpeed;
    private Coroutine _clockRoutine;

    // 동적으로 생성된 초침들을 추적하기 위한 리스트
    private readonly List<Transform> _spawnedHands = new List<Transform>();

    private void Awake()
    {
        _code = (int)myGimmick;
        ResetGimmick();
    }

    private void Update()
    {
        float step = -_currentSpeed * Time.deltaTime;

        // 1. 기본 초침 회전
        if (_baseSecondHand != null)
        {
            _baseSecondHand.Rotate(_rotationAxis, step, Space.Self);
        }

        // 2. 동적으로 생성된 추가 초침들 동시 회전
        for (int i = 0; i < _spawnedHands.Count; i++)
        {
            if (_spawnedHands[i] != null)
            {
                _spawnedHands[i].Rotate(_rotationAxis, step, Space.Self);
            }
        }
    }

    /// <summary>
    /// 기믹 활성화: 점진적 초침 동적 복제 및 가속 루틴 시작
    /// </summary>
    protected override void ExecuteGimmick()
    {
        Debug.Log($"[Gimmick_ClockChange] 기믹 활성화: 동적 초침 생성 시작 (ID: {_code})");

        if (_clockRoutine != null)
        {
            StopCoroutine(_clockRoutine);
        }

        _clockRoutine = StartCoroutine(SpawnAndSpeedUpRoutine());
    }

    /// <summary>
    /// 지정한 시간 간격마다 기본 초침을 복제하고 속도를 올리는 코루틴
    /// </summary>
    private IEnumerator SpawnAndSpeedUpRoutine()
    {
        if (_baseSecondHand == null) yield break;

        // 총 초침 개수에 맞춰 균등하게 퍼질 각도 간격 계산
        int totalHands = _targetExtraHandCount + 1;
        float angleStep = 360f / totalHands;

        for (int i = 1; i <= _targetExtraHandCount; i++)
        {
            yield return new WaitForSeconds(_splitInterval);

            // 1. 기본 초침을 똑같이 복제 (부모는 기본 초침의 부모로 지정)
            Transform newHand = Instantiate(_baseSecondHand, _baseSecondHand.parent);
            newHand.name = $"SecondHand_Cloned_{i}";

            // 2. 기본 초침의 현재 회전값에 오프셋 각도를 더해 분열된 느낌 연출
            newHand.localRotation = _baseSecondHand.localRotation * Quaternion.AngleAxis(angleStep * i, _rotationAxis);

            // 3. 리스트에 담아 회전 및 삭제 추적
            _spawnedHands.Add(newHand);

            // 4. 속도 단계별 증가
            _currentSpeed = _normalSpeed + (_speedStep * i);
        }
    }

    /// <summary>
    /// 초기화: 진행 중인 코루틴 중단, 복제된 초침 전부 삭제, 속도 원복
    /// </summary>
    protected override void ResetGimmick()
    {
        // 1. 코루틴 중단
        if (_clockRoutine != null)
        {
            StopCoroutine(_clockRoutine);
            _clockRoutine = null;
        }

        // 2. 동적으로 생성했던 초침 오브젝트 전부 파괴
        for (int i = 0; i < _spawnedHands.Count; i++)
        {
            if (_spawnedHands[i] != null)
            {
                Destroy(_spawnedHands[i].gameObject);
            }
        }
        _spawnedHands.Clear();

        // 3. 정상 속도로 복귀
        _currentSpeed = _normalSpeed;
    }
}