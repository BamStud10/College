using TMPro;
using UnityEngine;

public class Gimmick_Wall_PanelChange : BasicGimmick
{
    [Header("기믹 설정")]
    public Define.GimmickLV3 myGimmick = Define.GimmickLV3.panelChange;

    [Header("UI 텍스트 배열")]
    public TMP_Text[] texts = new TMP_Text[9];

    private void Awake()
    {
        _code = (int)myGimmick;
    }

    protected override void ExecuteGimmick()
    {
        // 배열에 있는 모든 텍스트를 순회하며 "666 호"로 변경합니다.
        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i] != null)
            {
                texts[i].text = "666 호";
            }
        }

        Debug.Log("[Gimmick] 이상현상 발동: 모든 호수가 666 호로 변경되었습니다.");
    }

    protected override void ResetGimmick()
    {
        // 별도의 초기화 코드가 필요 없습니다.
    }
}
