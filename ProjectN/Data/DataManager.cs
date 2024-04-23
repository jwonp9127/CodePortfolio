using System.Collections.Generic;
using UnityEngine;

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