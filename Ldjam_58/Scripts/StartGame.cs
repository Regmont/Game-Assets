using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    private GameObject relics;
    
    [SerializeField]
    private GameObject monsterSpawner;

    [SerializeField]
    private GameObject hands;

    [SerializeField]
    private GameObject compass;

    [SerializeField]
    private List<MonoBehaviour> playerControllers;

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private GameObject startButton;

    [SerializeField]
    private GameObject BG;

    [SerializeField]
    private float cameraMoveDuration = 3f;

    private bool isCameraMoving = false;

    public void ActivateObjects()
    {
        if (isCameraMoving) return;

        startButton.SetActive(false);
        BG.SetActive(false);
        compass.SetActive(true);


        StartCoroutine(MoveCameraThenActivate());
    }

    private IEnumerator MoveCameraThenActivate()
    {
        isCameraMoving = true;

        Vector3 initialLocalPosition = mainCamera.transform.localPosition;
        Quaternion initialLocalRotation = mainCamera.transform.localRotation;

        float elapsedTime = 0f;

        while (elapsedTime < cameraMoveDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / cameraMoveDuration;
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

            mainCamera.transform.localPosition = Vector3.Lerp(
                initialLocalPosition,
                Vector3.zero,
                smoothProgress
            );

            mainCamera.transform.localRotation = Quaternion.Lerp(
                initialLocalRotation,
                Quaternion.identity,
                smoothProgress
            );

            yield return null;
        }

        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.identity;

        ActivateGameObjects();

        isCameraMoving = false;
    }

    private void ActivateGameObjects()
    {
        if (monsterSpawner != null)
            monsterSpawner.SetActive(true);

        if (hands != null)
            hands.SetActive(true);

        if (playerControllers != null)
        {
            foreach (MonoBehaviour controller in playerControllers)
            {
                controller.enabled = true;
            }
        }
        
        if (relics != null)
            relics.SetActive(true);
    }
}