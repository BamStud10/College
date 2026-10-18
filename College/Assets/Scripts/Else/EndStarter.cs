using UnityEngine;

public class EndStarter : MonoBehaviour
{
    [Header("UI 연결")]
    [Tooltip("페이드 연출을 담당하는 스크립트")]
    public Fader _fader;

    void Start()
    {
        _fader.FadeIn();
    }

}
