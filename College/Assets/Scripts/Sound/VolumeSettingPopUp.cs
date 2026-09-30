using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettingPopUp : MonoBehaviour
{
    [Header("오디오 믹서")]
    public AudioMixer mainMixer;

    [Header("UI 슬라이더 연결")]
    public Slider wholeSlider;
    public Slider bgmSlider;
    public Slider seSlider;

    private void Start()
    {
        // 1. 저장된 볼륨값 불러오기 (저장된 값이 없으면 기본값 1.0f(최대) 적용)
        float savedWhole = PlayerPrefs.GetFloat("WholeVolume", 1f);
        float savedBGM = PlayerPrefs.GetFloat("BGMVolume", 1f);
        float savedSE = PlayerPrefs.GetFloat("SEVolume", 1f);

        // 2. 슬라이더 값에 적용 (이 코드가 실행되면서 아래 Set 함수들이 자동으로 한 번씩 호출됨)
        wholeSlider.value = savedWhole;
        bgmSlider.value = savedBGM;
        seSlider.value = savedSE;

        // 3. 슬라이더를 움직일 때마다 각각의 함수가 실행되도록 이벤트 리스너 연결
        wholeSlider.onValueChanged.AddListener(SetWholeVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        seSlider.onValueChanged.AddListener(SetSEVolume);
    }

    public void SetWholeVolume(float volume)
    {
        // 슬라이더 값(0~1)을 오디오 믹서의 데시벨(-80~0)로 변환 (볼륨이 0일 때의 에러 방지를 위해 최소값 0.0001f 보정)
        float db = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        mainMixer.SetFloat("WholeVolume", db);

        // 변경된 값을 PlayerPrefs에 저장
        PlayerPrefs.SetFloat("WholeVolume", volume);
    }

    public void SetBGMVolume(float volume)
    {
        float db = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        mainMixer.SetFloat("BGMVolume", db);
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSEVolume(float volume)
    {
        float db = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        mainMixer.SetFloat("SEVolume", db);
        PlayerPrefs.SetFloat("SEVolume", volume);
    }
}
