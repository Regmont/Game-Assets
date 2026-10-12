using TMPro;
using UnityEngine;
using System.Collections;

public class UnblockBall : MonoBehaviour
{
    [SerializeField]
    private GameObject ballBlocker;

    [SerializeField]
    private float blockResetTime = 2f;

    private Coroutine resetCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            ballBlocker.GetComponent<BoxCollider>().isTrigger = true;

            if (resetCoroutine != null)
            {
                StopCoroutine(resetCoroutine);
            }

            resetCoroutine = StartCoroutine(ResetTriggerAfterTime());
        }
    }

    private IEnumerator ResetTriggerAfterTime()
    {
        yield return new WaitForSeconds(blockResetTime);

        ballBlocker.GetComponent<BoxCollider>().isTrigger = false;
    }
}