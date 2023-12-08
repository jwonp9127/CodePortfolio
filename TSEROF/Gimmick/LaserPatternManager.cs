using UnityEngine;
using Random = System.Random;

public class LaserPatternManager : MonoBehaviour
{
    private readonly int[,] _randomPair = new int[6, 2] { { 3, 10 }, { 0, 5 }, { 2, 7 }, { 4, 9 }, { 6, 11 }, { 1, 8 } };
    private int[] _selectedLaser = new int[6];
    private Transform[] _laserTransform = new Transform[6];
    public GameObject Stage2ClearCutScene;
    public int successCount;
    private bool _isClear;

    private void Update()
    {
        if (_isClear)
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
        Stage2ClearCutScene.SetActive(false);
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
        Random rand = new Random();
        for (int i = 0; i < 6; i++)
        {
            _selectedLaser[i] = _randomPair[i, rand.Next(2)];
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
        Stage2ClearCutScene.SetActive(true);
        _isClear = true;
    }

}
