using UnityEngine;
using UnityEngine.SceneManagement;

public class TestReset : MonoBehaviour
{
    private void Update()
    {
        // Q키를 누르면 현재 활성화된 씬을 다시 로드합니다.
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
