using UnityEngine;

public class Gimmick_Ceiling_DiscoBall : BasicGimmick
{
    [Header("기믹 식별 설정")]
    public Define.GimmickLV1 myGimmick = Define.GimmickLV1.discoBall;

    [Header("형광등 통째로 끄기 (Ceiling_Lights 연결)")]
    public GameObject lightsParent;

    [Header("전체 맵 밝기 제어 (Directional Light 연결)")]
    public Light mainDirectionalLight;

    [Header("디스코볼 설정 (DiscoBall_Rotator 연결)")]
    public GameObject discoBallRoot;
    public float rotationSpeed = 90f;

    private bool _isGimmickActive = false;

    private void Awake()
    {
        _code = (int)myGimmick;
        _isGimmickActive = true; // 초기화를 위해 임시 활성화

        // 시작 시 무조건 정상 상태로 세팅 (미러볼 끄기)
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
        Debug.Log("[Gimmick] 디스코볼 기믹 발동!");
        _isGimmickActive = true;

        // 1. 형광등 묶음 비활성화
        if (lightsParent != null) lightsParent.SetActive(false);

        // 2. 맵 전체 태양광 끄기 (복도를 어둡게)
        if (mainDirectionalLight != null) mainDirectionalLight.enabled = false;

        // 3. 디스코볼 나타나기
        if (discoBallRoot != null) discoBallRoot.SetActive(true);
    }

    protected override void ResetGimmick()
    {
        // 자기가 실행 중이었을 때만 태양광 및 형광등을 복구합니다.
        if (_isGimmickActive)
        {
            // 1. 형광등 묶음 다시 나타나게 하기
            if (lightsParent != null) lightsParent.SetActive(true);

            // 2. 맵 전체 태양광 다시 켜기
            if (mainDirectionalLight != null) mainDirectionalLight.enabled = true;

            // 3. 디스코볼 숨기기
            if (discoBallRoot != null) discoBallRoot.SetActive(false);

            _isGimmickActive = false;
        }
        else
        {
            // 게임 시작 시 다른 기믹에 영향 없이 디스코볼만 확실히 숨김
            if (discoBallRoot != null) discoBallRoot.SetActive(false);
        }
    }

    private void Update()
    {
        // 기믹이 켜져 있을 때 회전
        if (_isGimmickActive && discoBallRoot != null)
        {
            // 디스코볼 부모 껍데기 회전
            discoBallRoot.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}
