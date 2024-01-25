public enum QuestNum
{
    First = 100,
    Second = 200,
    Third = 300,
    Forth = 400,
    Fifth = 500,
    Sixth = 600,
    Seventh = 700
}

public enum QuestMiniGame
{
    First = 0,
    Second = 1,
    Third = 2,
    Forth = 4,
    Fifth = 5,
    Sixth = 6,
    Seventh = 7
}
public class QuestData
{
    public string QuestName;
    public string[] QuestInfo;
    public int[] NpcId;

    public QuestData(string name, string[] info, int[] npc)
    {
        QuestName = name;
        QuestInfo = info;
        NpcId = npc;
    }
}
public class QuestItem
{
    //public ItemData[] QuestItemDatas = new ItemData[8];
    int NpcIndex;
    int TalkIndex;
    ItemData QuestItemData;

    public QuestItem(int npcIndex, int talkIndex, ItemData item)
    {
        NpcIndex = npcIndex;
        TalkIndex = talkIndex;
        QuestItemData = item;
    }


}
