using UnityEngine;

public class FriendlyNPC : MonoBehaviour
{
    [SerializeField] private SendCollideEvent sendCollideEvent;
    [SerializeField] private GameObject phrase;
    void Start()
    {
        sendCollideEvent.OnTriggerEnter += ShowPhrase;
        sendCollideEvent.OnTriggerExit += HidePhrase;
    }

    public void ShowPhrase(Collider2D player)
    {
        if(player.GetComponent<Player>())
            phrase.SetActive(true);
    }

    public void HidePhrase(Collider2D player)
    {
        if (player.GetComponent<Player>())
            phrase.SetActive(false);
    }

    private void OnDestroy()
    {
        sendCollideEvent.OnTriggerEnter -= ShowPhrase;
        sendCollideEvent.OnTriggerExit -= HidePhrase;
    }
}
