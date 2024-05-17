using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Dictionary<Type, PropertyInfo[]> _typeProperties = new Dictionary<Type, PropertyInfo[]>();
    
    private void Start()
    {
        PrintInfoAll<Item>(Category.Item);
        // PrintInfoAll<Crop>(Category.Crop);
        // PrintInfoAll<NPC>(Category.NPC);
    }
    
    private void PrintInfo<T>(int id) where T : class, IDataContent, new()
    {
        T data = DataManager.Instance.GetData<T>(id);
        
        if (data == null)
        {
            Debug.LogError($"해당하는 id의 {typeof(T)} 없음");
            return;
        }

        PrintLog(data);
    }
    
    private void PrintInfoAll<T>(Category category) where T : class, IDataContent, new()
    {
        Debug.Log($"[{category}]");
        Debug.Log("------------------------------------");
        var count = 0;
        
        for (int id = 0; id < 1000; id++)
        {
            T data = DataManager.Instance.GetData<T>(id + (int)category);

            if (data == null)
            {
                continue;
            }
            count++;
            PrintLog(data);
            Debug.Log("------------------------");
        }
        Debug.Log($"Total {typeof(T)} : {count}");
        Debug.Log("------------------------------------");
    }

    private void PrintLog<T>(T data) where T : IDataContent, new()
    {
        PropertyInfo[] properties = GetTypeProperties<T>();
        
        foreach (var propertyInfo in properties)
        {
            var propertyType = propertyInfo.PropertyType;
            var propertyValue = propertyInfo.GetValue(data);

            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                Debug.Log($"{propertyInfo.Name} :");
                var dictionary = (IDictionary)propertyValue;
                foreach (DictionaryEntry kvp in dictionary)
                {
                    Debug.Log($"- {kvp.Key} : {kvp.Value}");
                }
            }
            else if (propertyType.IsArray)
            {
                Debug.Log($"{propertyInfo.Name} :");
                var array = (Array)propertyValue;
                foreach (var item in array)
                {
                    Debug.Log($"- {item}");
                }
            }
            else
            {
                Debug.Log($"{propertyInfo.Name} : {propertyValue}");
            }
        }
    }
    
    private PropertyInfo[] GetTypeProperties<T>() where T : IDataContent, new()
    {
        Type type = typeof(T);

        if (_typeProperties.TryGetValue(type, out var typeProperties))
        {
            return typeProperties;
        }

        PropertyInfo[] properties = type.GetProperties();
        _typeProperties[type] = properties;

        return properties;
    }

}
