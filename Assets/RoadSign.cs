using UnityEngine;

public class RoadSign : MonoBehaviour
{
    [SerializeField] private SendCollideEvent targetSeeker;
    [SerializeField] private SendCollideEvent targetFollow;

    [SerializeField] private GameObject phrase;

    void Start()
    {
        phrase.SetActive(false);
        targetSeeker.OnTriggerEnter += ShowPhrase;
        targetFollow.OnTriggerExit += HidePhrase;
    }

    public void ShowPhrase(Collider2D player)
    {
        if (player.GetComponent<Player>())
            phrase.SetActive(true);
    }

    public void HidePhrase(Collider2D player)
    {
        if (player.GetComponent<Player>())
            phrase.SetActive(false);
    }

    private void OnDestroy()
    {
        targetSeeker.OnTriggerEnter -= ShowPhrase;
        targetFollow.OnTriggerExit -= HidePhrase;
    }
}
