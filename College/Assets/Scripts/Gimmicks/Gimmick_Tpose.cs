using UnityEngine;
using Unity.Cinemachine; // Cinemachine 패키지 네임스페이스 (버전에 따라 UnityStandardAssets 등일 수 있음)

public class Gimmick_Tpose : BasicGimmick
{
    [Header("기믹 설정")]
    public Define.GimmickLV1 myGimmick = Define.GimmickLV1.Tpose;

    [Header("스플라인 카트 참조")]
    [Tooltip("교수가 타고 있는 부모 카트 (Gimmick_Profs)")]
    public CinemachineSplineCart profCart;

    [Tooltip("대학원생이 탈 부모 카트 (Gimmick_Tpose 내 자신)")]
    public CinemachineSplineCart studentCart;

    [Header("타겟 오브젝트 설정")]
    [Tooltip("기존 교수 오브젝트 (Professor_Retopo_custom)")]
    public GameObject professorObject;

    [Tooltip("등장시킬 T포즈 대학원생 오브젝트 (GradStudent_Retopo)")]
    public GameObject gradStudentObject;

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
        // 내 오브젝트의 SplineCart 자동 할당
        if (studentCart == null)
        {
            studentCart = GetComponent<CinemachineSplineCart>();
        }

        // 초기 상태: 교수는 보이고, T포즈 대학원생은 숨김
        if (professorObject != null) professorObject.SetActive(true);
        if (gradStudentObject != null) gradStudentObject.SetActive(false);
    }

    protected override void ExecuteGimmick()
    {
        if (isGimmickActive) return;

        Debug.Log("T포즈 대학원생 변형 기믹 실행");

        if (profCart != null && studentCart != null)
        {
            // 1. 교수의 현재 스플라인 진행 위치(SplinePosition)를 대학원생 카트로 복사
            studentCart.SplinePosition = profCart.SplinePosition;
        }

        // 2. 교수 비활성화 및 대학원생 활성화
        if (professorObject != null) professorObject.SetActive(false);
        if (gradStudentObject != null) gradStudentObject.SetActive(true);

        isGimmickActive = true;
    }

    protected override void ResetGimmick()
    {
        if (!isGimmickActive) return;

        Debug.Log("T포즈 대학원생 기믹 초기화");

        if (gradStudentObject != null) gradStudentObject.SetActive(false);
        if (professorObject != null) professorObject.SetActive(true);

        isGimmickActive = false;
    }
}