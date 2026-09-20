using System.Diagnostics;
using UnityEngine;

public class Gimmick_Test_Coder94 : BasicGimmick
{

    public Define.GimmickLV1 myGimmick = Define.GimmickLV1.drwoning;

    //Awake문 반드시 작성할 것
    private void Awake()
    {
        // 게임 시작 시 인스펙터에서 고른 Enum 값을 부모의 _code 변수에 int로 형변환해서 저장
        _code = (int)myGimmick;
    }

    protected override void ExecuteGimmick()
    {
        UnityEngine.Debug.Log("기믹 실행");
    }

    protected override void ResetGimmick()
    {
        UnityEngine.Debug.Log("기믹 초기화");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
