using UnityEngine;

public class ShowText : MonoBehaviour
{
    void Update()
    {
        if (GameObject.Find("Player") != null)
        {
            Vector3 lookDirection = GameObject.Find("Player").transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
            transform.rotation *= Quaternion.Euler(0, 180, 0);
        }
    }
}