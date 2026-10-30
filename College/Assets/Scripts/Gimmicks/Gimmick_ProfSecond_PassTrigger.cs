using UnityEngine;

public class Gimmick_ProfSecond_PassTrigger : MonoBehaviour
{
    [SerializeField] private Gimmick_Prof_ProfSecond _mainGimmick;
    [SerializeField] private string _playerTag = "Player";

    // 1. Trigger 감지 테스트
    private void OnTriggerEnter(Collider other)
    {
        /*Debug.Log($"[PassTrigger] 무언가 통과함! 이름: {other.name} / 태그: {other.tag}");*/

        if (other.CompareTag(_playerTag))
        {
            /*Debug.Log("[PassTrigger] 플레이어 태그 일치 확인! 메인 기믹 호출");*/
            if (_mainGimmick != null)
            {
                _mainGimmick.OnPlayerPassedFirstProf();
            }
        }
    }

    // 2. 일반 Collision 감지 테스트 (혹시 Is Trigger 세팅 문제일 경우 대비)
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"[PassTrigger] Collision 부딪힘! 이름: {collision.gameObject.name}");
    }
}