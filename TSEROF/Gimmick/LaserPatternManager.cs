using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

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