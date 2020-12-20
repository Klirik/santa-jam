using System;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public event Action OnDie = delegate { };
    
    private void OnDestroy()
    {
        OnDie?.Invoke();
    }
}
