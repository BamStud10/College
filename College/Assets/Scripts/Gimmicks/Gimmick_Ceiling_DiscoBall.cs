using UnityEngine;

public class Gimmick_Ceiling_DiscoBall : BasicGimmick
{
    [Header("기믹 식별 설정")]
    public Define.GimmickLV1 myGimmick = Define.GimmickLV1.discoBall;

    [Header("오브젝트 세팅")]
    [Tooltip("평소에 켜져 있을 정상 형광등 오브젝트")]
    public GameObject normalLight;

    [Tooltip("실제 미러볼 메쉬와 조명이 들어있는 하위 오브젝트")]
    public GameObject actualMirrorBall; // 자식 오브젝트를 여기에 연결

    [Header("미러볼 회전 설정")]
    public float rotationSpeed = 90f;

    private bool _isGimmickActive = false;

    private void Awake()
    {
        _code = (int)myGimmick;

        // 깜빡하고 하위 오브젝트를 안 넣었을 경우, 자동으로 첫 번째 자식을 찾아주는 센스 있는 코드
        if (actualMirrorBall == null && transform.childCount > 0)
        {
            actualMirrorBall = transform.GetChild(0).gameObject;
        }

        // 시작 시 무조건 정상 상태로 세팅 (미러볼 끄기)
        ResetGimmick();
    }

    protected override void ExecuteGimmick()
    {
        Debug.Log("[Gimmick] 미러볼 기믹 발동!");

        // 1. 정상 조명 끄기
        if (normalLight != null) normalLight.SetActive(false);

        // 2. 내 하위 오브젝트(실제 미러볼) 켜기
        if (actualMirrorBall != null) actualMirrorBall.SetActive(true);

        _isGimmickActive = true;
    }

    protected override void ResetGimmick()
    {
        Debug.Log("[Gimmick] 미러볼 기믹 초기화");

        // 1. 정상 조명 다시 켜기
        if (normalLight != null) normalLight.SetActive(true);

        // 2. 내 하위 오브젝트(실제 미러볼) 숨기기
        if (actualMirrorBall != null) actualMirrorBall.SetActive(false);

        _isGimmickActive = false;
    }

    private void Update()
    {
        // 기믹이 켜져 있을 때만 실제 미러볼 오브젝트를 회전시킴
        if (_isGimmickActive && actualMirrorBall != null)
        {
            actualMirrorBall.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }
}
