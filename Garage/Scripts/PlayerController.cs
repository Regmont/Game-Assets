using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float force = 3f;

    [SerializeField]
    private float stoppingForce = 10f;

    private float forwardInput;
    private float sideInput;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        forwardInput = Input.GetAxis("Vertical");
        sideInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        Vector3 targetVelocity = (transform.forward * forwardInput + transform.right * sideInput) * force;

        if (targetVelocity.magnitude > 0.1f)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, force * Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, stoppingForce * Time.fixedDeltaTime);
        }
    }
}