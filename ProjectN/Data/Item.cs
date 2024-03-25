using System;
using System.Collections.Generic;
using UnityEngine;

public class Item : IDataContent
{
    public int ID { get; set; }
    public ItemType ItemType { get; private set; }
    public string Name { get; set; }
    public string Description { get; private set; }
    public string UIImagePath { get; private set; }
    public string PrefabPath { get; private set; }
    public Dictionary<Ability, float> Ability { get; private set; }
    public int Price { get; private set; }

    public Item()
    {
        Ability = new Dictionary<Ability, float>();
    }
    
    public Item(Item item)
    {
        ID = item.ID;
        ItemType = item.ItemType;
        Name = item.Name;
        Description = item.Description;
        UIImagePath = item.UIImagePath;
        PrefabPath = item.PrefabPath;
        Ability = new Dictionary<Ability, float>(item.Ability);
        Price = item.Price;
    }

    public bool GetAbility(Ability abilityType, out float abilityValue)
    {
        if(Ability.TryGetValue(abilityType, out abilityValue))
        {
            return true;
        }
        else
        {
            abilityValue = 0;
            return false;
        }
    }
}
