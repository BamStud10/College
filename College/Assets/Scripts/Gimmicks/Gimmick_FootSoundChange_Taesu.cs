using UnityEngine;

public class Gimmick_FootSound_Taesu : BasicGimmick
{
    public Define.GimmickLV2 myGimmick = Define.GimmickLV2.footSoundChange;

    [Header("Foot Sound Settings")]
    [Tooltip("발자국 소리를 재생하는 오디오 소스")]
    public AudioSource playerAudioSource;

    [Tooltip("기본 발자국 사운드 (기본값: Floor_step0)")]
    public AudioClip defaultFootstepSound;

    [Tooltip("기믹 발동 시 변경될 발자국 사운드 (기본값: Floor_step1)")]
    public AudioClip gimmickFootstepSound;

    private AudioClip originalFootstepSound;
    private bool isGimmickActive = false;
    private bool isInitialized = false;

    private void Awake()
    {
        TryInitialize();
    }

    protected override void OnEnable()
    {
        _code = (int)myGimmick;
        base.OnEnable();
        TryInitialize();
    }

    private void Start()
    {
        TryInitialize();
    }

    private void Reset()
    {
        AutoAssign();
    }

    private void OnValidate()
    {
        AutoAssign();
    }

    private void AutoAssign()
    {
        // 1. Player 태그를 가진 오브젝트에서 오디오 소스 자동 찾기
        if (playerAudioSource == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerAudioSource = player.GetComponent<AudioSource>();
            }
        }

        // 2. 프로젝트 리소스에서 Floor_step0, Floor_step1 사운드 클립 자동 로드 (지정되어 있지 않은 경우)
        if (defaultFootstepSound == null)
        {
            defaultFootstepSound = Resources.Load<AudioClip>("Sound/Classic Footstep SFX/Floor/Floor_step0");
        }

        if (gimmickFootstepSound == null)
        {
            gimmickFootstepSound = Resources.Load<AudioClip>("Sound/Classic Footstep SFX/Floor/Floor_step7");
        }
    }

    private void TryInitialize()
    {
        if (isInitialized) return;

        AutoAssign();

        if (playerAudioSource != null)
        {
            // 현재 오디오 소스의 클립을 백업하되, 기본값이 지정되어 있다면 초기 클립으로 설정
            if (defaultFootstepSound != null)
            {
                originalFootstepSound = defaultFootstepSound;
                playerAudioSource.clip = defaultFootstepSound;
            }
            else
            {
                originalFootstepSound = playerAudioSource.clip;
            }

            isInitialized = true;
        }
    }

    protected override void ExecuteGimmick()
    {
        if (isGimmickActive) return;

        TryInitialize();
        Debug.Log("기믹 실행");

        if (playerAudioSource != null && gimmickFootstepSound != null)
        {
            playerAudioSource.clip = gimmickFootstepSound;
        }

        // 발소리 스크립트에도 기믹 활성화 전달
        PLFootstep_Taesu.isGimmickActive = true;
        isGimmickActive = true;
    }

    protected override void ResetGimmick()
    {
        if (!isGimmickActive) return;

        Debug.Log("기믹 초기화");

        if (playerAudioSource != null)
        {
            playerAudioSource.clip = originalFootstepSound;
        }

        // 발소리 스크립트에도 기믹 해제 전달
        PLFootstep_Taesu.isGimmickActive = false;
        isGimmickActive = false;
    }
}