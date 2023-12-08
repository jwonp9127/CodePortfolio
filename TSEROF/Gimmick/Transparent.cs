using System.Collections;
using UnityEngine;

public class Transparent : MonoBehaviour
{
    //private GameObject realNeedleTrap;
    //private GameObject fakeNeedleTrap;

    //private Transform Pos1;
    //private Transform Pos2;

    [SerializeField] private GameObject _Needle;
    private int _random;
    private bool _isShow;
    private MeshRenderer meshRenderer;
    
    private void Start()
    {
        StartSetting();
    }

    private void StartSetting()
    {
        _Needle.GetComponent<MeshRenderer>().enabled = _isShow;

        _random = Random.Range(1, 3);
        if (_random == 1)
        {
            transform.GetChild(0).position = transform.GetChild(2).position;
            transform.GetChild(1).position = transform.GetChild(3).position;
        }
        else
        {
            transform.GetChild(0).position = transform.GetChild(3).position;
            transform.GetChild(1).position = transform.GetChild(2).position;
        }

    }

    private void OnNeedle()
    {
        meshRenderer.enabled = true;
    }

    private void OffNeedle()
    {
        meshRenderer.enabled = false;
    }


    private IEnumerator ShowStart()
    {
        meshRenderer= _Needle.GetComponent<MeshRenderer>();

        if (meshRenderer == null)
        {
            yield break;
        }

        OnNeedle();
        yield return new WaitForSeconds(10f);
        OffNeedle();
    }

    public void StartShowCoroutine()
    {
        StartCoroutine(ShowStart());
    }


}
