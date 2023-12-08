using UnityEngine;

public class LaserReceiver : MonoBehaviour
{
    public Transform receivedLaser;
    private Renderer _renderer;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private Color _color = Color.cyan;
    // public float _startTime = -1;
    // private float successTime = 5;

    private void Awake()
    {
        _renderer = transform.GetComponent<Renderer>();
    }

    private void Start()
    {
        _renderer.material.SetColor(EmissionColor, _color);
    }

    public void Activate(Transform receivedTransform, out float startTime)
    {
        receivedLaser = receivedTransform;

        if (!IsCorrect())
        {
            startTime = -1;
            return;
        }

        startTime = Time.time;
        _renderer.material.EnableKeyword("_EMISSION");
    }

    public void Deactivate(out float startTime)
    {
        startTime = -1;
        _renderer.material.DisableKeyword("_EMISSION");
    }

    private bool IsCorrect()
    {
        return receivedLaser.parent == transform.parent;
    }
}
