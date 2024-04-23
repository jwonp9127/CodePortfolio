using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : IDataContent
{
    public int ID { get; set; }
    public string Name { get; set; }
    public NPCType NPCType { get; set; }
    public string PrefabPath { get; set; }
    public Dictionary<Ability, float> Ability { get; set; }

    public NPC()
    {
        Ability = new Dictionary<Ability, float>();
    }

    public NPC(NPC npc)
    {
        ID = npc.ID;
        Name = npc.Name;
        NPCType = npc.NPCType;
        PrefabPath = npc.PrefabPath;
        Ability = new Dictionary<Ability, float>(npc.Ability);
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

    public void SetAbility(Ability abilityType, float abilityValue)
    {
        Ability[abilityType] = abilityValue;
    }
}
