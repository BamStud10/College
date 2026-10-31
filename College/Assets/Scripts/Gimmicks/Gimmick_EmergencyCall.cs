using UnityEngine;

public class Gimmick_EmergencyCall : BasicGimmick
{
    [Header("기믹 설정")]
    public Define.GimmickLV1 myGimmick = Define.GimmickLV1.emergencyCall; // Define에 선언된 이름 확인

    [Header("오브젝트 교체 설정")]
    [Tooltip("기존에 복도를 돌아다니는 정상 교수 오브젝트")]
    public GameObject normalProfessor;

    [Tooltip("벽/천장/바닥에 상체만 박혀있는 비상 교수 오브젝트들 (3개 이상 등록 가능)")]
    public GameObject[] emergencyProfessors;

    private bool isGimmickActive = false;

    private void Awake()
    {
        _code = (int)myGimmick;
    }

    protected override void OnEnable()
    {
        _code = (int)myGimmick;
        base.OnEnable();
    }

    private void Start()
    {
        // 초기 상태: 정상 교수는 보이고, 비상 교수들은 전부 숨김
        if (normalProfessor != null)
        {
            normalProfessor.SetActive(true);
        }

        if (emergencyProfessors != null)
        {
            foreach (var prof in emergencyProfessors)
            {
                if (prof != null) prof.SetActive(false);
            }
        }
    }

    protected override void ExecuteGimmick()
    {
        if (isGimmickActive) return;

        Debug.Log("교수 비상 호출 기믹 실행 (여러 명 등장)");

        // 1. 정상 교수 비활성화
        if (normalProfessor != null)
        {
            normalProfessor.SetActive(false);
        }

        // 2. 등록된 모든 비상 교수 오브젝트들을 일괄 활성화
        if (emergencyProfessors != null)
        {
            foreach (var prof in emergencyProfessors)
            {
                if (prof != null) prof.SetActive(true);
            }
        }

        isGimmickActive = true;
    }

    protected override void ResetGimmick()
    {
        if (!isGimmickActive) return;

        Debug.Log("비상 호출 기믹 초기화");

        // 1. 비상 교수들 전부 숨기기
        if (emergencyProfessors != null)
        {
            foreach (var prof in emergencyProfessors)
            {
                if (prof != null) prof.SetActive(false);
            }
        }

        // 2. 정상 교수 다시 복구
        if (normalProfessor != null)
        {
            normalProfessor.SetActive(true);
        }

        isGimmickActive = false;
    }
}