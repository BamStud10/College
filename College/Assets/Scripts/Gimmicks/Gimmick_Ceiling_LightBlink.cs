using System.Collections;
using UnityEngine;

public class Gimmick_Ceiling_LightBlink : BasicGimmick
{
    [Header("기믹 식별 설정")]
    public Define.GimmickLV1 myGimmick = Define.GimmickLV1.lightBlink;

    [Header("조명 그룹 부모 오브젝트 (Ceiling_Lights 연결)")]
    public Transform lightsParent;

    [Header("전체 맵 밝기 제어 (Directional Light 연결)")]
    [Tooltip("형광등이 꺼질 때 맵의 그림자도 같이 없애기 위해 사용합니다.")]
    public Light mainDirectionalLight;

    [Header("깜빡임 제어 타겟 (원본 오브젝트 연결)")]
    [HideInInspector] public Light[] targetLights;
    [HideInInspector] public MeshRenderer[] fluorescentRenderers;

    [Header("매테리얼 설정")]
    [Tooltip("Mesh Renderer의 Materials 배열 중 전구(발광) 부분의 인덱스")]
    public int lightMaterialIndex = 1;

    [Header("깜빡임 주기 설정")]
    public float minOnTime = 0.2f;
    public float maxOnTime = 2.0f;
    public float minOffTime = 0.01f;
    public float maxOffTime = 0.1f;

    private Coroutine _flickerCoroutine;
    private bool _isGimmickActive = false;

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
        _isGimmickActive = true;
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
        _isGimmickActive = true; // 기믹 활성화 기록

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

        // 자신이 켜져 있던 상태에서만 태양광 및 전구를 켜서 원상복구합니다.
        if (_isGimmickActive)
        {
            SetLightsState(true);
            _isGimmickActive = false;
        }
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
        // 형광등이 켜지고 꺼질 때 Directional Light도 같이 제어하여 그림자를 지웁니다.
        if (mainDirectionalLight != null)
        {
            mainDirectionalLight.enabled = isOn;
        }

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
                    // 메쉬에 달린 모든 매테리얼을 확인합니다.
                    for (int i = 0; i < renderer.materials.Length; i++)
                    {
                        Material mat = renderer.materials[i];

                        // 지정한 인덱스(전구 부분)일 경우: 기믹 상태에 맞춰 깜빡임 제어
                        if (i == lightMaterialIndex)
                        {
                            if (isOn)
                            {
                                mat.EnableKeyword("_EMISSION");
                                mat.SetColor("_EmissionColor", Color.white * 2f);
                                mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                            }
                            else
                            {
                                mat.DisableKeyword("_EMISSION");
                                mat.SetColor("_EmissionColor", Color.black);
                                mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                            }
                        }
                        // 지정한 인덱스가 아닌 나머지(테두리 껍데기 등)일 경우: 무조건 발광을 까맣게 꺼버림
                        else
                        {
                            mat.DisableKeyword("_EMISSION");
                            mat.SetColor("_EmissionColor", Color.black);
                            mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                        }
                    }
                }
            }
        }
    }
}
