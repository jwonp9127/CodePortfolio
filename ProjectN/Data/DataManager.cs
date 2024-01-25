using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;
    private DataReader _dataReader;
    private List<IDataContent> _dataList;

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
            Destroy(this.gameObject);
            return;
        }

        _instance = this;

        _dataReader = new DataReader();
        _dataList = new List<IDataContent>();
        
        LoadDefaultData();
    }
    
    private void LoadDefaultData()
    {
        _dataList.AddRange(_dataReader.ReadData<Item>("Assets/PJW/Data/ItemData.csv"));
    }

    public T GetData<T>(string dataName) where T : class, IDataContent
    {
        return _dataList.Find(data => data.Name == dataName) as T;
        // 중복 고려 안 함.
    }
}
