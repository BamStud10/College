using UnityEngine;

public class PLFootstep_Taesu : MonoBehaviour
{
    [Header("발소리 파일 설정")]
    public AudioClip defaultStepSound; // Floor_step0
    public AudioClip gimmickStepSound; // Floor_step1

    [Header("발소리 재생 간격")]
    public float stepInterval = 0.5f;
    private float _stepTimer;

    private AudioSource _audioSource;

    // 외부 기믹 스크립트에서 접근할 수 있는 기믹 활성화 상태 플래그
    public static bool isGimmickActive = false;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 0f; // 2D 사운드

        // 경로가 Resources 폴더 기준일 때 자동 로드
        if (defaultStepSound == null)
            defaultStepSound = Resources.Load<AudioClip>("Sound/Classic Footstep SFX/Floor/Floor_step0");

        if (gimmickStepSound == null)
            gimmickStepSound = Resources.Load<AudioClip>("Sound/Classic Footstep SFX/Floor/Floor_step7");
    }

    private void Update()
    {
        // 이동 입력 감지 (WASD)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        bool isMoving = (Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f);

        if (isMoving && _audioSource != null)
        {
            // 기믹 활성화 여부에 따라 클립 선택
            AudioClip targetClip = isGimmickActive ? gimmickStepSound : defaultStepSound;

            if (targetClip != null)
            {
                _stepTimer -= Time.deltaTime;
                if (_stepTimer <= 0f)
                {
                    _audioSource.PlayOneShot(targetClip);
                    _stepTimer = stepInterval;
                }
            }
        }
        else
        {
            _stepTimer = 0f;
        }
    }
}