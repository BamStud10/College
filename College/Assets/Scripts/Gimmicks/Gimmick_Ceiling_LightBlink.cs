using System.Collections;
using UnityEngine;

public class Gimmick_Ceiling_LightBlink : BasicGimmick
{
    [Header("기믹 식별 설정")]
    public Define.GimmickLV1 myGimmick = Define.GimmickLV1.lightBlink;

    [Header("깜빡임 제어 타겟 (원본 오브젝트 연결)")]
    public Light[] targetLights;
    public MeshRenderer fluorescentRenderer;

    [Header("깜빡임 주기 설정")]
    public float minOnTime = 0.2f;
    public float maxOnTime = 2.0f;
    public float minOffTime = 0.01f;
    public float maxOffTime = 0.1f;

    private Coroutine _flickerCoroutine;

    private void Awake()
    {
        _code = (int)myGimmick;
        ResetGimmick(); // 시작 시 정상 켜짐 상태 보장
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GimmickManager.OnGimmickTriggered += CheckAndReset;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GimmickManager.OnGimmickTriggered -= CheckAndReset;
    }

    // 다른 기믹이 뽑히면 자신을 원상복구(초기화)
    private void CheckAndReset(int triggeredID)
    {
        if (triggeredID != _code)
        {
            ResetGimmick();
        }
    }

    protected override void ExecuteGimmick()
    {
        Debug.Log("[Gimmick] 원본 형광등 깜빡임 시작!");

        if (_flickerCoroutine != null) StopCoroutine(_flickerCoroutine);
        _flickerCoroutine = StartCoroutine(FlickerRoutine());
    }

    protected override void ResetGimmick()
    {
        if (_flickerCoroutine != null)
        {
            StopCoroutine(_flickerCoroutine);
            _flickerCoroutine = null;
        }

        // 깜빡임을 멈추고 원본 조명을 '항상 켜진 상태'로 복구
        SetLightsState(true);
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            SetLightsState(true);
            yield return new WaitForSeconds(Random.Range(minOnTime, maxOnTime));

            SetLightsState(false);
            yield return new WaitForSeconds(Random.Range(minOffTime, maxOffTime));
        }
    }

    private void SetLightsState(bool isOn)
    {
        if (targetLights != null)
        {
            foreach (Light light in targetLights)
            {
                if (light != null) light.enabled = isOn;
            }
        }

        if (fluorescentRenderer != null)
        {
            if (isOn) fluorescentRenderer.material.EnableKeyword("_EMISSION");
            else fluorescentRenderer.material.DisableKeyword("_EMISSION");
        }
    }
}
