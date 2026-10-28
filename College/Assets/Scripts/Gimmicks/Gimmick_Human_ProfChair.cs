using UnityEngine;

public class Gimmick_Human_ProfChair : BasicGimmick
{
    [Header("기믹 설정")]
    public Define.GimmickLV1 myGimmick = Define.GimmickLV1.profToChair;

    [Header("오브젝트 연결")]
    [Tooltip("평상시에 보일 교수 NPC 오브젝트")]
    public GameObject professorObj;

    [Tooltip("기믹 발동 시 보일 의자 오브젝트")]
    public GameObject chairObj;

    private void Awake()
    {
        // 부모 클래스(BasicGimmick)에서 기믹 번호를 식별할 수 있도록 _code 변수에 할당합니다.
        _code = (int)myGimmick;
    }

    protected override void ExecuteGimmick()
    {
        // 기믹 발동: 교수 NPC를 숨기고, 의자 오브젝트를 나타나게 합니다.
        if (professorObj != null) professorObj.SetActive(false);
        if (chairObj != null) chairObj.SetActive(true);

        Debug.Log("[Gimmick] 이상현상 발동: 교수가 의자로 변했습니다.");
    }

    protected override void ResetGimmick()
    {
        // 루프 초기화: 다시 정상 상태로 복구하기 위해 교수를 켜고 의자를 끕니다.
        if (professorObj != null) professorObj.SetActive(true);
        if (chairObj != null) chairObj.SetActive(false);
    }
}
