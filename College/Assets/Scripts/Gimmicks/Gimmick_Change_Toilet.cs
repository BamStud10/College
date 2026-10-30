using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 여러 개의 의자를 한번에 변기로 바꾸는 기믹 (ID: 105 / GimmickLV2.restPlaceChange)
/// - 변기 프리팹 원본의 Scale을 유지하며, 의자가 바라보던 회전값(Rotation)을 그대로 적용
/// </summary>
public class Gimmick_Change_Toilet : BasicGimmick
{
    [Header("기믹 설정")]
    public Define.GimmickLV2 myGimmick = Define.GimmickLV2.restPlaceChange;

    [Header("대상 의자들")]
    [Tooltip("변기로 바꿀 의자 게임오브젝트 목록")]
    [SerializeField] private List<GameObject> _chairList = new List<GameObject>();

    [Header("변기 프리팹")]
    [Tooltip("Project 창에 있는 변기 Prefab (프리팹 자체 스케일 사용)")]
    [SerializeField] private GameObject _toiletPrefab;

    // 생성된 변기 오브젝트들을 관리할 리스트
    private List<GameObject> _spawnedToilets = new List<GameObject>();

    private void Awake()
    {
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
    /// 105번 신호 수신 시: 의자를 숨기고, 의자의 위치와 회전값(Rotation)에 맞춰 변기 생성
    /// </summary>
    protected override void ExecuteGimmick()
    {
        Debug.Log($"[Gimmick_105] {_chairList.Count}개의 의자를 변기로 교체합니다.");

        ClearSpawnedToilets();

        foreach (GameObject chair in _chairList)
        {
            if (chair == null) continue;

            // 1. 기존 의자 비활성화
            chair.SetActive(false);

            // 2. 변기 생성
            // - 위치: chair.transform.position
            // - 회전: chair.transform.rotation (의자가 바라보던 방향 그대로 적용)
            // - 스케일: 변기 프리팹에 미리 설정해둔 기본 Scale 유지
            if (_toiletPrefab != null)
            {
                GameObject toilet = Instantiate(_toiletPrefab, chair.transform.position, chair.transform.rotation, transform);
                _spawnedToilets.Add(toilet);
            }
        }
    }

    /// <summary>
    /// 리셋 시: 생성된 변기들을 삭제하고 의자들을 다시 활성화
    /// </summary>
    protected override void ResetGimmick()
    {
        ClearSpawnedToilets();

        foreach (GameObject chair in _chairList)
        {
            if (chair != null)
            {
                chair.SetActive(true);
            }
        }
    }

    private void ClearSpawnedToilets()
    {
        foreach (GameObject toilet in _spawnedToilets)
        {
            if (toilet != null) Destroy(toilet);
        }
        _spawnedToilets.Clear();
    }
}