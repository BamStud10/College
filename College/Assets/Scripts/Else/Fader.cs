using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    [Header("페이드 설정")]
    [Tooltip("페이드 효과에 사용할 검은색 전체화면 이미지")]
    public Image fadeImage;

    [Tooltip("페이드 인/아웃에 걸리는 시간 (초)")]
    public float fadeDuration = 1.0f;

    private void Start()
    {
        // 인스펙터에서 이미지 연결을 깜빡했을 경우를 대비해 자식에서 찾아옵니다.
        if (fadeImage == null)
        {
            fadeImage = GetComponentInChildren<Image>();
        }

        // 시작할 때 화면이 까만 상태에서 밝아지게(페이드 인) 하려면 아래 주석을 해제하세요.
        /*
        Color startColor = fadeImage.color;
        startColor.a = 1f;
        fadeImage.color = startColor;
        FadeIn();
        */
    }

    /// <summary>
    /// 페이드 아웃: 화면이 점차 까맣게 어두워집니다. (알파값 0 -> 1)
    /// </summary>
    public void FadeOut()
    {
        // 혹시 진행 중인 다른 페이드 코루틴이 있다면 겹치지 않게 중지합니다.
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(1f));
    }

    /// <summary>
    /// 페이드 인: 화면이 점차 밝아지며 게임 화면이 보입니다. (알파값 1 -> 0)
    /// </summary>
    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(0f));
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        // 이미지가 꺼져있었다면 켭니다.
        fadeImage.gameObject.SetActive(true);

        Color fadeColor = fadeImage.color;
        float startAlpha = fadeColor.a;
        float time = 0f;

        // fadeDuration이 0일 경우 0으로 나누는 에러 방지
        if (fadeDuration <= 0f)
        {
            fadeColor.a = targetAlpha;
            fadeImage.color = fadeColor;
            yield break;
        }

        // 설정한 시간(fadeDuration)에 맞춰 알파값을 목표치까지 서서히 변경합니다.
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeColor.a = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            fadeImage.color = fadeColor;

            // 다음 프레임까지 대기
            yield return null;
        }

        // 루프가 끝난 뒤 목표 알파값으로 확실하게 고정 (오차 방지)
        fadeColor.a = targetAlpha;
        fadeImage.color = fadeColor;

        // 페이드 인이 끝나서 화면이 완전히 투명해졌다면 이미지를 끕니다.
        // (안 끄면 투명한 이미지가 화면을 덮고 있어서 "Press F" 등 다른 UI 클릭을 방해할 수 있습니다)
        if (targetAlpha == 0f)
        {
            fadeImage.gameObject.SetActive(false);
        }
    }
}
