using UnityEngine;

public class LaserReceiver : MonoBehaviour
{
    public Transform ReceivedLaser{ get; private set; }
    private Renderer _renderer;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private readonly Color _color = Color.cyan;

    private void Awake()
    {
        _renderer = transform.GetComponent<Renderer>();
        ReceivedLaser = null;
    }

    private void Start()
    {
        _renderer.material.SetColor(EmissionColor, _color);
    }

    public void Activate(Transform receivedTransform, out float startTime)
    {
        ReceivedLaser = receivedTransform;

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
        return ReceivedLaser.parent == transform.parent;
    }
}