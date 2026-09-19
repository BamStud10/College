using System;
using System.Collections.Generic;
using UnityEngine;

public class GimmickManager : MonoBehaviour
{
    #region variables
    [Header("진척도 관리")]
    [Range(1, 8)]
    [Tooltip("현재 스테이지 (1~8)")]
    public int currentStage = 1; //테스트용. 추후 게임매니저로 분리할거임

    [Header("확률 설정")]
    [Range(0, 100)]
    [Tooltip("이상 현상이 발생할 확률 (%)")]
    public int _anomalyChance = 70; // 기본값 70%

    // 약속: 기믹 ID 0은 '정상(기믹 없음)'을 의미
    public const int NORMAL_ID = 0;

    //기믹 트리거 액션
    public static Action<int> OnGimmickTriggered;
    // 기믹 초기화 액션
    public static Action OnGimmickReset;
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateRandomGimmick();
        }
    }

    public void GenerateRandomGimmick()
    {
        int selectedGimmickID = NORMAL_ID;

        // 1. 0 ~ 99 사이의 랜덤 숫자를 뽑아서 이상 현상 확률 체크
        int randomRoll = UnityEngine.Random.Range(0, 100);

        if (randomRoll < _anomalyChance)
        {
            // 이상 현상 당첨! (예: 70% 확률 성공)
            selectedGimmickID = GetRandomGimmickIDByStage();
            Debug.Log($"[GimmickManager] 스테이지 {currentStage} 진입. ⚠️이상 현상 발생! (ID: {selectedGimmickID})");
        }
        else
        {
            // 정상 상태 당첨!
            selectedGimmickID = NORMAL_ID;
            Debug.Log($"[GimmickManager] 스테이지 {currentStage} 진입. 🟢정상 상태입니다.");
        }

        // 2. 결정된 ID 방송 (0이 방송되면 기믹 스크립트들은 아무 동작도 안 하거나 원상복구 됨)
        OnGimmickTriggered?.Invoke(selectedGimmickID);
    }

    private int GetRandomGimmickIDByStage()
    {
        List<int> availableGimmicks = new List<int>();

        // 1단계 기믹
        availableGimmicks.AddRange((int[])Enum.GetValues(typeof(Define.GimmickLV1)));

        // 5~6 스테이지: 2단계 기믹 추가
        if (currentStage >= 5)
        {
            availableGimmicks.AddRange((int[])Enum.GetValues(typeof(Define.GimmickLV2)));
        }

        // 7~8 스테이지: 3단계 기믹 추가
        if (currentStage >= 7)
        {
            availableGimmicks.AddRange((int[])Enum.GetValues(typeof(Define.GimmickLV3)));
        }

        int randomIndex = UnityEngine.Random.Range(0, availableGimmicks.Count);
        return availableGimmicks[randomIndex];
    }
}