using System.Diagnostics;
using UnityEngine;

public class Gimmick_StageFlip_Taesu : BasicGimmick
{
    public Define.GimmickLV1 myGimmick = Define.GimmickLV1.stageFlip;

    public Transform mapTransform;

    public float hallwayHeight = 5.0f;

    private Quaternion originalMapRotation;
    private Vector3 originalMapPosition;
    private bool isGimmickActive = false;

    protected override void OnEnable()
    {
        _code = (int)myGimmick;
        base.OnEnable();
    }

    private void Start()
    {
        // mapTransform을 인스펙터에서 따로 비워두면 스크립트가 붙은 자기 자신으로 자동 지정
        if (mapTransform == null)
        {
            mapTransform = transform;
        }

        // 초기 회전값과 위치값 모두 저장!
        originalMapRotation = mapTransform.rotation;
        originalMapPosition = mapTransform.position;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void ExecuteGimmick()
    {
        if (isGimmickActive) return;

        UnityEngine.Debug.Log("기믹 실행");

        if (mapTransform != null)
        {
            mapTransform.rotation = originalMapRotation * Quaternion.Euler(0f, 0f, 180f);
            mapTransform.position = originalMapPosition + new Vector3(0f, hallwayHeight, 0f);
        }

        isGimmickActive = true;
    }

    protected override void ResetGimmick()
    {
        if (!isGimmickActive) return;

        UnityEngine.Debug.Log("기믹 초기화");

        if (mapTransform != null)
        {
            //회전과 위치를 모두 원본값으로 복구
            mapTransform.rotation = originalMapRotation;
            mapTransform.position = originalMapPosition;
        }

        isGimmickActive = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}