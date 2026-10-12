using System.Collections;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public int State => state;

    [SerializeField] private GameObject player;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Terrain terrain;

    [SerializeField] private float stateOneDistance = 16f;
    [SerializeField] private float stateTwoDistance = 16f;
    [SerializeField] private float stateThreeDistance = 8.5f;
    [SerializeField] private float stateFourDistance = 4f;

    [SerializeField]
    [Range(0f, 0.1f)]
    private float stageTwoDisableProbability = 0.005f;

    [SerializeField] private float minScreenTimeStageOne = 5f;
    [SerializeField] private float minScreenTimeStageTwo = 1f;

    [SerializeField]
    private float monsterYOffset = 1f;

    [SerializeField] private float rotationAngleThirdState = 60f;

    [SerializeField]
    private float stageThreeDelay = 1f;

    [SerializeField] private float stageOneSpeed = 5f;
    [SerializeField] private float stageTwoSpeed = 0.1f;
    [SerializeField] private float stageThreeSpeed = 15f;
    [SerializeField] private float stageFourSpeed = 22f;

    [SerializeField] private float maxTimeOnScreen = 30f;
    [SerializeField] private float stateOneTimer = 0f;

    private Renderer objectRenderer;
    private Vector3 initialOffsetFromPlayer;
    private float maxRetreatDistance = 20f;

    private int state = 0;
    Vector3 runAwayDirection;

    private float screenTimeTimer = 0f;
    private bool minTimePassed = false;
    private bool delayCompleted = false;
    private bool isRunning = false;

    public void StateSpawn(int state)
    {
        if (player != null)
        {
            this.state = state;

            switch (state)
            {
                case 1:
                case 2:
                    FarSpawn();
                    break;
                case 3:
                case 4:
                    CloseSpawn();
                    break;
                default:
                    Debug.Log("Unknown state of monster!");
                    return;
            }

            initialOffsetFromPlayer = transform.position - player.transform.position;

            LookAtPlayer();
            EnableMonster();

            Debug.Log("Monster state: " + state);
        }
    }

    public void DisableMonster()
    {
        Debug.Log($"Monster off");

        gameObject.SetActive(false);
        state = 0;
        delayCompleted = false;
        isRunning = false;
        stateOneTimer = 0;
        minTimePassed = false;
    }

    private void FarSpawn()
    {
        Vector3 playerForward = player.transform.forward;
        float distance = state == 1 ? stateOneDistance : stateTwoDistance;
        Vector3 spawnPosition = player.transform.position + playerForward * distance;

        spawnPosition.y = GetTerrainHeightAtPosition(spawnPosition);

        transform.position = spawnPosition;
    }

    private void CloseSpawn()
    {
        Vector3 playerForward = player.transform.forward;
        float distance = state == 3 ? stateThreeDistance : stateFourDistance;
        Vector3 spawnPosition = player.transform.position + playerForward * distance;

        spawnPosition.y = GetTerrainHeightAtPosition(spawnPosition);

        transform.position = spawnPosition;

        float angle;
        if (state == 3)
        {
            angle = Random.Range(0, 2) == 0 ? rotationAngleThirdState : -rotationAngleThirdState;
        }
        else
        {
            angle = GetRandomRotationAroundPlayer();
            Debug.Log(angle);
        }

        transform.RotateAround(player.transform.position, Vector3.up, angle);
    }

    private float GetRandomRotationAroundPlayer()
    {
        return Random.Range(90f, 270f);
    }

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            switch(state)
            {
                case 1:
                    ProcessStateOneLogic();
                    break;
                case 2:
                    ProcessStateTwoLogic();
                    break;
                case 3:
                    ProcessStateThreeLogic();
                    break;
                case 4:
                    ProcessStateFourLogic();
                    break;
                default:
                    Debug.Log("Trying to execute unknown monster state logic");
                    break;
            }
        }
    }

    private void ProcessStateOneLogic()
    {
        stateOneTimer += Time.deltaTime;

        if (stateOneTimer < maxTimeOnScreen)
        {
            KeepDistanceFromPlayer();
        }
        else
        {
            Retreat();
        }
    }

    private void ProcessStateTwoLogic()
    {
        initialOffsetFromPlayer = Vector3.Lerp(initialOffsetFromPlayer, Vector3.zero, stageTwoSpeed * Time.deltaTime);
        transform.position = GetOffsetFromPlayer();

        LookAtPlayer();
        CheckMinScreenTime(minScreenTimeStageTwo);
        TryDisableMonster();
    }

    private void ProcessStateThreeLogic()
    {
        ProcessStateFourLogic();
    }

    private void ProcessStateFourLogic()
    {
        LookAtPlayer();

        if (delayCompleted)
        {
            float speed = state == 3 ? stageThreeSpeed : -stageFourSpeed;
            transform.position += speed * Time.deltaTime * runAwayDirection;
        }
        else if (IsVisibleByMainCamera())
        {
            runAwayDirection = (transform.position - player.transform.position).normalized;
            StartCoroutine(StartRunAwayAfterDelay());
        }
        else if (!isRunning)
        {
            transform.position = GetOffsetFromPlayer();
        }

        if (Vector3.Distance(transform.position, player.transform.position) >= maxRetreatDistance)
        {
            DisableMonster();
        }
    }

    private IEnumerator StartRunAwayAfterDelay()
    {
        isRunning = true;
        yield return new WaitForSeconds(stageThreeDelay);
        delayCompleted = true;
    }

    private bool IsVisibleByMainCamera()
    {
        if (mainCamera == null)
        {
            return false;
        }

        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(transform.position);

        return viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
               viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
               viewportPoint.z > 0;
    }

    private void EnableMonster()
    {
        gameObject.SetActive(true);
        screenTimeTimer = 0f;
        minTimePassed = false;
        Debug.Log("Monster on");
    }

    private void LookAtPlayer()
    {
        transform.eulerAngles = Vector3.zero;
        transform.LookAt(player.transform);
        transform.eulerAngles = new Vector3(-90f, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    private float GetTerrainHeightAtPosition(Vector3 position)
    {
        if (terrain != null && terrain.terrainData != null)
        {
            TerrainData terrainData = terrain.terrainData;
            Vector3 terrainPosition = terrain.transform.position;

            Vector3 localPos = position - terrainPosition;

            float normalizedX = localPos.x / terrainData.size.x;
            float normalizedZ = localPos.z / terrainData.size.z;

            if (normalizedX >= 0f && normalizedX <= 1f && normalizedZ >= 0f && normalizedZ <= 1f)
            {
                float height = terrainData.GetInterpolatedHeight(normalizedX, normalizedZ);
                return terrainPosition.y + height;
            }
        }

        return position.y + monsterYOffset;
    }

    private void KeepDistanceFromPlayer()
    {
        transform.position = GetOffsetFromPlayer();

        LookAtPlayer();
        CheckMinScreenTime(minScreenTimeStageOne);
        TryDisableMonster();
    }

    private void Retreat()
    {
        Vector3 runAwayDirection = (transform.position - player.transform.position).normalized;
        transform.position += stageOneSpeed * Time.deltaTime * runAwayDirection;

        LookAtPlayer();

        if (Vector3.Distance(transform.position, player.transform.position) >= maxRetreatDistance)
        {
            DisableMonster();
        }
    }

    private void CheckMinScreenTime(float minScreenTime)
    {
        if (objectRenderer.isVisible)
        {
            screenTimeTimer += Time.deltaTime;

            if (!minTimePassed && screenTimeTimer >= minScreenTime)
            {
                minTimePassed = true;
                Debug.Log("Minimum time on screen has passed! Monster can despawn now.");
            }
        }
    }

    private void TryDisableMonster()
    {
        if (!objectRenderer.isVisible && minTimePassed)
        {
            if (Random.value < stageTwoDisableProbability)
            {
                DisableMonster();
            }
        }
    }

    private Vector3 GetOffsetFromPlayer()
    {
        return new Vector3(
            player.transform.position.x + initialOffsetFromPlayer.x,
            transform.position.y,
            player.transform.position.z + initialOffsetFromPlayer.z
        );
    }
}