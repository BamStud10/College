using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

public class ProfMover : MonoBehaviour
{
    private CinemachineSplineCart _cart;
    private bool _isFinished = false;

    private void Awake()
    {
        // 카트에 붙어있는 SplineAnimate 컴포넌트를 가져옵니다.
        _cart = GetComponent<CinemachineSplineCart>();
    }

    private void OnEnable()
    {
        // 루프 초기화 이벤트에 카트 복귀 함수를 등록합니다.
        GimmickManager.OnGimmickReset += ResetCart;
    }

    private void OnDisable()
    {
        GimmickManager.OnGimmickReset -= ResetCart;
    }

    private void Update()
    {
        // 1. 스플라인의 끝(진행도 1.0)에 도달했는지 확인
        if (!_isFinished && _cart != null && _cart.SplinePosition >= 1f)
        {
            _isFinished = true;
            // 2. 카트의 '자식 오브젝트(교수, 의자 등)'를 모두 숨깁니다.
            // (카트 자체를 끄면 스크립트가 멈추므로 자식들인 모델링만 끕니다)
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private void ResetCart()
    {
        if (_cart != null)
        {
            _isFinished = false;

            // 진행도를 0으로 되돌려 처음 위치로 보냅니다.
            _cart.SplinePosition = 0f;
        }
    }
}
