using System.Diagnostics;
using UnityEngine;

public class Gimmik_StageFlip_Taesu : BasicGimmick
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
        if (mapTransform != null)
        {
            originalMapRotation = mapTransform.rotation;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void ExecuteGimmick()
    {
        if (isGimmickActive) return;

        UnityEngine.Debug.Log("±‚πÕ Ω««‡");

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

        UnityEngine.Debug.Log("±‚πÕ √ ±‚»≠");

        if (mapTransform != null)
        {
            mapTransform.rotation = originalMapRotation;
        }

        isGimmickActive = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
