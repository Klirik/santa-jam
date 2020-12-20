using UnityEngine;

public class UIMenu : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Boss boss;

    [SerializeField] private GameObject TutorialPanel;
    [SerializeField] private GameObject ResetPanel;
    [SerializeField] private GameObject WinPanel;

    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        TutorialPanel.SetActive(true);
        boss.OnDie += ShowWinPanel;
        player.OnDie += ShowReset;
        audioSource.volume = 0;
    }

    private void Update()
    {
        if(audioSource.volume <= 0.5f)
        {
            audioSource.volume += 0.01f;
        }
    }

    private void ShowWinPanel()
    {
        audioSource.Stop();
        WinPanel.SetActive(true);
    }

    private void ShowReset()
    {
        ResetPanel.SetActive(true);
    }

    private void OnDestroy()
    {
        boss.OnDie -= ShowWinPanel;
        player.OnDie -= ShowReset;
    }

}
