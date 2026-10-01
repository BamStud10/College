using UnityEngine;

public class Gimmick_StageFlip_Taesu : BasicGimmick
{
    [Header("기믹 설정")]
    public Define.GimmickLV1 myGimmick = Define.GimmickLV1.stageFlip;

    [Header("타겟 맵 Transform (자동 할당)")]
    [Tooltip("실제 뒤집힐 복도/맵 오브젝트. 비워두면 첫 번째 자식을 지정합니다.")]
    public Transform mapTransform;

    [Header("오프셋 설정")]
    public float hallwayHeight = 5.0f;

    private Quaternion originalMapRotation;
    private Vector3 originalMapPosition;
    private bool isGimmickActive = false;

    private void Reset()
    {
        AutoAssignMap();
    }

    private void OnValidate()
    {
        if (mapTransform == null)
        {
            AutoAssignMap();
        }
    }

    private void AutoAssignMap()
    {
        _code = (int)myGimmick;
        if (transform.childCount > 0 && mapTransform == null)
        {
            mapTransform = transform.GetChild(0);
        }
    }

    protected override void OnEnable()
    {
        _code = (int)myGimmick;
        base.OnEnable();
    }

    private void Start()
    {
        if (mapTransform == null)
        {
            AutoAssignMap();
        }

        if (mapTransform != null)
        {
            originalMapRotation = mapTransform.localRotation;
            originalMapPosition = mapTransform.localPosition;
        }
        else
        {
            Debug.LogWarning($"[{GetType().Name}] 하위 맵 오브젝트를 찾을 수 없습니다!");
        }
    }

    protected override void ExecuteGimmick()
    {
        if (isGimmickActive || mapTransform == null) return;

        Debug.Log($"기믹 실행");

        mapTransform.localRotation = originalMapRotation * Quaternion.Euler(0f, 0f, 180f);
        mapTransform.localPosition = originalMapPosition + new Vector3(0f, hallwayHeight, 0f);

        isGimmickActive = true;
    }

    protected override void ResetGimmick()
    {
        if (!isGimmickActive || mapTransform == null) return;

        Debug.Log($"기믹 초기화");

        mapTransform.localRotation = originalMapRotation;
        mapTransform.localPosition = originalMapPosition;

        isGimmickActive = false;
    }
}