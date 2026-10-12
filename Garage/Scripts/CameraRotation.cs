using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 100f;

    private float yRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -80f, 80f);

        transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
    }
}