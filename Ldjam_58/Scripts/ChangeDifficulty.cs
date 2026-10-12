using UnityEngine;

public class ChangeDifficulty : MonoBehaviour
{
    private MonsterSpawner monsterSpawner;
    private bool isTaken = false;

    private void Awake()
    {
        monsterSpawner = GameObject.Find("Monster Spawner").GetComponent<MonsterSpawner>();
    }

    private void OnDisable()
    {
        if (!isTaken)
        {
            isTaken = true;
            monsterSpawner.IncreaseDifficulty();
        }
    }
}
