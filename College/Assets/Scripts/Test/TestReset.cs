using UnityEngine;
using UnityEngine.SceneManagement;

public class TestReset : MonoBehaviour
{
    [Header("테스트할 기믹 넘버 기입")]
    public int _testNum;
    public void TestGen()
    {
        GimmickManager.OnGimmickTriggered?.Invoke(_testNum);
    }
    private void Update()
    {
        // Q키를 누르면 현재 활성화된 씬을 다시 로드합니다.
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TestGen();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 1. 빌드된 실제 게임(.exe)에서 프로그램 종료
            Application.Quit();

            // 2. 유니티 에디터에서 테스트 중일 때 플레이 모드 종료 (프로토타입 꿀팁)
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}
