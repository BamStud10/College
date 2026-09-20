using UnityEngine;

public abstract class BasicGimmick : MonoBehaviour
{
    // 자식들이 공통으로 사용할 기믹 번호 변수 (내부 변수 규칙 _ 적용)
    protected int _code;

    // 자식 클래스는 굳이 이 함수를 오버라이드할 필요 없이 _code 값만 설정
    protected virtual int GetGimmickID()
    {
        return _code;
    }

    protected virtual void OnEnable()
    {
        GimmickManager.OnGimmickTriggered += CheckAndExecute;
        GimmickManager.OnGimmickReset += ResetGimmick;
    }

    protected virtual void OnDisable()
    {
        GimmickManager.OnGimmickTriggered -= CheckAndExecute;
        GimmickManager.OnGimmickReset -= ResetGimmick;
    }

    private void CheckAndExecute(int triggeredID)
    {
        if (triggeredID == GetGimmickID())
        {
            ExecuteGimmick();
        }
    }

    protected abstract void ExecuteGimmick();
    protected abstract void ResetGimmick();
}