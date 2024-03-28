using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class Stage2Manager : MonoBehaviour
{
    private const int SECTION_COUNT = 3;
    
    [Header("Section")]
    [SerializeField] private GameObject[] _objects = new GameObject[SECTION_COUNT];
    [SerializeField] private CinemachineVirtualCamera[] _cameras = new CinemachineVirtualCamera[SECTION_COUNT];
    [SerializeField] private Vector3[] _respawnPositions = new Vector3[SECTION_COUNT];
    [SerializeField] private GameObject[] _players = new GameObject[SECTION_COUNT];
    
    public int CurrentSection { get; private set; }
    public Vector3 CurrentRespawnPosition { get; private set; }
    public GameObject CurrentPlayer { get; private set; }
    
    private static Stage2Manager _instance;

    public static Stage2Manager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("DataManager").AddComponent<Stage2Manager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    private void Start()
    {
        CurrentSection = 0;
        SetSection(CurrentSection);
    }

    public void SetSection(int section)
    {
        CurrentSection = section;
        CurrentRespawnPosition = _respawnPositions[section];

        for (int i = 0; i < SECTION_COUNT; i++)
        {
            _cameras[i].enabled = (section == i);
            _objects[i].SetActive(section == i);
        }

        if (_players[section] != null)
        {
            for (int i = 0; i < SECTION_COUNT; i++)
            {
                _players[i]?.SetActive(section == i);
            }
            CurrentPlayer = _players[section];
        }
    }

    public void ClearStage2()
    {
        GameManager.Instance.isSecondStageClear = true;
        GameManager.Instance.isSpawn = true;
        SceneManager.LoadScene("StageSelect");
    }
}