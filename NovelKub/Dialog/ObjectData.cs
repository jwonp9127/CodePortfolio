using UnityEngine;
using UnityEngine.Serialization;

public enum ObjectNum
{
    NPC1 = 1000,
    NPC2 = 2000,
    NPC3 = 3000,
    NPC4 = 4000,
    NPC5 = 5000,
    NPC6 = 6000,
    NPC7 = 7000, 
    NPC8 = 8000,
	NPC9 = 9000,

	InteractableObject1 = 11000,
    Evidence2 = 12000,
    Evidence3 = 13000,
    Evidence4 = 14000,
    Evidence5 = 15000
}
public class ObjectData : MonoBehaviour
{
    public int objectId;
    public bool withQuest;
}
