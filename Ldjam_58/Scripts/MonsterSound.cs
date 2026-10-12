using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MonsterSoundController : MonoBehaviour
{
    [Header("Stage Sounds")]
    [SerializeField] private AudioClip stageOneSound;
    [SerializeField] private AudioClip stageThreeSound;
    [SerializeField] private AudioClip stageFourSound;
    
    [Header("Sound Settings")]
    [SerializeField] private float stageOneVolume = 0.2f;
    [SerializeField] private float stageThreeVolume = 0.4f;
    [SerializeField] private float stageFourVolume = 0.2f;
    
    private AudioSource audioSource;
    private MonsterController monsterController;
    private int currentState = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        monsterController = GetComponent<MonsterController>();
    }

    private void Update()
    {
        if (monsterController == null)
        {
            return;
        }
        
        if (monsterController.gameObject.activeSelf)
        {
            int newState = monsterController.State;
            
            if (newState != currentState)
            {
                currentState = newState;
                PlayStageSound(currentState);
            }
        }
        else if (currentState != 0)
        {
            currentState = 0;
            audioSource.Stop();
        }
    }

    private void PlayStageSound(int stage)
    {
        audioSource.Stop();
        
        if (stageOneSound == null || stageThreeSound == null || stageFourSound == null)
        {
            Debug.Log("Not all sound assigned!");
            return;
        }

        switch (stage)
        {
            case 1:
            case 2:
                audioSource.clip = stageOneSound;
                audioSource.volume = stageOneVolume;
                break;
            case 3:
                audioSource.clip = stageThreeSound;
                audioSource.volume = stageThreeVolume;
                break;
            case 4:
                audioSource.clip = stageFourSound;
                audioSource.volume = stageFourVolume;
                break;
        }

        audioSource.Play();
    }
}