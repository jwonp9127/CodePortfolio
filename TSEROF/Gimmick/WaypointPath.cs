using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    private int _waypointCount;
    private Transform[] _waypoints;
    
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
            Debug.LogError("Waypoint Load Error");
        }
    }
    
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

    // 주어진 현재 index의 이전 waypoint를 반환합니다.
    public Transform GetPreviousWaypoint(ref int currentIndex)
    {
        currentIndex--;

        if (currentIndex < 0)
        {
            currentIndex = _waypointCount - 1;
        }

        return _waypoints[currentIndex];
    }
    
    // 주어진 현재 index의 다음 waypoint를 반환합니다.
    public Transform GetNextWaypoint(ref int currentIndex)
    {
        currentIndex++;
        
        if (currentIndex >= _waypointCount)
        {
            currentIndex = 0;
        }

        return _waypoints[currentIndex];
    }
}