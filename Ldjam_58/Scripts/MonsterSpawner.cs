using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private MonsterController monsterController;

    [SerializeField]
    private float spawnTimer;

    [SerializeField]
    private float nextSpawnTime;

    [SerializeField]
    private float minSpawnTime = 50f;

    [SerializeField]
    private float maxSpawnTime = 70f;

    [SerializeField]
    private int difficulty = 0;

    public void IncreaseDifficulty()
    {
        difficulty++;
        minSpawnTime /= difficulty;
        maxSpawnTime /= difficulty;
    }

    private void Start()
    {
        SetRandomSpawnTime();
    }

    private void Update()
    {
        if (!monsterController.gameObject.activeSelf)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= nextSpawnTime)
            {
                SpawnMonster();
            }
        }
    }

    private void SpawnMonster()
    {
        float randomValue = Random.Range(0f, 100f);

        // Probabilities for 7 levels of difficulty (0-6)
        float[] stage1Probabilities = { 70f, 60f, 50f, 40f, 30f, 20f, 10f };
        float[] stage3Probabilities = { 25f, 30f, 35f, 35f, 35f, 35f, 30f };
        
        //state (1-4) / difficulty (0-6) -> %
        //      0   1   2   3   4   5   6
        //1-2   70  60  50  40  30  20  10
        //3     25  30  35  35  35  35  30
        //4     5   10  15  25  35  45  60

        float stage1Prob = stage1Probabilities[difficulty];
        float stage3Prob = stage1Prob + stage3Probabilities[difficulty];

        StateSpawn(randomValue, stage1Prob, stage3Prob);

        SetRandomSpawnTime();
    }

    private void StateSpawn(float randomValue, float stage1Prob, float stage3Prob)
    {
        if (randomValue <= stage1Prob)
        {
            if (Random.value < 0.5f)
            {
                monsterController.StateSpawn(1);
            }
            else
            {
                monsterController.StateSpawn(2);
            }
        }
        else if (randomValue < stage3Prob)
        {
            monsterController.StateSpawn(3);
        }
        else
        {
            monsterController.StateSpawn(4);
        }
    }

    private void SetRandomSpawnTime()
    {
        spawnTimer = 0f;
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }
}