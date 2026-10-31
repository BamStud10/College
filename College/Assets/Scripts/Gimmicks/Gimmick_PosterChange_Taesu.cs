using UnityEngine;

public class Gimmik_PosterChange_Taesu : BasicGimmick
{
    [Header("기믹 설정")]
    public Define.GimmickLV2 myGimmick = Define.GimmickLV2.posterChange;

    [Header("타겟 포스터 오브젝트들 (자동 할당 가능)")]
    [Tooltip("변경할 포스터 오브젝트들을 등록하세요. 비워두면 하위의 모든 오브젝트를 자동으로 찾습니다.")]
    public GameObject[] targetPosters;

    [Header("기믹 이미지 경로 설정 (Resources 폴더 기준)")]
    [Tooltip("예: Assets/Textures/gimmick_game_poster/gimmick_game_poster_1")]
    public string gimmickTexturePath = "Assets/Textures/gimmick_game_poster/gimmick_game_poster_1";

    private MeshRenderer[] targetMeshRenderers;
    private Texture[] originalTextures;
    private Texture gimmickTexture;
    private bool isGimmickActive = false;

    private void Reset()
    {
        AutoAssignTargets();
    }

    private void OnValidate()
    {
        if (targetPosters == null || targetPosters.Length == 0)
        {
            AutoAssignTargets();
        }
    }

    private void AutoAssignTargets()
    {
        _code = (int)myGimmick;

        // 자식 오브젝트들이 존재하고 배열이 비어있다면 전부 자동으로 가져오기
        if (transform.childCount > 0 && (targetPosters == null || targetPosters.Length == 0))
        {
            targetPosters = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                targetPosters[i] = transform.GetChild(i).gameObject;
            }
        }
    }

    protected override void OnEnable()
    {
        _code = (int)myGimmick;
        base.OnEnable();
    }

    private void Start()
    {
        if (targetPosters == null || targetPosters.Length == 0)
        {
            AutoAssignTargets();
        }

        // Resources 폴더에서 기믹 이미지 미리 로드
        gimmickTexture = Resources.Load<Texture>(gimmickTexturePath);
        if (gimmickTexture == null)
        {
            Debug.LogWarning($"[{GetType().Name}] 기믹 텍스처를 불러오지 못했습니다. 경로를 확인해주세요: {gimmickTexturePath}");
        }

        if (targetPosters != null && targetPosters.Length > 0)
        {
            targetMeshRenderers = new MeshRenderer[targetPosters.Length];
            originalTextures = new Texture[targetPosters.Length];

            for (int i = 0; i < targetPosters.Length; i++)
            {
                if (targetPosters[i] != null)
                {
                    targetMeshRenderers[i] = targetPosters[i].GetComponent<MeshRenderer>();

                    if (targetMeshRenderers[i] != null && targetMeshRenderers[i].material != null)
                    {
                        // 각 포스터의 원래 텍스처 백업
                        originalTextures[i] = targetMeshRenderers[i].material.mainTexture;
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning($"[{GetType().Name}] 하위 포스터 오브젝트를 찾을 수 없습니다!");
        }
    }

    protected override void ExecuteGimmick()
    {
        if (isGimmickActive || targetPosters == null || targetPosters.Length == 0) return;

        Debug.Log($"포스터 기믹 실행 (모든 포스터 이미지 교체)");

        // 등록된 모든 포스터의 머티리얼 텍스처를 기믹 이미지로 교체
        for (int i = 0; i < targetMeshRenderers.Length; i++)
        {
            if (targetMeshRenderers[i] != null && gimmickTexture != null)
            {
                targetMeshRenderers[i].material.mainTexture = gimmickTexture;
            }
        }

        isGimmickActive = true;
    }

    protected override void ResetGimmick()
    {
        if (!isGimmickActive || targetPosters == null || targetPosters.Length == 0) return;

        Debug.Log($"포스터 기믹 초기화 (모든 포스터 원래 이미지로 복구)");

        // 모든 포스터의 텍스처를 각자의 원래 이미지로 복구
        for (int i = 0; i < targetMeshRenderers.Length; i++)
        {
            if (targetMeshRenderers[i] != null && originalTextures[i] != null)
            {
                targetMeshRenderers[i].material.mainTexture = originalTextures[i];
            }
        }

        isGimmickActive = false;
    }
}