using UnityEngine;

public class Gimmick_Ceiling_DiscoBall : BasicGimmick
{
    [Header("기믹 식별 설정")]
    public Define.GimmickLV1 myGimmick = Define.GimmickLV1.discoBall;

    [Header("조명 제어 (Ceiling_Lights 연결)")]
    public Transform lightsParent;
    private MeshRenderer[] normalLights;

    [Header("전체 맵 밝기 제어")]
    [Tooltip("하이어라키 최상단에 있는 Directional Light를 연결하세요")]
    public Light mainDirectionalLight;

    [Header("디스코볼 설정 (Hall/DiscoBall 연결)")]
    public GameObject discoBallObject;
    public float rotationSpeed = 90f;

    private bool _isGimmickActive = false;

    private void Awake()
    {
        _code = (int)myGimmick;

        // 부모 오브젝트가 연결되어 있으면 17개의 형광등 메쉬를 미리 찾아둡니다.
        if (lightsParent != null)
        {
            normalLights = lightsParent.GetComponentsInChildren<MeshRenderer>();
        }

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

        // 형광등 및 태양광 끄기
        SetNormalLightsEmission(false);
        if (mainDirectionalLight != null) mainDirectionalLight.enabled = false;

        if (discoBallObject != null) discoBallObject.SetActive(true);
    }

    protected override void ResetGimmick()
    {
        _isGimmickActive = false;

        // 형광등 및 태양광 다시 켜기
        SetNormalLightsEmission(true);
        if (mainDirectionalLight != null) mainDirectionalLight.enabled = true;

        if (discoBallObject != null) discoBallObject.SetActive(false);
    }

    private void Update()
    {
        // 기믹이 켜져 있을 때 회전
        if (_isGimmickActive && discoBallObject != null)
        {
            // Space.World를 추가하여 지지대가 휘둘러지지 않고 제자리에서 돌게 만듭니다.
            discoBallObject.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    // 형광등 17개의 발광(Emission)을 일괄 제어하는 함수
    private void SetNormalLightsEmission(bool isOn)
    {
        if (normalLights == null) return;

        foreach (MeshRenderer renderer in normalLights)
        {
            if (renderer != null)
            {
                foreach (Material mat in renderer.materials)
                {
                    if (isOn)
                    {
                        mat.EnableKeyword("_EMISSION");
                        // 원래의 하얀색 발광으로 복구 (숫자를 높이면 더 밝아집니다)
                        mat.SetColor("_EmissionColor", Color.white * 2f);
                    }
                    else
                    {
                        mat.DisableKeyword("_EMISSION");
                        // 강제로 검은색을 적용해 형광등을 완벽하게 끔
                        mat.SetColor("_EmissionColor", Color.black);
                    }
                }
            }
        }
    }
}
