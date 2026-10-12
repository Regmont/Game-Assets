using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    public float Force => _force;
    public bool IsHoldingBall => _isHoldingBall;
    public GameObject BallInHands => _ballInHands;
    public GameObject BallPhysical => _ballPhysical;

    public float MaxForce => _maxForce;

    [SerializeField]
    private GameObject _ballInHands;

    [SerializeField]
    private GameObject _ballPhysical;

    [SerializeField]
    private InputActionReference mouseClickAction;

    [SerializeField]
    private float _maxForce = 10f;

    [SerializeField]
    private float maxTime = 2f;

    private bool _isHoldingBall = true;
    private float _force = 0f;
    private float chargeTime = 0f;
    private bool isMousePressed = false;
    private float minForce = 1f;
    private Rigidbody _ballRb;

    private void Awake()
    {
        _ballRb = _ballPhysical.GetComponent<Rigidbody>();
    }


    public void ResetBallState()
    {
        _isHoldingBall = true;
        _force = minForce;
        chargeTime = 0f;
        isMousePressed = false;
    }

    private void Update()
    {
        if (isMousePressed && _isHoldingBall)
        {
            ChangeForce();
        }
    }

    private void OnEnable()
    {
        mouseClickAction.action.started += OnMousePressed;
        mouseClickAction.action.canceled += OnMouseRelease;
        mouseClickAction.action.Enable();
    }

    private void OnDisable()
    {
        mouseClickAction.action.started -= OnMousePressed;
        mouseClickAction.action.canceled -= OnMouseRelease;
        mouseClickAction.action.Disable();
    }

    private void OnMousePressed(InputAction.CallbackContext context)
    {
        isMousePressed = true;
    }

    private void OnMouseRelease(InputAction.CallbackContext context)
    {
        if (_isHoldingBall)
        {
            ThrowBall();
        }

        isMousePressed = false;
    }

    private void ChangeForce()
    {
        if (chargeTime < maxTime)
        {
            chargeTime += Time.deltaTime;
            _force = Mathf.Clamp(chargeTime / maxTime * _maxForce, minForce, _maxForce);
        }
    }

    private void ThrowBall()
    {
        _ballPhysical.transform.position = _ballInHands.transform.position;
        _ballInHands.gameObject.SetActive(false);
        _ballPhysical.gameObject.SetActive(true);
        _isHoldingBall = false;
        _ballRb.linearVelocity = Vector3.zero;
        _ballRb.angularVelocity = Vector3.zero;
        _ballPhysical.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * _force, ForceMode.Impulse);
    }
}
