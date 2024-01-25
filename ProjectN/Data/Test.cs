using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        Debug.Log(DataManager.Instance.GetData<Item>("이름2").id);
        Debug.Log(DataManager.Instance.GetData<Item>("이름2").Name);
        Debug.Log(DataManager.Instance.GetData<Item>("이름2").PrefabName);
        Debug.Log(DataManager.Instance.GetData<Item>("이름2").UIImageName);
        Debug.Log(DataManager.Instance.GetData<Item>("이름2").ItemType);
        Debug.Log(DataManager.Instance.GetData<Item>("이름2").Price);
        Debug.Log(DataManager.Instance.GetData<Item>("이름2").Ability);
    }
}
