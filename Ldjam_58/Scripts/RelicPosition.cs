using UnityEngine;

public class Relic : MonoBehaviour
{
    void Start()
    {
        RelicManager.Instance.RegisterRelic(transform);
    }
    
    void OnDestroy()
    {
        if (RelicManager.Instance != null)
        {
            RelicManager.Instance.UnregisterRelic(transform);
        }
    }
}