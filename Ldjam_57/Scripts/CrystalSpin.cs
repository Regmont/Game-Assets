using UnityEngine;

public class CrystalSpin : MonoBehaviour
{
    public float rotationSpeed = 100f;

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            string partName = "CPart_" + (i + 1);
            Transform partTransform = transform.Find(partName);

            if (partTransform != null)
            {
                Transform defaultTransform = partTransform.Find("default");
                if (defaultTransform != null)
                {
                    defaultTransform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
                }
            }
        }
    }
}