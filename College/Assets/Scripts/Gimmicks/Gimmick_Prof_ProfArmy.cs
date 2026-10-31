using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ProfArmy 기믹 (ID: 9 / GimmickLV1.profArmy)
/// - 평상시: Spline 교수님(ProfMover)이 스플라인을 따라 자동 이동 (돌진 7명 비활성화)
/// - 기믹 발동 시: Spline 교수님의 비주얼 모델을 숨기고, 7명의 돌진 교수님을 켜서 전방으로 빠르게 이동
/// </summary>
public class Gimmick_Prof_ProfArmy : BasicGimmick
{
    [Header("기믹 설정")]
    [Tooltip("Define.cs에 정의된 기믹 Enum (profArmy = 9)")]
    public Define.GimmickLV1 myGimmick = Define.GimmickLV1.profArmy;

    [Header("Spline 교수님 연결")]
    [Tooltip("ProfMover 스크립트가 붙어있는 Spline Cart 오브젝트 (또는 해당 스크립트)")]
    [SerializeField] private ProfMover _splineProfMover;

    [Header("기믹용 돌진 교수님 NPC (7명)")]
    [Tooltip("기믹 발동 시 작동할 7명의 교수님 오브젝트 리스트 (Element 0~6)")]
    [SerializeField] private List<GameObject> _professorList = new List<GameObject>();

    [Header("애니메이션 설정")]
    [Tooltip("애니메이터의 IsRunning 파라미터 이름")]
    [SerializeField] private string _runParamName = "IsRunning";

    [Header("이동 및 속도 설정")]
    [Tooltip("기믹 발동 시 뛰기 속도")]
    [SerializeField] private float _runSpeed = 8.0f;

    [Tooltip("이동 방향 (기본: Vector3.forward -> 정면 전방 방향)")]
    [SerializeField] private Vector3 _moveDirection = Vector3.forward;

    // 초기 로컬 좌표, 회전값 및 애니메이터 캐싱
    private List<Vector3> _initialLocalPositions = new List<Vector3>();
    private List<Quaternion> _initialLocalRotations = new List<Quaternion>();
    private List<Animator> _animators = new List<Animator>();

    private bool _isGimmickActive = false;

    private void Awake()
    {
        _code = (int)myGimmick;

        // 7명의 초기 로컬 Transform 및 Animator 정보 캐싱
        _initialLocalPositions.Clear();
        _initialLocalRotations.Clear();
        _animators.Clear();

        foreach (var prof in _professorList)
        {
            if (prof != null)
            {
                _initialLocalPositions.Add(prof.transform.localPosition);
                _initialLocalRotations.Add(prof.transform.localRotation);
                _animators.Add(prof.GetComponent<Animator>());
            }
        }

        ResetGimmick();
    }

    private void Update()
    {
        // 기믹이 발동되었을 때만 7명의 교수님이 동시에 앞으로 뛰어감
        if (_isGimmickActive)
        {
            for (int i = 0; i < _professorList.Count; i++)
            {
                if (_professorList[i] != null && _professorList[i].activeSelf)
                {
                    _professorList[i].transform.Translate(_moveDirection.normalized * _runSpeed * Time.deltaTime, Space.Self);
                }
            }
        }
    }

    /// <summary>
    /// 기믹 발동: Spline 교수님 모델 숨기기 + 7명 교수님 등장 및 돌진 시작
    /// </summary>
    protected override void ExecuteGimmick()
    {
        Debug.Log($"[Gimmick_ProfArmy] 기믹 활성화: Spline 교수님 전환 후 7명 돌진 (ID: {_code})");

        _isGimmickActive = true;

        // 1. Spline 교수님의 자식 모델(비주얼)들을 모두 숨김
        if (_splineProfMover != null)
        {
            for (int i = 0; i < _splineProfMover.transform.childCount; i++)
            {
                _splineProfMover.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        // 2. 7명 돌진 교수님 활성화 및 뛰기 애니메이션 적용
        for (int i = 0; i < _professorList.Count; i++)
        {
            if (_professorList[i] == null) continue;

            _professorList[i].SetActive(true);

            if (i < _animators.Count && _animators[i] != null)
            {
                _animators[i].SetBool(_runParamName, true);
            }
        }
    }

    /// <summary>
    /// 초기화: 7명 숨기기, 위치 원복, Spline 교수님 모델 다시 켜기
    /// </summary>
    protected override void ResetGimmick()
    {
        _isGimmickActive = false;

        // 1. 돌진 교수님 7명 초기 위치/애니메이션 원복 및 비활성화
        for (int i = 0; i < _professorList.Count; i++)
        {
            if (_professorList[i] == null) continue;

            _professorList[i].transform.localPosition = _initialLocalPositions[i];
            _professorList[i].transform.localRotation = _initialLocalRotations[i];

            if (i < _animators.Count && _animators[i] != null)
            {
                _animators[i].SetBool(_runParamName, false);
            }

            // 기믹이 아닐 때는 7명 모두 끔 (Spline 교수님이 걸어갈 것이므로)
            _professorList[i].SetActive(false);
        }

        // 2. Spline 교수님의 자식 모델(비주얼)들을 다시 켬
        if (_splineProfMover != null)
        {
            for (int i = 0; i < _splineProfMover.transform.childCount; i++)
            {
                _splineProfMover.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}