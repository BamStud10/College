using System.Collections;
using UnityEngine;

public class Gimmick_Ceiling_LightBlink : BasicGimmick
{
    [Header("기믹 식별 설정")]
    public Define.GimmickLV1 myGimmick = Define.GimmickLV1.lightBlink;

    [Header("조명 그룹 부모 오브젝트 (Ceiling_Lights 연결)")]
    public Transform lightsParent;

    [Header("깜빡임 제어 타겟 (원본 오브젝트 연결)")]
    [HideInInspector] public Light[] targetLights;
    [HideInInspector] public MeshRenderer[] fluorescentRenderers;

    [Header("깜빡임 주기 설정")]
    public float minOnTime = 0.2f;
    public float maxOnTime = 2.0f;
    public float minOffTime = 0.01f;
    public float maxOffTime = 0.1f;

    private Coroutine _flickerCoroutine;

    private void Awake()
    {
        _code = (int)myGimmick;
        if (lightsParent != null)
        {
            // 새 맵에는 Light가 없으므로 배열이 비어있게 되지만 에러는 나지 않습니다.
            targetLights = lightsParent.GetComponentsInChildren<Light>();

            // 17개의 형광등 메쉬를 모두 찾아 배열에 저장합니다.
            fluorescentRenderers = lightsParent.GetComponentsInChildren<MeshRenderer>();
        }
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
        // 1. 모든 실제 조명 제어
        if (targetLights != null)
        {
            foreach (Light light in targetLights)
            {
                if (light != null) light.enabled = isOn;
            }
        }

        // 2. 모든 형광등 메쉬의 발광(Emission) 제어
        if (fluorescentRenderers != null)
        {
            foreach (MeshRenderer renderer in fluorescentRenderers)
            {
                if (renderer != null)
                {
                    // 형광등 하나에 붙어있는 다중 매테리얼(Light, Cover)을 모두 껐다 켭니다.
                    foreach (Material mat in renderer.materials)
                    {
                        if (isOn)
                        {
                            mat.EnableKeyword("_EMISSION");
                            mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                        }
                        else
                        {
                            mat.DisableKeyword("_EMISSION");
                            mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                        }
                    }
                }
            }
        }
    }
}
