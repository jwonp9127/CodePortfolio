using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        // PrintItemLog<Item>("이름1");
        // PrintAbility("이름1", Ability.AttackPower);
        // PrintAbility("이름1", Ability.SP);
        // PrintCropLog<Crop>("농작물1");
        PrintNPCLog(1);
        PrintNPCLog(2);
    }

    private void PrintItemLog<T>(int id) where T : class, IDataContent, new()
    {
        Item item = DataManager.Instance.GetData<Item>(id);

        if (item == null)
        {
            Debug.LogError("");
            return;
        }
        
        Debug.Log("id : " + item.ID);
        Debug.Log("name : " + item.Name);
        Debug.Log("Description : " + item.Description);
        Debug.Log("prefabPath : " + item.PrefabPath);
        Debug.Log("UIImagePath : " + item.UIImagePath);
        Debug.Log("ItemType : " + item.ItemType);
        Debug.Log("Price : " + item.Price);
        for (int i = 0; i < System.Enum.GetValues(typeof(Ability)).Length; i++)
        {
            if (item.Ability.TryGetValue((Ability)i, out var value))
            {
                Debug.Log("Ability" + (Ability)i + " : " + value);
            }
            else
            {
                Debug.Log("Ability" + (Ability)i + "는 없음");
            }
        }
    }

    private void PrintAbility(int id, Ability abilityName)
    {
        Item item = DataManager.Instance.GetData<Item>(id);

        if (item.GetAbility(abilityName, out float value))
        {
            Debug.Log(abilityName + " : " + value);
        }
        else
        {
            Debug.Log(abilityName+"은(는) 없음");
        }
    }
    
    private void PrintCropLog<T>(int id)
    {
        Crop cropData = DataManager.Instance.GetData<Crop>(id);

        if (cropData == null)
        {
            Debug.LogError("");
            return;
        }
        
        Debug.Log("id : " + cropData.ID);
        Debug.Log("name : " + cropData.Name);
        Debug.Log("HarvestPrefabPath : " + cropData.HarvestPrefabPath);
        Debug.Log("HarvestQuantity : " + cropData.HarvestQuantity);
        
        for (int i = 0; i < cropData.GrowthDays.Length; i++)
        { 
            Debug.Log($"Growth {i+1} + (Day {cropData.GrowthDays[i]} ~ ) : {cropData.VariationPrefabPaths[i]}");
        }
    }
    
    private void PrintNPCLog(int id)
    {
        NonPlayerCharacter NPC = DataManager.Instance.GetData<NonPlayerCharacter>(id);

        if (NPC == null)
        {
            Debug.LogError("");
            return;
        }
        
        Debug.Log("id : " + NPC.ID);
        Debug.Log("name : " + NPC.Name);
        Debug.Log("CharacterType : " + NPC.CharacterType);
        Debug.Log("PrefabPath : " + NPC.PrefabPath);
        for (int i = 0; i < System.Enum.GetValues(typeof(Ability)).Length; i++)
        {
            if (NPC.Ability.TryGetValue((Ability)i, out var value))
            {
                Debug.Log("Ability" + (Ability)i + " : " + value);
            }
            else
            {
                Debug.Log("Ability" + (Ability)i + "는 없음");
            }
        }
    }
    
}
