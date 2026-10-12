using UnityEngine;

public class Upgrade : MonoBehaviour
{
    bool isColliding = false;
    SubState subState;

    GameObject sub1;
    GameObject sub2;
    GameObject sub3;
    GameObject subBase;

    PlayerStats playerStats;

    private void Start()
    {
        subState = gameObject.GetComponent<SubState>();
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();

        sub1 = GameObject.Find("Sub1");
        sub2 = GameObject.Find("Sub2");
        sub3 = GameObject.Find("Sub3");
        subBase = GameObject.Find("Sub_base");
    }

    private void Update()
    {
        if (isColliding && Input.GetKeyDown(KeyCode.F))
        {
            switch (subState.state)
            {
                case 0:
                    if (playerStats.processedAlloy >= 3)
                    {
                        subState.state++;
                        Destroy(sub1);

                        Vector3 currentPosition = sub2.transform.position;
                        Vector3 newPosition = new Vector3(currentPosition.x, currentPosition.y + 50, currentPosition.z);
                        sub2.transform.position = newPosition;

                        playerStats.processedAlloy -= 3;
                    }
                    break;
                case 1:
                    if (playerStats.processedAlloy >= 5 && playerStats.powerCells >= 1)
                    {
                        subState.state++;

                        Vector3 currentPosition2 = subBase.transform.position;
                        Vector3 newPosition2 = new Vector3(currentPosition2.x, currentPosition2.y + 100, currentPosition2.z);
                        subBase.transform.position = newPosition2;

                        playerStats.processedAlloy -= 5;
                        playerStats.powerCells -= 1;
                    }
                    break;
                case 2:
                    if (playerStats.processedAlloy >= 1 && playerStats.bioGel >= 3 && playerStats.powerCells >= 2)
                    {
                        subState.state++;
                        Destroy(sub2);

                        Vector3 currentPosition3 = sub3.transform.position;
                        Vector3 newPosition3 = new Vector3(currentPosition3.x, currentPosition3.y + 100, currentPosition3.z);
                        sub3.transform.position = newPosition3;

                        playerStats.processedAlloy--;
                        playerStats.bioGel -= 3;
                        playerStats.powerCells -= 2;
                    }
                    break;
                case 3:
                    subState.state++;
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isColliding = false;
    }
}