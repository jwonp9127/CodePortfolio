using UnityEngine;

public class SideNeedleTraps : MonoBehaviour
{
    [Header("Object Pos")]
    [SerializeField]private Transform _pos1;
    [SerializeField] private Transform _pos2;
    [SerializeField] private Transform _thornPos;

    [Header("Object Time")]
    [SerializeField] public float _pos1Time;
    [SerializeField] public float _pos2Time;

    private int count;
    void Start()
    {
        _pos1 = transform.GetChild(0);
        _pos2 = transform.GetChild(1);
        _thornPos = transform.GetChild(2);
        _thornPos.position = _pos1.position;
    }

    void Update()
    {
        ThornMove();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        Debug.Log("가시에 닿았습니다");
    //    }
    //}

    public void ThornMove()
    {
        if (count == 0)
        {
            _thornPos.position = Vector3.MoveTowards(_thornPos.position, _pos1.position, 0.03f);//0.15f_pos1Time
            if (_thornPos.position == _pos1.position)
            {
                count++;
            }

        }
        if (count == 1)
        {
            _thornPos.position = Vector3.MoveTowards(_thornPos.position, _pos2.position, 0.12f);// 0.01f_pos2Time
            if (_thornPos.position == _pos2.position)
            {
                count--;
            }

        }
    }
}
