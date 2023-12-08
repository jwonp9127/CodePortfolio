# Sample Code

## [Leaf](https://github.com/jwonp9127/CodePortfolio/blob/main/TSEROF/Gimmick/Leaf.cs)

```csharp
public class Leaf : MonoBehaviour
{
    private Player _player;
    private bool _playerIsOnRide;
    private GameObject _interactionIcon;
    private Transform _playerTransform;
    
    private void Awake()
    {
        _interactionIcon = transform.GetChild(2).gameObject;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        _interactionIcon.SetActive(true);
        if (_playerIsOnRide)
        {
            return;
        }
        
        _player = other.GetComponent<Player>();
        
        if (_player == null)
        {
            return;
        }
        
        _player.Input.PlayerActions.Interaction.started += CheckOnRideStarted;
    }

    private void OnTriggerExit(Collider other)
    {
        _interactionIcon.SetActive(false);
        if (_playerIsOnRide)
        {
            return;
        }
        
        _player = other.GetComponent<Player>();
        
        if (_player == null)
        {
            return;
        }
        
        _player.Input.PlayerActions.Interaction.started -= CheckOnRideStarted;
    }
    
    private void CheckOnRideStarted(InputAction.CallbackContext obj)
    {
        if (_playerIsOnRide)
        {
            return;
        }
        SetPlayerOnRide();
    }
    
    private void SetPlayerOnRide()
    {
        if (_playerTransform == null)
        {
            return;
        }

        // 불필요한 중복 호출을 방지하여 캐싱
        _playerIsOnRide = true;
        _playerTransform = _player.transform;
        _playerTransform.position = transform.position;
        _playerTransform.rotation = transform.rotation;
        _player.movementSpeed *= 1.7f;
        Stage2Manager.instance.isStage2Clear = true;
        transform.SetParent(_playerTransform);
    }
}
```

## [TransparentObject](https://github.com/jwonp9127/CodePortfolio/blob/main/TSEROF/Gimmick/TransparentObject.cs)

```csharp
public class TransparentObject : MonoBehaviour
{
    // 가비지를 최소화 하기 위해 const로 지정
    private const string PlayerTag = "Player";

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag(PlayerTag))
        {
            return;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    
    private void OnCollisionExit(Collision other)
    {
        if (!other.gameObject.CompareTag(PlayerTag))
        {
            return;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
```

## [LaserPatternManager](https://github.com/jwonp9127/CodePortfolio/blob/main/TSEROF/Gimmick/LaserPatternManager.cs)

```csharp
public class LaserPatternManager : MonoBehaviour
{
    private const int LaserCount = 6;
    private const int LaserPairs = 2;
    
    private readonly int[,] _randomPair = new int[LaserCount, LaserPairs] { { 3, 10 }, { 0, 5 }, { 2, 7 }, { 4, 9 }, { 6, 11 }, { 1, 8 } };
    private int[] _selectedLaser = new int[LaserCount];
    private Transform[] _laserTransform = new Transform[LaserCount];
    public GameObject stage2ClearCutScene;
    public int successCount;
    private bool _isCleared;
    private readonly Random _random = new Random();

    private void Update()
    {
        if (_isCleared)
        {
            return;
        }

        if (successCount == 6)
        {
            Clear();
        }
    }

    private void Start()
    {
        stage2ClearCutScene.SetActive(false);
        GetLaserPair();
        RandomSelect();
        LaserPatternSetting();
    }

    private void GetLaserPair()
    {
        for (int i = 0; i < 6; i++)
        {
            _laserTransform[i] = transform.GetChild(i);
            _laserTransform[i].gameObject.SetActive(true);
        }
    }

    private void RandomSelect()
    {
        for (int i = 0; i < 6; i++)
        {
            _selectedLaser[i] = _randomPair[i, _random.Next(LaserPairs)];
        }
    }
    
    private void LaserPatternSetting()
    {
        foreach(int laserNum in _selectedLaser)
        {
            int selectedLaser = laserNum == 0 ? 0 : laserNum / 2;
            int selectedTransmitter = laserNum % 2;
            _laserTransform[selectedLaser].GetChild(selectedTransmitter + 1).gameObject.SetActive(false);
        }
    }

    private void Clear()
    {
        for (int i = 0; i < 6; i++)
        {
            _laserTransform[i].gameObject.SetActive(false);
        }
        stage2ClearCutScene.SetActive(true);
        _isCleared = true;
    }
}

```

## [LaserReceiver](https://github.com/jwonp9127/CodePortfolio/blob/main/TSEROF/Gimmick/LaserReceiver.cs)

```csharp
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
```



