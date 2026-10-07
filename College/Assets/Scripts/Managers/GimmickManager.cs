using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GimmickManager : MonoBehaviour
{
    #region variables
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

    public void ResetAllGimmicks()
    {
        OnGimmickReset?.Invoke();
    }

    public int GenerateRandomGimmick(int currentStage)
    {
        int selectedGimmickID = NORMAL_ID;
        int randomRoll = UnityEngine.Random.Range(0, 100);
        if (randomRoll < _anomalyChance)
        {
            selectedGimmickID = GetRandomGimmickIDByStage(currentStage);
            Debug.Log($"[GimmickManager] 스테이지 {currentStage} 진입. ⚠️이상 현상 발생! (ID: {selectedGimmickID})");
        }
        else
        {
            selectedGimmickID = NORMAL_ID;
            Debug.Log($"[GimmickManager] 스테이지 {currentStage} 진입. 🟢정상 상태입니다.");
        }

        OnGimmickTriggered?.Invoke(selectedGimmickID);

        return selectedGimmickID; // GameManager가 이상현상 유무를 알 수 있게 리턴
    }

    private int GetRandomGimmickIDByStage(int currentStage)
    {
        List<int> availableGimmicks = new List<int>();

        availableGimmicks.AddRange((int[])Enum.GetValues(typeof(Define.GimmickLV1)));

        if (currentStage >= 5)
            availableGimmicks.AddRange((int[])Enum.GetValues(typeof(Define.GimmickLV2)));

        if (currentStage >= 7)
            availableGimmicks.AddRange((int[])Enum.GetValues(typeof(Define.GimmickLV3)));
        int randomIndex = UnityEngine.Random.Range(0, availableGimmicks.Count);

        return availableGimmicks[randomIndex];
    }
}