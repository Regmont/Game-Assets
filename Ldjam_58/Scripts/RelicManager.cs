using UnityEngine;
using System.Collections.Generic;

public class RelicManager : MonoBehaviour
{
    public static RelicManager Instance { get; private set; }

    public void RegisterRelic(Transform relic)
    {
        if (relic != null && !relics.Contains(relic))
        {
            relics.Add(relic);
        }
    }

    public void UnregisterRelic(Transform relic)
    {
        relics.Remove(relic);
    }

    public Vector3 GetDirectionToNearestRelic(Vector3 fromPosition)
    {
        if (relics.Count == 0)
        {
            return Vector3.zero;
        }

        Transform nearestRelic = FindNearestRelic(fromPosition);

        if (nearestRelic == null)
        {
            return Vector3.zero;
        }

        return (nearestRelic.position - fromPosition).normalized;
    }

    public int GetRelicCount()
    {
        return relics.Count;
    }

    public bool HasRelics()
    {
        return relics.Count > 0;
    }

    private List<Transform> relics = new List<Transform>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        FindAllRelics();
    }

    private void FindAllRelics()
    {
        GameObject[] relicObjects = GameObject.FindGameObjectsWithTag("Relic");

        foreach (GameObject relic in relicObjects)
        {
            RegisterRelic(relic.transform);
        }
    }

    private Transform FindNearestRelic(Vector3 fromPosition)
    {
        Transform nearestRelic = null;
        float nearestDistance = Mathf.Infinity;

        for (int i = relics.Count - 1; i >= 0; i--)
        {
            Transform relic = relics[i];

            if (relic == null)
            {
                relics.RemoveAt(i);
                continue;
            }

            float distance = Vector3.Distance(fromPosition, relic.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestRelic = relic;
            }
        }

        return nearestRelic;
    }
}