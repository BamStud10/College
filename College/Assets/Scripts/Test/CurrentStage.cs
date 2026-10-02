using TMPro;
using UnityEngine;

public class CurrentStage : MonoBehaviour
{
    [Header("매니저 연결")]
    public GameManager gameManager;

    private TextMeshProUGUI _stageText;

    private void Start()
    {
        // 내 오브젝트에 붙어있는 TextMeshProUGUI 컴포넌트를 자동으로 가져옵니다.
        _stageText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        // 프로토타입용: 매 프레임마다 GameManager의 CurrentStage를 확인해서 텍스트를 바꿈
        if (gameManager != null && _stageText != null)
        {
            _stageText.text = $"현재 스테이지: {gameManager.CurrentStage}";
        }
    }
}
