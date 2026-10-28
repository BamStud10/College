using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ProfArmy 기믹 (ID: 9 / GimmickLV1.profArmy)
/// - 평상시: 부모는 고정. 0번 원본 교수님만 자식 로컬 좌표에서 앞으로 걷기
/// - 기믹 발동 시: 1~6번 분신 켜기 + 7명 동시에 뛰어감
/// </summary>
public class Gimmick_Prof_ProfArmy : BasicGimmick
{
    [Header("기믹 설정")]
    [Tooltip("Define.cs에 정의된 기믹 Enum (profArmy = 9)")]
    public Define.GimmickLV1 myGimmick = Define.GimmickLV1.profArmy;

    [Header("교수님 NPC 설정")]
    [Tooltip("기믹 발동 시 작동할 7명의 교수님 오브젝트 리스트 (0번: 원본, 1~6번: 분신)")]
    [SerializeField] private List<GameObject> _professorList = new List<GameObject>();

    [Header("애니메이션 설정")]
    [Tooltip("애니메이터의 IsRunning 파라미터 이름")]
    [SerializeField] private string _runParamName = "IsRunning";

    [Header("이동 및 속도 설정")]
    [Tooltip("평상시 걷기 속도")]
    [SerializeField] private float _walkSpeed = 1.5f;

    [Tooltip("기믹 발동 시 뛰기 속도")]
    [SerializeField] private float _runSpeed = 8.0f;

    [Tooltip("이동 방향 (Vector3.forward: 바라보는 정면 방향)")]
    [SerializeField] private Vector3 _moveDirection = Vector3.forward;

    // 자식 교수님들의 초기 로컬 위치/회전값 저장
    private List<Vector3> _initialLocalPositions = new List<Vector3>();
    private List<Quaternion> _initialLocalRotations = new List<Quaternion>();
    private List<Animator> _animators = new List<Animator>();

    private bool _isGimmickActive = false;

    private void Awake()
    {
        _code = (int)myGimmick;

        // 7명의 초기 로컬 Transform 및 Animator 정보 저장
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
        if (_isGimmickActive)
        {
            // [기믹 발동 시] 7명 모두 제자리 대열을 유지한 채 빠른 속도로 돌진
            for (int i = 0; i < _professorList.Count; i++)
            {
                if (_professorList[i] != null && _professorList[i].activeSelf)
                {
                    _professorList[i].transform.Translate(_moveDirection.normalized * _runSpeed * Time.deltaTime, Space.Self);
                }
            }
        }
        else
        {
            // [평상시/정상 상태] 비활성화된 분신들은 가만히 두고, 0번 원본 교수님만 앞으로 걷기
            if (_professorList.Count > 0 && _professorList[0] != null && _professorList[0].activeSelf)
            {
                _professorList[0].transform.Translate(_moveDirection.normalized * _walkSpeed * Time.deltaTime, Space.Self);
            }
        }
    }

    /// <summary>
    /// 기믹 발동: 7명 모두 활성화, 0번 교수님 위치 기준에 맞춰 분신 배치 후 다같이 뛰기
    /// </summary>
    protected override void ExecuteGimmick()
    {
        Debug.Log($"[Gimmick_ProfArmy] 기믹 활성화: 교수님 7명 전체 돌진 (ID: {_code})");

        _isGimmickActive = true;

        // 0번 교수님이 원래 걸어가던 현재 위치를 기준으로 분신들을 대열에 맞춰 정렬
        Vector3 currentProf0Pos = _professorList[0].transform.localPosition;

        for (int i = 0; i < _professorList.Count; i++)
        {
            if (_professorList[i] == null) continue;

            // 0번 교수님의 현재 위치 대비 상대적 오프셋 계산하여 분신 위치 배치
            if (i > 0)
            {
                Vector3 offset = _initialLocalPositions[i] - _initialLocalPositions[0];
                _professorList[i].transform.localPosition = currentProf0Pos + offset;
            }

            // 1. 교수님 오브젝트 활성화
            _professorList[i].SetActive(true);

            // 2. 뛰기 애니메이션(IsRunning = true) 재생
            if (i < _animators.Count && _animators[i] != null)
            {
                _animators[i].SetBool(_runParamName, true);
            }
        }
    }

    /// <summary>
    /// 초기화: 자식 위치/회전 원복, 원본(0번)만 남기고 걷기 애니메이션으로 원복
    /// </summary>
    protected override void ResetGimmick()
    {
        _isGimmickActive = false;

        for (int i = 0; i < _professorList.Count; i++)
        {
            if (_professorList[i] == null) continue;

            // 1. 모든 교수님의 위치를 처음 세팅했던 로컬 위치로 원복
            _professorList[i].transform.localPosition = _initialLocalPositions[i];
            _professorList[i].transform.localRotation = _initialLocalRotations[i];

            // 2. 애니메이션 걷기(IsRunning = false) 상태로 원복
            if (i < _animators.Count && _animators[i] != null)
            {
                _animators[i].SetBool(_runParamName, false);
            }

            // 3. 원본(0번)만 활성화, 분신(1~6번)은 비활성화
            if (i == 0)
            {
                _professorList[i].SetActive(true);
            }
            else
            {
                _professorList[i].SetActive(false);
            }
        }
    }
}