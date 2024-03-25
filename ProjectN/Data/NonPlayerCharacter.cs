using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : IDataContent
{
    public int ID { get; set; }
    public string Name { get; set; }
    public CharacterType CharacterType { get; set; }
    public string PrefabPath { get; set; }
    public Dictionary<Ability, float> Ability { get; set; }

    public NonPlayerCharacter()
    {
        Ability = new Dictionary<Ability, float>();
    }

    public NonPlayerCharacter(NonPlayerCharacter nonPlayerCharacter)
    {
        ID = nonPlayerCharacter.ID;
        Name = nonPlayerCharacter.Name;
        CharacterType = nonPlayerCharacter.CharacterType;
        PrefabPath = nonPlayerCharacter.PrefabPath;
        Ability = new Dictionary<Ability, float>(nonPlayerCharacter.Ability);
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
