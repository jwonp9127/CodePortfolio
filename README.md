# Sample Code

|프로젝트|샘플 코드|
|:---:|---|
|ProjectN|[DataManager](https://github.com/jwonp9127/CodePortfolio/blob/main/ProjectN/Data/DataManager.cs)|
||[DataReader](https://github.com/jwonp9127/CodePortfolio/blob/main/ProjectN/Data/DataReader.cs)|
|TSEROF|[Leaf](https://github.com/jwonp9127/CodePortfolio/blob/main/TSEROF/Gimmick/Leaf.cs)|
||[WaypointPath](https://github.com/jwonp9127/CodePortfolio/blob/main/TSEROF/Gimmick/WaypointPath.cs)|
||[MobingObject](https://github.com/jwonp9127/CodePortfolio/blob/main/TSEROF/Gimmick/MobingObject.cs)|

## ProjectN

### 1. [DataManager](https://github.com/jwonp9127/CodePortfolio/blob/main/ProjectN/Data/DataManager.cs)
```csharp
public class DataManager : MonoBehaviour
{
    private static DataManager _instance;
    private DataReader _dataReader;
    private Dictionary<int, IDataContent> _dataContainer;
    private const string BaseDataPath = "Assets/Data/";
    
    // 에러 및 경고 메시지 상수
    private const string ErrorPath = "파일 경로를 찾을 수 없습니다: ";
    private const string WarningDataNotFound = "해당하는 데이터가 없습니다: ";
    private const string WarningDuplicateID = "중복된 아이디가 발견되었습니다. 아이디: ";

    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("DataManager").AddComponent<DataManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        _dataReader = new DataReader();
        _dataContainer = new Dictionary<int, IDataContent>();
        
        LoadDefaultData();
    }
    
    // 기본 데이터 로드 함수
    private void LoadDefaultData()
    {
        LoadData<Item>("ItemData.csv");
        LoadData<Crop>("CropData.csv");
        LoadData<NPC>("NPCData.csv");
    }

    // 특정 유형의 데이터 로드 함수
    private void LoadData<T>(string fileName) where T : class, IDataContent, new()
    {
        var dataDictionary = _dataReader.ReadData<T>(BaseDataPath + fileName);
        
        // 데이터 딕셔너리가 null인 경우 오류 메시지 출력 후 함수 종료
        if (dataDictionary == null)
        {
            Debug.LogError($"{ErrorPath}{fileName}");
            return;
        }
        
        // 데이터 딕셔너리에 데이터 추가
        foreach (var kvp in dataDictionary)
        {
            // 중복된 아이디 체크
            if (_dataContainer.ContainsKey(kvp.Key))
            {
                Debug.LogWarning($"{WarningDuplicateID}{kvp.Key}");
                continue; // 중복된 아이디는 스킵하고 다음 데이터로 넘어감
            }
            
            _dataContainer[kvp.Key] = kvp.Value;
        }
    }

    // 특정 데이터 가져오는 함수
    public T GetData<T>(int id) where T : class, IDataContent
    {
        // 데이터 컨테이너에서 데이터를 찾고 반환
        if(_dataContainer.TryGetValue(id, out var data))
        {
            return data as T;
        }
        else
        {
            // 데이터가 없는 경우 경고 메시지 출력 후 null 반환
            Debug.LogWarning($"{WarningDataNotFound}{id}");
            return null;
        }
    }
}
```

### 2. [DataReader](https://github.com/jwonp9127/CodePortfolio/blob/main/ProjectN/Data/DataReader.cs)
```csharp
public class DataReader
{
    // 타입에 따른 속성 캐싱
    private Dictionary<Type, PropertyInfo[]> _typeProperties = new Dictionary<Type, PropertyInfo[]>();
    private string[] _header;

    // 에러 및 경고 메시지 상수
    private const string ErrorPath = "파일 경로를 찾을 수 없습니다: ";
    private const string ErrorDataFormat = "데이터 형식이 잘못되었습니다";
    private const string ErrorEmptyHeader = "헤더가 비어 있습니다. 데이터를 읽을 수 없습니다";
    private const string WarningDataLength = "데이터의 길이와 헤더의 길이가 일치하지 않습니다 (line: {0})";
    private const string WarningInvalidData = "유효하지 않은 데이터가 있습니다 (line: {0})";
    private const string WarningDuplicateData = "중복된 데이터가 있습니다 (line: {0})";
    private const string ErrorEmptyData = "파일에 데이터가 없습니다";
    private const string ErrorEmptyDataDictionary = "데이터가 발견되지 않았습니다";
    private const string ErrorInvalidData = "잘못된 데이터 형식입니다";
    private const string WarningInvalidConvert = "값을 변환하는 중에 오류가 발생했습니다";

    // 데이터를 읽어들이는 메인 함수
    public Dictionary<int, T> ReadData<T>(string path) where T : IDataContent, new()
    {
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            Debug.LogError($"{ErrorPath}{path}");
            return null;
        }

        Dictionary<int, T> dataDictionary = new Dictionary<int, T>();
        PropertyInfo[] properties = GetTypeProperties<T>();

        if (properties == null || properties.Length == 0)
        {
            Debug.LogError(ErrorDataFormat);
            return null;
        }

        using (StreamReader streamReader = new StreamReader(path))
        {
            if (!ReadHeader(streamReader))
            {
                Debug.LogError(ErrorEmptyHeader);
                return null;
            }

            if (!ReadDataFromFile(streamReader, properties, dataDictionary))
            {
                Debug.LogError(ErrorEmptyData);
                return null;
            }
        }

        // 캐시 해제
        _typeProperties.Clear();

        if (dataDictionary.Count > 0)
        {
            return dataDictionary;
        }
        else
        {
            Debug.LogError(ErrorEmptyDataDictionary);
            return null;
        }
    }

    // 헤더를 읽어오는 함수
    private bool ReadHeader(StreamReader streamReader)
    {
        string headerLine = streamReader.ReadLine();
        if (string.IsNullOrEmpty(headerLine))
        {
            return false;
        }
        _header = headerLine.Split(',');
        return true;
    }

    // 데이터를 파일에서 읽어오는 함수
    private bool ReadDataFromFile<T>(StreamReader streamReader, PropertyInfo[] properties, Dictionary<int, T> dataDictionary) where T : IDataContent, new()
    {
        if (streamReader.EndOfStream)
        {
            Debug.LogError(ErrorEmptyData);
            return false;
        }
        
        int lineNumber = 1;
        while (!streamReader.EndOfStream)
        {
            string dataLine = streamReader.ReadLine();
            lineNumber++;

            // 비어있는 행 스킵
            if (string.IsNullOrEmpty(dataLine))
            {
                continue;
            }

            string[] values = dataLine.Split(',');

            if (values.Length != _header.Length)
            {
                Debug.LogWarning(string.Format(WarningDataLength, lineNumber));
                continue;
            }

            T data = ProcessData<T>(values, properties);

            if (data == null || string.IsNullOrEmpty(data.Name))
            {
                Debug.LogWarning(string.Format(WarningInvalidData, lineNumber));
                continue;
            }

            if (dataDictionary.ContainsKey(data.ID))
            {
                Debug.LogWarning(string.Format(WarningDuplicateData, lineNumber));
                continue;
            }

            dataDictionary[data.ID] = data;
        }

        return true;
    }

    // 타입에 따른 속성 가져오는 함수
    private PropertyInfo[] GetTypeProperties<T>() where T : IDataContent, new()
    {
        Type type = typeof(T);

        // 캐시에 존재할 경우 반환
        if (_typeProperties.TryGetValue(type, out var typeProperties))
        {
            return typeProperties;
        }

        PropertyInfo[] properties = type.GetProperties();
        _typeProperties[type] = properties;

        return properties;
    }

    // 데이터 처리 함수
    private T ProcessData<T>(string[] values, PropertyInfo[] properties) where T : IDataContent, new()
    {
        T instance = new T();
        int index = 0;

        foreach (var propertyInfo in properties)
        {
            var propertyType = propertyInfo.PropertyType;
            object convertedValue;

            // propertyType이 Dictionary일 때
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                convertedValue = ConvertDictionary(propertyType, values, ref index);
            }
            else if (propertyType.IsArray)
            {
                convertedValue = ConvertArray(propertyType, values, ref index);
            }
            else
            {
                convertedValue = ConvertType(values[index], propertyType);
            }

            if (convertedValue == null)
            {
                Debug.LogWarning($"{WarningInvalidData} (Property: {propertyInfo.Name})");
                return default;
            }
            propertyInfo.SetValue(instance, convertedValue);

            index++;
        }
        return instance;
    }

    // 데이터 타입 변환 함수
    private object ConvertType(string value, Type propertyType)
    {
        if (value == null)
        {
            return null;
        }
        if (propertyType.IsEnum)
        {
            if (Enum.TryParse(propertyType, value, out object enumValue))
            {
                return enumValue;
            }
            else
            {
                Debug.LogWarning(WarningInvalidConvert);
                return null;
            }
        }
        else
        {
            return Convert.ChangeType(value, propertyType);
        }
    }

    // Dictionary 타입 변환 함수
    private object ConvertDictionary(Type propertyType, string[] values, ref int index)
    {
        // Dictionary 형식을 가져옴
        Type[] genericArguments = propertyType.GetGenericArguments();
        Type keyType = genericArguments[0];
        Type valueType = genericArguments[1];

        // Dictionary 인스턴스 생성
        Type dictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
        object dictionary = Activator.CreateInstance(dictionaryType);
        
        int keyCount = Enum.GetNames(keyType).Length;
        
        for (int i = 0; i < keyCount; i++)
        {
            if (String.IsNullOrEmpty(values[index]))
            {
                index++;
                continue;
            }

            object key = ConvertType(_header[index], keyType);
            object value = ConvertType(values[index++], valueType);

            ((IDictionary)dictionary).Add(key, value);
        }

        index--;
        return dictionary;
    }

    // 배열 타입 변환 함수
    private object ConvertArray(Type propertyType, string[] values, ref int index)
    {
        Type elementType = propertyType.GetElementType();
        int arrayLength = int.Parse(values[index]);

        if (elementType == null)
        {
            Debug.LogError(ErrorInvalidData);
            return null;
        }
        
        Array array = Array.CreateInstance(elementType, arrayLength);

        for (int i = 0; i < arrayLength; i++)
        {
            index++;

            object value = ConvertType(values[index], elementType);
            array.SetValue(value, i);
        }
        
        while (index + 1 < values.Length && String.IsNullOrEmpty(values[index + 1]))
        {
            index++;
        }

        return array;
    }
}
```

## TSEROF

### 1. [Leaf](https://github.com/jwonp9127/CodePortfolio/blob/main/TSEROF/Gimmick/Leaf.cs)

```csharp
public class Leaf : MonoBehaviour
{
    [SerializeField] private GameObject icon;
    
    private Player _player;
    private bool _isOnRide;
    private float _speedMultiplier_f = 1.7f;

    private void OnTriggerEnter(Collider other)
    {
        icon.SetActive(true);
        _player = other.GetComponent<Player>();
        
        if (_isOnRide || _player == null)
        {
            return;
        }
        
        _player.Input.PlayerActions.Interaction.started += CheckOnRideStarted;
    }

    private void OnTriggerExit(Collider other)
    {
        icon.SetActive(false);
        _player = other.GetComponent<Player>();
        
        if (_isOnRide || _player == null)
        {
            return;
        }
        
        _player.Input.PlayerActions.Interaction.started -= CheckOnRideStarted;
    }
    
    private void CheckOnRideStarted(InputAction.CallbackContext obj)
    {
        if (!_isOnRide)
        {
            _isOnRide = true;
            _player.transform.position = transform.position;
            _player.transform.rotation = transform.rotation;
            _player.movementSpeed *= _speedMultiplier_f;
            transform.SetParent(_player.transform);
        
            // Section Change
            Stage2Manager.Instance.SetSection(2);
        }
    }
}
```

### 2. [WaypointPath](https://github.com/jwonp9127/CodePortfolio/blob/main/TSEROF/Gimmick/WaypointPath.cs)

```csharp
public class WaypointPath : MonoBehaviour
{
    private int _waypointCount;
    private Transform[] _waypoints;

    private const string ErrorWaypointLoad = "Waypoint Load Error";
    
    private void Start()
    {
        LoadWaypoints();
    }

    // Waypoint를 로드하여 배열에 저장합니다.
    private void LoadWaypoints()
    {
        _waypointCount = transform.childCount;
        _waypoints = new Transform[_waypointCount];
        
        for (int i = 0; i < _waypointCount; i++)
        {
            _waypoints[i] = transform.GetChild(i);
        }

        if (_waypointCount == 0)
        {
            Debug.LogError(ErrorWaypointLoad);
        }
    }

    // 해당 인덱스의 Waypoint를 반환합니다.
    public Transform GetWaypoint(int waypointIndex)
    {
        if (waypointIndex >= 0 && waypointIndex < _waypointCount)
        {
            return _waypoints[waypointIndex];
        }
        else
        {
            return null;
        }
    }

    // 다음 Waypoint를 반환하고 현재 인덱스를 증가시킵니다.
    public Transform GetNextWaypoint(ref int currentIndex)
    {
        currentIndex++;

        if (currentIndex >= _waypointCount)
        {
            currentIndex = 0;
        }

        return _waypoints[currentIndex];
    }

    // 이전 Waypoint를 반환하고 현재 인덱스를 감소시킵니다.
    public Transform GetPreviousWaypoint(ref int currentIndex)
    {
        currentIndex--;

        if (currentIndex < 0)
        {
            currentIndex = _waypointCount - 1;
        }

        return _waypoints[currentIndex];
    }
}
```

### 3. [MovingObject](https://github.com/jwonp9127/CodePortfolio/blob/main/TSEROF/Gimmick/MovingObject.cs)

```csharp
public class MovingObject : MonoBehaviour
{
    [Header("Waypoint Path")]
    [SerializeField] private WaypointPath _waypointPath;

    [Header("Option")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private bool _isCollisionMoving;
    [SerializeField] private bool _isReverse;
    
    private int _currentIndex;
    private Transform _currentWaypoint;
    private Transform _targetWaypoint;

    private float _timeToWaypoint;
    private float _elapsedTime;
    
    private bool _canMove;

    private void Start()
    {
        SetTargetWaypoint();
    }

    private void Update()
    {
        if (!_isCollisionMoving || _canMove)
        {
            Move();
        }
    }

    private void Move()
    {
        _elapsedTime += Time.deltaTime;

        float elapsedPercentage = _elapsedTime / _timeToWaypoint;
        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
        
        // waypoint 사이을 보간하여 이동
        transform.position = Vector3.Lerp(_currentWaypoint.position, _targetWaypoint.position, elapsedPercentage);
        transform.rotation = Quaternion.Slerp(_currentWaypoint.rotation, _targetWaypoint.rotation, elapsedPercentage);
        
        if (elapsedPercentage >= 1)
        {
            SetTargetWaypoint();
        }
    }

    private void SetTargetWaypoint()
    {
        _currentWaypoint = _waypointPath.GetWaypoint(_currentIndex);

        if (!_isReverse)
        {
            _targetWaypoint = _waypointPath.GetNextWaypoint(ref _currentIndex);
        }
        else
        {
            _targetWaypoint = _waypointPath.GetPreviousWaypoint(ref _currentIndex);
        }

        _elapsedTime = 0;

        float distanceToWaypoint = Vector3.Distance(_currentWaypoint.position, _targetWaypoint.position);
        _timeToWaypoint = distanceToWaypoint / _movementSpeed;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Globals.PlayerTag))
        {
            collision.transform.SetParent(transform);
            _canMove = true;
        }
    }

    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(Globals.PlayerTag))
        {
            collision.transform.SetParent(null);
            _canMove = false;
        }
    }
}
```


