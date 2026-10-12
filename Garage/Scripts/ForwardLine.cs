using UnityEngine;

public class ForwardLine : MonoBehaviour
{
    [SerializeField] private float lineLength = 2f;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private float lineWidth = 0.05f;

    [SerializeField]
    private BallController ballController;

    private LineRenderer lineRenderer;
    private BallController playerController;

    private void Start()
    {
        InitializeLineRenderer();
        playerController = GameObject.Find("Player").GetComponent<BallController>();
    }

    private void InitializeLineRenderer()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        if (greenMaterial != null)
        {
            lineRenderer.material = greenMaterial;
        }
        else if (lineRenderer.material == null)
        {
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }
    }

    private void LateUpdate()
    {
        if (lineRenderer != null && Application.isPlaying)
        {
            lineRenderer.enabled = true;
            Vector3 objectPosition = transform.position;
            Vector3 forwardPoint = objectPosition + transform.forward * lineLength;

            lineRenderer.SetPosition(0, objectPosition);
            lineRenderer.SetPosition(1, forwardPoint);

            UpdateLineColor();
        }
    }

    private void UpdateLineColor()
    {
        if (playerController != null)
        {
            float force = playerController.Force;
            float normalizedForce = Mathf.Clamp01(force / ballController.MaxForce);

            Color newColor = Color.Lerp(Color.green, Color.red, normalizedForce);

            lineRenderer.material.color = newColor;
        }
    }
}