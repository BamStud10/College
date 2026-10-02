using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("상호작용 설정")]
    public float interactRange = 1f; // 상호작용 가능 거리 (1m)
    public Camera playerCamera;      // 1인칭 카메라 (Main Camera)
    public GameObject interactUI;    // 아까 만든 "Press F!" 텍스트 UI 오브젝트

    private LoopDoor _targetDoor;

    void Update()
    {
        CheckInteractable();

        // 목표물이 있고(문을 바라보는 중), F키를 눌렀을 때
        if (_targetDoor != null && Input.GetKeyDown(KeyCode.F))
        {
            interactUI.SetActive(false); // UI 숨기기
            _targetDoor.Interact();      // 문 열기 실행!
        }
    }

    private void CheckInteractable()
    {
        // 카메라 정중앙에서 앞으로 뻗어나가는 레이저(Ray) 생성
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        // 레이저가 물체에 닿았고, 그 거리가 interactRange 안쪽일 때
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            // 부딪힌 물체에 LoopDoor 스크립트가 있는지 확인
            LoopDoor door = hit.collider.GetComponent<LoopDoor>();

            if (door != null)
            {
                _targetDoor = door;
                interactUI.SetActive(true); // "Press F!" 켜기
                return;
            }
        }

        // 허공을 보거나 너무 멀리 있거나 문이 아닌 곳을 볼 때
        _targetDoor = null;
        if (interactUI != null) interactUI.SetActive(false);
    }
}
