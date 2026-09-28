using UnityEngine;

public class Gimmik_PosterChange_Taesu : BasicGimmick
{
    [Header("기믹 설정")]
    [Tooltip("Define.GimmickLV2 중 posterChange 선택")]
    public Define.GimmickLV2 myGimmick = Define.GimmickLV2.posterChange;

    [Header("임시 테스트 색상 설정")]
    [Tooltip("기믹 발동 시 변경될 이상현상 색상 (기본: 빨간색)")]
    public Color anomalyColor = Color.red;

    [Header("크기 변경 설정")]
    [Tooltip("기믹 발동 시 적용할 스케일 배율 (예: X=2.5, Y=2.5, Z=1)")]
    public Vector3 targetScaleMultiplier = new Vector3(2.5f, 2.5f, 1f);

    private MeshRenderer meshRenderer;
    private Color originalColor;
    private Vector3 originalScale;
    private bool isGimmickActive = false;

    protected override void OnEnable()
    {
        // LV2 enum 값을 ID 코드로 설정
        _code = (int)myGimmick;
        base.OnEnable();
    }

    private void Start()
    {
        // 포스터 오브젝트의 초기 스케일 저장
        originalScale = transform.localScale;

        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null && meshRenderer.material != null)
        {
            // 에디터에서 설정해둔 기본 색상(파란색) 저장
            originalColor = meshRenderer.material.color;
        }
    }

    protected override void ExecuteGimmick()
    {
        if (isGimmickActive) return;

        Debug.Log($"기믹 실행");

        // 1. 포스터 거대화
        transform.localScale = Vector3.Scale(originalScale, targetScaleMultiplier);

        // 2. 이상현상 임시 색상(빨간색)으로 변경
        if (meshRenderer != null)
        {
            meshRenderer.material.color = anomalyColor;
        }

        isGimmickActive = true;
    }

    protected override void ResetGimmick()
    {
        if (!isGimmickActive) return;

        Debug.Log($"기믹 초기화");

        // 1. 크기 원복
        transform.localScale = originalScale;

        // 2. 원래 색상(파란색)으로 복구
        if (meshRenderer != null)
        {
            meshRenderer.material.color = originalColor;
        }

        isGimmickActive = false;
    }
}