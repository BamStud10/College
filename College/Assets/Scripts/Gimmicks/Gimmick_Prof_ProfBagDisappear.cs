using UnityEngine;

/// <summary>
/// 교수님 가방 소멸 기믹 (독립 ID 적용)
/// </summary>
public class Gimmick_Prof_ProfBagDisappear : BasicGimmick
{
    [Header("기믹 설정")]
    // 새로 추가한 Enum 항목으로 변경 (예: profBagDisappear)
    public Define.GimmickLV3 myGimmick = Define.GimmickLV3.profBagDisappear;

    [Header("소멸시킬 가방 오브젝트")]
    [Tooltip("RightHand 손 뼈대 자식으로 들어가 있는 가방 오브젝트")]
    [SerializeField] private GameObject _bagObject;

    private void Awake()
    {
        // Enum의 정수 ID값(예: 108)을 BasicGimmick의 _code에 할당
        _code = (int)myGimmick;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    private void Start()
    {
        ResetGimmick();
    }

    /// <summary>
    /// GimmickManager로부터 본인 ID 신호를 받을 때 실행
    /// </summary>
    protected override void ExecuteGimmick()
    {
        Debug.Log($"[Gimmick_{_code}] 가방 소멸 기믹이 발동되었습니다.");

        if (_bagObject != null)
        {
            _bagObject.SetActive(false); // 가방 안 보이게 처리
        }
    }

    /// <summary>
    /// 기믹 리셋 신호 수신 시 원상복구
    /// </summary>
    protected override void ResetGimmick()
    {
        if (_bagObject != null)
        {
            _bagObject.SetActive(true); // 가방 다시 보이게 처리
        }
    }
}