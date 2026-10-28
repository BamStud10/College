using TMPro;
using UnityEngine;

public class PanelChange : MonoBehaviour
{
    [Header("호수 텍스트 배열 (인스펙터 할당)")]
    public TMP_Text[] texts = new TMP_Text[9];

    [Header("매니저 연결")]
    [Tooltip("@GameManager 오브젝트를 드래그해 넣으세요")]
    public GameManager gameManager;

    private void OnEnable()
    {
        // 1. 루프가 진행될 때마다 (루프 초기화 이벤트 발생 시) 텍스트 변경
        // 2. 이 이벤트는 GenerateRandomGimmick 보다 먼저 실행되므로 기믹 작동 전에 변경 완료됨
        GimmickManager.OnGimmickReset += UpdatePanelTexts;
    }

    private void OnDisable()
    {
        GimmickManager.OnGimmickReset -= UpdatePanelTexts;
    }

    private void Start()
    {
        // 게임 맨 처음 시작 시에도 1층 텍스트를 띄워주기 위해 한 번 실행
        UpdatePanelTexts();
    }

    private void UpdatePanelTexts()
    {
        // GameManager가 연결되어 있지 않다면 에러 방지
        if (gameManager == null)
        {
            Debug.LogWarning("[PanelChange] GameManager가 연결되지 않았습니다.");
            return;
        }

        // 3. GameManager의 CurrentStage를 층수로 읽어옵니다.
        int currentFloor = gameManager.CurrentStage;

        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i] != null)
            {
                // 층수 * 100 + 방 번호 (예: 1층이고 i가 0이면 100 + 1 = 101)
                int roomNumber = (currentFloor * 100) + (i + 1);

                texts[i].text = roomNumber.ToString() + "호";
            }
        }
    }

}
