using UnityEngine;

public class Gimmick_StageFlip_Taesu : BasicGimmick
{
    public Define.GimmickLV1 myGimmick = Define.GimmickLV1.stageFlip;

    public Transform mapTransform;

    public float hallwayHeight = 5.0f;

    private Quaternion originalMapRotation;
    private Vector3 originalMapPosition;
    private bool isGimmickActive = false;
    private bool isInitialized = false; // 초기화 여부 플래그

    private void Awake()
    {
        // 💡 OnEnable이나 ExecuteGimmick보다 먼저 실행되도록 Awake에서 초기 좌표 미리 저장!
        InitializeOriginalTransform();
    }

    protected override void OnEnable()
    {
        _code = (int)myGimmick;
        base.OnEnable();
    }

    private void Start()
    {
        InitializeOriginalTransform();
    }

    private void InitializeOriginalTransform()
    {
        if (isInitialized) return;

        if (mapTransform == null && transform.childCount > 0)
        {
            mapTransform = transform.GetChild(0);
        }

        if (mapTransform != null)
        {
            // 💡 부모-자식 구조에서는 localPosition / localRotation을 사용해야 안전함
            originalMapRotation = mapTransform.localRotation;
            originalMapPosition = mapTransform.localPosition;
            isInitialized = true;
        }
    }

    protected override void ExecuteGimmick()
    {
        if (isGimmickActive) return;

        // 혹시 몰라 실행 직전에도 초기화 확인
        InitializeOriginalTransform();

        UnityEngine.Debug.Log("기믹 실행");

        if (mapTransform != null)
        {
            // 💡 local 기준 회전 및 위치 오프셋 적용
            mapTransform.localRotation = originalMapRotation * Quaternion.Euler(0f, 0f, 180f);
            mapTransform.localPosition = originalMapPosition + new Vector3(0f, hallwayHeight, 0f);
        }

        isGimmickActive = true;
    }

    protected override void ResetGimmick()
    {
        if (!isGimmickActive) return;

        UnityEngine.Debug.Log("기믹 초기화");

        if (mapTransform != null)
        {
            // 💡 local 기준 원래 위치/회전 복구
            mapTransform.localRotation = originalMapRotation;
            mapTransform.localPosition = originalMapPosition;
        }

        isGimmickActive = false;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}