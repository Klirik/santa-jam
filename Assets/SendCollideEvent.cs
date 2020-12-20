using System;
using UnityEngine;

public class SendCollideEvent : MonoBehaviour
{    
    public event Action<Collider2D> OnTriggerEnter = delegate { };
    public event Action<Collider2D> OnTriggerExit = delegate { };
        
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter?.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerExit?.Invoke(collision);
    }
}
