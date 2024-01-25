using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipment,
    Consumable,
    Material
}

public enum Ability
{
    A,
    B,
    C
}

public class Item : IDataContent
{
    public int id { get; set; }
    public string Name { get; set; }
    public string UIImageName { get; set; }
    public string PrefabName { get; set; }
    public int Price { get; set; }
    public ItemType ItemType { get; set; }
    public Dictionary<Ability, int> Ability { get; set; }

    public Item()
    {
        Ability = new Dictionary<Ability, int>();
    }
    
    public Item(Item item)
    {
        id = item.id;
        Name = item.Name;
        ItemType = item.ItemType;
        UIImageName = item.UIImageName;
        PrefabName = item.PrefabName;
        Price = item.Price;
        Ability = new Dictionary<Ability, int>(item.Ability);
    }
}
