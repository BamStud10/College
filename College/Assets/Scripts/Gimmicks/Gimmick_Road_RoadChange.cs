using UnityEngine;

public class Gimmick_Road_RoadChange : BasicGimmick
{
    [Header("기믹 설정")]
    [Tooltip("Define.cs에 정의된 기믹 Enum")]
    public Define.GimmickLV1 myGimmick = Define.GimmickLV1.roadChange;

    [Header("바닥 오브젝트 교체")]
    [Tooltip("평소에 켜져 있는 기본 복도 바닥 오브젝트 (Plane)")]
    [SerializeField] private GameObject _normalFloorObject;

    [Tooltip("기믹 발생 시 켜질 흙길/오솔길 바닥 오브젝트 (Floor_DirtRoad)")]
    [SerializeField] private GameObject _dirtRoadObject;


    private void Awake()
    {
        _code = (int)myGimmick;

        // 게임 시작 시 초기 상태 보장
        ResetGimmick();
    }

    /// <summary>
    /// GimmickManager가 roadChange(ID: 2)를 방송했을 때 호출
    /// </summary>
    protected override void ExecuteGimmick()
    {
        Debug.Log($"[Gimmick_Road_RoadChange] 기믹 활성화: 복도 바닥이 오솔길/흙길로 변경됩니다. (ID: {_code})");

        // 오브젝트 스위칭
        if (_normalFloorObject != null) _normalFloorObject.SetActive(false);
        if (_dirtRoadObject != null) _dirtRoadObject.SetActive(true);

    }

    /// <summary>
    /// 다음 루프로 넘어가거나 초기화될 때 원래 복도 상태로 복구
    /// </summary>
    protected override void ResetGimmick()
    {
        // 복구: 기본 바닥 ON, 흙길 바닥 OFF
        if (_normalFloorObject != null) _normalFloorObject.SetActive(true);
        if (_dirtRoadObject != null) _dirtRoadObject.SetActive(false);
    }
}