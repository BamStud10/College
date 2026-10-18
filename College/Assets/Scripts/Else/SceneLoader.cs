using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    [Header("이동 설정")]
    [Tooltip("아무 키나 눌렀을 때 넘어갈 씬의 이름을 정확히 적어주세요.")]
    public string targetSceneName;

    private void Update()
    {
        // 마우스 클릭이나 키보드 아무 키나 누르는 것을 감지
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
