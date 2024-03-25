using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class DataReader
{
    // 타입에 따른 속성 캐싱
    private Dictionary<Type, PropertyInfo[]> _typeProperties = new Dictionary<Type, PropertyInfo[]>();
    private string[] _header;

    // 에러 및 경고 메시지 상수
    private const string ErrorPath = "파일 경로를 찾을 수 없습니다: ";
    private const string ErrorDataFormat = "데이터 형식이 잘못되었습니다";
    private const string ErrorEmptyHeader = "헤더가 비어 있습니다. 데이터를 읽을 수 없습니다";
    private const string WarningDataLength = "데이터의 길이와 헤더의 길이가 일치하지 않습니다 (line: {0})";
    private const string WarningInvalidData = "유효하지 않은 데이터가 있습니다 (line: {0})";
    private const string WarningDuplicateData = "중복된 데이터가 있습니다 (line: {0})";
    private const string ErrorEmptyData = "파일에 데이터가 없습니다";
    private const string ErrorEmptyDataDictionary = "데이터가 발견되지 않았습니다";
    private const string ErrorInvalidData = "잘못된 데이터 형식입니다";
    private const string WarningInvalidConvert = "값을 변환하는 중에 오류가 발생했습니다";

    // 데이터를 읽어들이는 메인 함수
    public Dictionary<int, T> ReadData<T>(string path) where T : IDataContent, new()
    {
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            Debug.LogError($"{ErrorPath}{path}");
            return null;
        }

        Dictionary<int, T> dataDictionary = new Dictionary<int, T>();
        PropertyInfo[] properties = GetTypeProperties<T>();

        if (properties == null || properties.Length == 0)
        {
            Debug.LogError(ErrorDataFormat);
            return null;
        }

        using (StreamReader streamReader = new StreamReader(path))
        {
            if (!ReadHeader(streamReader))
            {
                Debug.LogError(ErrorEmptyHeader);
                return null;
            }

            if (!ReadDataFromFile(streamReader, properties, dataDictionary))
            {
                Debug.LogError(ErrorEmptyData);
                return null;
            }
        }

        // 캐시 해제
        _typeProperties.Clear();

        if (dataDictionary.Count > 0)
        {
            return dataDictionary;
        }
        else
        {
            Debug.LogError(ErrorEmptyDataDictionary);
            return null;
        }
    }

    // 헤더를 읽어오는 함수
    private bool ReadHeader(StreamReader streamReader)
    {
        string headerLine = streamReader.ReadLine();
        if (string.IsNullOrEmpty(headerLine))
        {
            return false;
        }
        _header = headerLine.Split(',');
        return true;
    }

    // 데이터를 파일에서 읽어오는 함수
    private bool ReadDataFromFile<T>(StreamReader streamReader, PropertyInfo[] properties, Dictionary<int, T> dataDictionary) where T : IDataContent, new()
    {
        if (streamReader.EndOfStream)
        {
            Debug.LogError(ErrorEmptyData);
            return false;
        }
        
        int lineNumber = 1;
        while (!streamReader.EndOfStream)
        {
            string dataLine = streamReader.ReadLine();
            lineNumber++;

            // 비어있는 행 스킵
            if (string.IsNullOrEmpty(dataLine))
            {
                continue;
            }

            string[] values = dataLine.Split(',');

            if (values.Length != _header.Length)
            {
                Debug.LogWarning(string.Format(WarningDataLength, lineNumber));
                continue;
            }

            T data = ProcessData<T>(values, properties);

            if (data == null || string.IsNullOrEmpty(data.Name))
            {
                Debug.LogWarning(string.Format(WarningInvalidData, lineNumber));
                continue;
            }

            if (dataDictionary.ContainsKey(data.ID))
            {
                Debug.LogWarning(string.Format(WarningDuplicateData, lineNumber));
                continue;
            }

            dataDictionary[data.ID] = data;
        }

        return true;
    }

    // 타입에 따른 속성 가져오는 함수
    private PropertyInfo[] GetTypeProperties<T>() where T : IDataContent, new()
    {
        Type type = typeof(T);

        // 캐시에 존재할 경우 반환
        if (_typeProperties.TryGetValue(type, out var typeProperties))
        {
            return typeProperties;
        }

        PropertyInfo[] properties = type.GetProperties();
        _typeProperties[type] = properties;

        return properties;
    }

    // 데이터 처리 함수
    private T ProcessData<T>(string[] values, PropertyInfo[] properties) where T : IDataContent, new()
    {
        T instance = new T();
        int index = 0;

        foreach (var propertyInfo in properties)
        {
            var propertyType = propertyInfo.PropertyType;
            object convertedValue;

            // propertyType이 Dictionary일 때
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                convertedValue = ConvertDictionary(propertyType, values, ref index);
            }
            else if (propertyType.IsArray)
            {
                convertedValue = ConvertArray(propertyType, values, ref index);
            }
            else
            {
                convertedValue = ConvertType(values[index], propertyType);
            }

            if (convertedValue == null)
            {
                Debug.LogWarning($"{WarningInvalidData} (Property: {propertyInfo.Name})");
                return default;
            }
            propertyInfo.SetValue(instance, convertedValue);

            index++;
        }
        return instance;
    }

    // 데이터 타입 변환 함수
    private object ConvertType(string value, Type propertyType)
    {
        if (value == null)
        {
            return null;
        }
        if (propertyType.IsEnum)
        {
            if (Enum.TryParse(propertyType, value, out object enumValue))
            {
                return enumValue;
            }
            else
            {
                Debug.LogWarning(WarningInvalidConvert);
                return null;
            }
        }
        else
        {
            return Convert.ChangeType(value, propertyType);
        }
    }

    // Dictionary 타입 변환 함수
    private object ConvertDictionary(Type propertyType, string[] values, ref int index)
    {
        // Dictionary 형식을 가져옴
        Type[] genericArguments = propertyType.GetGenericArguments();
        Type keyType = genericArguments[0];
        Type valueType = genericArguments[1];

        // Dictionary 인스턴스 생성
        Type dictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
        object dictionary = Activator.CreateInstance(dictionaryType);

        int iterations = int.Parse(values[index]);

        for (int i = 0; i < iterations; i++)
        {
            index++;
            
            if (String.IsNullOrEmpty(values[index]))
            {
                continue;
            }

            object key = ConvertType(_header[index], keyType);
            object value = ConvertType(values[index], valueType);

            ((IDictionary)dictionary).Add(key, value);
        }
        return dictionary;
    }

    // 배열 타입 변환 함수
    private object ConvertArray(Type propertyType, string[] values, ref int index)
    {
        Type elementType = propertyType.GetElementType();
        int arrayLength = int.Parse(values[index]);

        if (elementType == null)
        {
            Debug.LogError(ErrorInvalidData);
            return null;
        }
        
        Array array = Array.CreateInstance(elementType, arrayLength);

        for (int i = 0; i < arrayLength; i++)
        {
            index++;

            object value = ConvertType(values[index], elementType);
            array.SetValue(value, i);
        }
        
        while (index + 1 < values.Length && String.IsNullOrEmpty(values[index + 1]))
        {
            index++;
        }

        return array;
    }
}
