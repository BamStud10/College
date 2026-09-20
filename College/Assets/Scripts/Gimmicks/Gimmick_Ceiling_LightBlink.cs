using System.Collections;
using UnityEngine;

public class Gimmick_Ceiling_LightBlink : BasicGimmick
{
    [Header("기믹 식별 설정")]
    [Tooltip("팀 공용 Define에 정의된 본인의 1단계 기믹 항목을 선택하세요.")]
    public Define.GimmickLV1 myGimmick = Define.GimmickLV1.lightBlink; // 팀 enum에 추가된 깜빡임 항목(예: flickering 등) 선택

    [Header("조명 및 메쉬 타겟")]
    public Light[] targetLights;
    public MeshRenderer fluorescentRenderer;

    [Header("깜빡임 주기 설정")]
    public float minOnTime = 0.3f;
    public float maxOnTime = 2.0f;
    public float minOffTime = 0.01f;
    public float maxOffTime = 0.1f;

    private Coroutine _flickerCoroutine;

    private void Awake()
    {
        // 팀 규칙: Enum 값을 int로 변환해 부모 클래스의 _code에 할당
        _code = (int)myGimmick;
    }

    /// <summary>
    /// GimmickManager에서 본인 기믹 ID가 방송되었을 때 호출됨
    /// </summary>
    protected override void ExecuteGimmick()
    {
        Debug.Log("[Gimmick] 형광등 깜빡임 기믹 시작");

        // 이전 루틴이 돌고 있다면 중복 실행 방지를 위해 정지 후 재시작
        if (_flickerCoroutine != null)
        {
            StopCoroutine(_flickerCoroutine);
        }
        _flickerCoroutine = StartCoroutine(FlickerRoutine());
    }

    /// <summary>
    /// GimmickManager.OnGimmickReset 방송 시 호출됨 (정상 상태 복구)
    /// </summary>
    protected override void ResetGimmick()
    {
        Debug.Log("[Gimmick] 형광등 깜빡임 기믹 초기화");

        // 깜빡임 중지
        if (_flickerCoroutine != null)
        {
            StopCoroutine(_flickerCoroutine);
            _flickerCoroutine = null;
        }

        // 정상 상태(상시 켜짐)로 복귀
        SetLightsState(true);
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            // 1. 켜짐 유지
            SetLightsState(true);
            float randomOnTime = Random.Range(minOnTime, maxOnTime);
            yield return new WaitForSeconds(randomOnTime);

            // 2. 일시 소등
            SetLightsState(false);
            float randomOffTime = Random.Range(minOffTime, maxOffTime);
            yield return new WaitForSeconds(randomOffTime);
        }
    }

    private void SetLightsState(bool isOn)
    {
        if (targetLights != null)
        {
            foreach (Light light in targetLights)
            {
                if (light != null)
                {
                    light.enabled = isOn;
                }
            }
        }

        if (fluorescentRenderer != null)
        {
            if (isOn)
            {
                fluorescentRenderer.material.EnableKeyword("_EMISSION");
            }
            else
            {
                fluorescentRenderer.material.DisableKeyword("_EMISSION");
            }
        }
    }
}
