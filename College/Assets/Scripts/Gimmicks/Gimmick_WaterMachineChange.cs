using UnityEngine;

public class Gimmick_WaterMachineChange : BasicGimmick
{
    [Header("기믹 식별 설정")]
    public Define.GimmickLV3 myGimmick = Define.GimmickLV3.waterMachineChange;

    [Header("정수기 배열 세팅 (인덱스를 맞춰서 넣어주세요)")]
    [Tooltip("맵에 배치된 정상 정수기들을 넣으세요 (예: Element 0 = 앞쪽 정상, Element 1 = 뒤쪽 정상)")]
    public GameObject[] normalPurifiers;

    [Tooltip("맵에 배치된 기믹 정수기들을 넣으세요 (예: Element 0 = 앞쪽 기믹, Element 1 = 뒤쪽 기믹)")]
    public GameObject[] trickPurifiers;

    private void Awake()
    {
        _code = (int)myGimmick;
        ResetGimmick();
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

    private void CheckAndReset(int triggeredID)
    {
        if (triggeredID != _code)
        {
            ResetGimmick();
        }
    }

    protected override void ExecuteGimmick()
    {
        Debug.Log("[Gimmick] 정수기 온수 2개 기믹 발동!");

        // 두 정수기 중 어느 것을 기믹으로 만들지 랜덤하게 선택 (0 또는 1)
        int randomIndex = Random.Range(0, normalPurifiers.Length);

        for (int i = 0; i < normalPurifiers.Length; i++)
        {
            if (i == randomIndex)
            {
                // 당첨된 인덱스: 정상 정수기를 끄고 기믹 정수기를 켬
                if (normalPurifiers[i] != null) normalPurifiers[i].SetActive(false);
                if (trickPurifiers[i] != null) trickPurifiers[i].SetActive(true);
            }
            else
            {
                // 당첨되지 않은 인덱스: 정상 상태 유지
                if (normalPurifiers[i] != null) normalPurifiers[i].SetActive(true);
                if (trickPurifiers[i] != null) trickPurifiers[i].SetActive(false);
            }
        }
    }

    protected override void ResetGimmick()
    {
        // 기믹 미발동 시: 모든 정수기를 정상 상태로 초기화
        for (int i = 0; i < normalPurifiers.Length; i++)
        {
            if (normalPurifiers[i] != null) normalPurifiers[i].SetActive(true);
            if (trickPurifiers[i] != null) trickPurifiers[i].SetActive(false);
        }
    }
}
