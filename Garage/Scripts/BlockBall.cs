using UnityEngine;

public class BlockBall : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        GetComponent<BoxCollider>().isTrigger = false;
    }
}
