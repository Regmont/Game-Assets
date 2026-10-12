using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 100f;

    private float xRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        xRotation = 180f;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        xRotation += mouseX;
        transform.rotation = Quaternion.Euler(0f, xRotation, 0f);
    }
}