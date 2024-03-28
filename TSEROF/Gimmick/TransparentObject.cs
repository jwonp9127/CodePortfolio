using UnityEngine;

public class TransparentObject : MonoBehaviour
{
    private int _childCount;
    private GameObject[] _child;
    
    private void Start()
    {
        _childCount = transform.childCount;
        if (_childCount > 0)
        {
            _child = new GameObject[_childCount];
            GetChildObjects();
            SetChildActive(false);            
        }
    }
    
    // Child Object 캐싱
    private void GetChildObjects()
    {
        for (int i = 0; i < _childCount; i++)
        {
            _child[i] = transform.GetChild(i).gameObject;
        }
    }

    // 충돌 시작시 모든 자식 오브젝트 활성화
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(Globals.PlayerTag))
        {
            SetChildActive(true);
        }
    }
    
    // 충돌 종료시 모든 자식 오브젝트 비활성화
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag(Globals.PlayerTag))
        {
            SetChildActive(false);
        }
    }

    // 모든 자식 오브젝트 활성화 여부 설정
    private void SetChildActive(bool isActive)
    {
        for (int i = 0; i < _childCount; i++)
        {
            _child[i]?.SetActive(isActive);
        }
    }
}
