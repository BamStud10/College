using System.Collections;
using UnityEngine;

public class Gimmick_Ceiling_FlickeringLight : MonoBehaviour
{
    int code;
    [Header("조명 및 메쉬 설정")]
    [Tooltip("깜빡이게 할 여러 개의 조명들을 여기에 넣으세요.")]
    public Light[] targetLights; // 단일 조명이 아닌 배열([])로 변경되었습니다.

    [Tooltip("형광등 껍데기(Cylinder)를 여기에 넣으세요.")]
    public MeshRenderer fluorescentRenderer; // Emission을 제어할 매터리얼 렌더러입니다.

    [Header("불규칙한 깜빡임 설정")]
    public float minOnTime = 0.3f;
    public float maxOnTime = 2.0f;
    public float minOffTime = 0.01f;
    public float maxOffTime = 0.1f;

    void Start()
    {
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            // 1. 불규칙한 시간 동안 켜둡니다.
            SetLightsState(true);
            float randomOnTime = Random.Range(minOnTime, maxOnTime);
            yield return new WaitForSeconds(randomOnTime);

            // 2. 아주 짧고 불규칙한 시간 동안 끕니다.
            SetLightsState(false);
            float randomOffTime = Random.Range(minOffTime, maxOffTime);
            yield return new WaitForSeconds(randomOffTime);
        }
    }

    // 모든 조명과 매터리얼 발광을 한 번에 제어하는 함수
    private void SetLightsState(bool isOn)
    {
        // 배열 안에 있는 4개의 조명을 반복문으로 모두 켜거나 끕니다.
        foreach (Light light in targetLights)
        {
            if (light != null)
            {
                light.enabled = isOn;
            }
        }

        // 형광등 메쉬의 발광(Emission) 효과를 켜거나 끕니다.
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
