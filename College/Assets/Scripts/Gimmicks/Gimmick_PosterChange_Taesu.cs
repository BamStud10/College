using UnityEngine;

public class Gimmik_PosterChange_Taesu : BasicGimmick
{
    [Header("기믹 설정")]
    public Define.GimmickLV2 myGimmick = Define.GimmickLV2.posterChange;

    [Header("타겟 오브젝트 (자동 할당)")]
    [Tooltip("실제 포스터 Quad/Mesh 오브젝트. 비워두면 첫 번째 자식 오브젝트를 자동으로 사용합니다.")]
    public GameObject targetPoster;

    [Header("임시 테스트 색상 설정")]
    public Color anomalyColor = Color.red;

    [Header("크기 변경 설정")]
    [Tooltip("기믹 발동 시 적용할 스케일 배율")]
    public Vector3 targetScaleMultiplier = new Vector3(2.5f, 2.5f, 1f);

    private MeshRenderer targetMeshRenderer;
    private Color originalColor;
    private Vector3 originalScale;
    private bool isGimmickActive = false;

    private void Reset()
    {
        AutoAssignTarget();
    }

    private void OnValidate()
    {
        if (targetPoster == null)
        {
            AutoAssignTarget();
        }
    }

    private void AutoAssignTarget()
    {
        _code = (int)myGimmick;
        // 자식 오브젝트가 존재하면 첫 번째 자식을 타겟으로 지정
        if (transform.childCount > 0 && targetPoster == null)
        {
            targetPoster = transform.GetChild(0).gameObject;
        }
    }

    protected override void OnEnable()
    {
        _code = (int)myGimmick;
        base.OnEnable();
    }

    private void Start()
    {
        if (targetPoster == null)
        {
            AutoAssignTarget();
        }

        if (targetPoster != null)
        {
            originalScale = targetPoster.transform.localScale;
            targetMeshRenderer = targetPoster.GetComponent<MeshRenderer>();

            if (targetMeshRenderer != null && targetMeshRenderer.material != null)
            {
                originalColor = targetMeshRenderer.material.color;
            }
        }
        else
        {
            Debug.LogWarning($"[{GetType().Name}] 하위 오브젝트(실제 포스터)를 찾을 수 없습니다!");
        }
    }

    protected override void ExecuteGimmick()
    {
        if (isGimmickActive || targetPoster == null) return;

        Debug.Log($"기믹 실행");

        // 1. 하위 포스터 오브젝트 크기 변경
        targetPoster.transform.localScale = Vector3.Scale(originalScale, targetScaleMultiplier);

        // 2. 하위 포스터 머티리얼 색상 변경
        if (targetMeshRenderer != null)
        {
            targetMeshRenderer.material.color = anomalyColor;
        }

        isGimmickActive = true;
    }

    protected override void ResetGimmick()
    {
        if (!isGimmickActive || targetPoster == null) return;

        Debug.Log($"기믹 초기화");

        // 1. 하위 포스터 오브젝트 크기 복구
        targetPoster.transform.localScale = originalScale;

        // 2. 하위 포스터 머티리얼 색상 복구
        if (targetMeshRenderer != null)
        {
            targetMeshRenderer.material.color = originalColor;
        }

        isGimmickActive = false;
    }
}