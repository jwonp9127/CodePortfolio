using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class DataReader
{
    private Dictionary<string, PropertyInfo[]> typePropertiesCache = new Dictionary<string, PropertyInfo[]>();
    private string _inputData = "";
    
    public List<T> ReadData<T>(string path) where T : IDataContent, new()
    {
        List<T> dataList = new List<T>();
        
        using (StreamReader streamReader = new StreamReader(path))
        {
            streamReader.ReadLine();
        
            while (!streamReader.EndOfStream)
            {
                PropertyInfo[] properties = GetTypeProperties<T>();

                _inputData = streamReader.ReadLine();
                
                if (_inputData == null)
                {
                    break;
                }
            
                string[] values = _inputData.Split(',');
                T data = Process<T>(values, properties);
                //data check 과정 필요함
                dataList.Add(data);
            }
        }
        return dataList;
    }

    private PropertyInfo[] GetTypeProperties<T>() where T : IDataContent, new()
    {
        var typeName = typeof(T).FullName;

        if (typeName == null)
        {
            Debug.Log("해당하는 data 형식이 없음");
            return null;
        }
        
        // 캐시에서 속성들을 가져오거나 없을 경우 속성들을 가져와서 캐시에 추가
        if (!typePropertiesCache.TryGetValue(typeName, out var properties))
        {
            properties = typeof(T).GetProperties();
            typePropertiesCache[typeName] = properties;
        }

        return properties;
    }

    private T Process<T>(string[] values, PropertyInfo[] properties) where T : IDataContent, new()
    {
        T instance = new T();

        for (int i = 0; i < properties.Length; i++)
        {
            var propertyType = properties[i].PropertyType;
            
            if (propertyType.IsEnum) // Enum 처리
            {
                properties[i].GetSetMethod().Invoke(instance, new object[] { Enum.Parse(propertyType, values[i]) });
            }
            else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                Type[] genericArguments = propertyType.GetGenericArguments();

                // 일반적인 처리를 위해 제네릭 인자의 형식을 가져옴
                Type keyType = genericArguments[0];
                Type valueType = genericArguments[1];

                // Dictionary 형식을 만들어 인스턴스 생성
                Type dictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
                object dictionary = Activator.CreateInstance(dictionaryType);

                int dictionaryCount = int.Parse(values[i]);

                for (int j = 0; j < dictionaryCount * 2; j += 2)
                {
                    if (!Enum.TryParse(keyType, values[i + 1 + j], out object key))
                    {
                        key = Convert.ChangeType(values[i + 1 + j], keyType);
                    }
                    object value = Convert.ChangeType(values[i + 2 + j], valueType);

                    ((IDictionary)dictionary).Add(key, value);
                }

                properties[i].GetSetMethod().Invoke(instance, new object[] { dictionary });
                
                // dictionary 사용 후 처리
                i += 2 * dictionaryCount;
            }
            else
            {
                properties[i].GetSetMethod().Invoke(instance, new object[] { Convert.ChangeType(values[i], propertyType) });
            }
        }
        return instance;
    }
}
