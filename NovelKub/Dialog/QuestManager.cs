using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex;
    public GameObject manager;
    public TimelineManager timelineManager;

    public string QuestName { get; private set; }
    public string QuestInfo { get; private set; }
    
    private Dictionary<int, QuestData> _questDate;

    private void Awake()
    {
        timelineManager = manager.GetComponent<TimelineManager>();
        _questDate = new Dictionary<int, QuestData>();
        GenerateData();
    }

    private void Start()
    {
        QuestName = CheckQuest();
    }

    private void GenerateData()
    {
        _questDate.Add((int)QuestNum.First,
            new QuestData("탐정의 등장", new string[] { "중식당의 사장님과 대화하기" },
                new int[] { (int)ObjectNum.NPC1 }));
        _questDate.Add((int)QuestNum.Second,
            new QuestData("첫 번째 실마리", new string[] { "중식당의 사장님과 대화하기", "펍에 가서 CCTV 확인해보기" },
                new int[] { (int)ObjectNum.NPC2, (int)ObjectNum.NPC2 }));
		_questDate.Add((int)QuestNum.Third,
			new QuestData("지켜보고있다", new string[] { "건물주 할아버지를 통해 CCTV 확인하기", "건물주 할아버지를 통해 CCTV 확인하기", "피자가게 직원과 대화하기" },
				new int[] { (int)ObjectNum.InteractableObject1, (int)ObjectNum.NPC3, (int)ObjectNum.NPC3 }));
		_questDate.Add((int)QuestNum.Forth,
			new QuestData("수상한 그림자", new string[] { "피자가게 직원과 대화하기", "피자가게 직원이 얘기한 노숙자를 찾아 대화하기" },
				new int[] { (int)ObjectNum.NPC4, (int)ObjectNum.NPC4 }));
		_questDate.Add((int)QuestNum.Fifth,
			new QuestData("드러난 마약", new string[] { "피자가게 직원이 얘기한 노숙자를 찾아 대화하기", "펍으로 들어가 바텐더와 대화하기" },
				new int[] { (int)ObjectNum.NPC5, (int)ObjectNum.NPC5 }));
		_questDate.Add((int)QuestNum.Sixth,
			new QuestData("가려진 배후", new string[] { "펍으로 들어가 바텐더와 대화하기", "죽은 남자의 부인을 추궁해 검거하기" },
				new int[] { (int)ObjectNum.NPC6, (int)ObjectNum.NPC6  }));
		_questDate.Add((int)QuestNum.Seventh,
			new QuestData("진실은 오직 하나!", new string[] { "Clear!", "" },
				new int[] { (int)ObjectNum.NPC7, (int)ObjectNum.NPC7 }));
	}

    public int GetQuestDialogIndex()
    {
        return questId + questActionIndex;
    }

    public void CheckQuest(int id)
    {
        if (id == _questDate[questId].NpcId[questActionIndex])
        {
            QuestInfo = GetQuestInfo();
            if (QuestInfo == "Clear!")
            {
                timelineManager.Ending();
            }
            questActionIndex++;
        }
        if (questActionIndex == _questDate[questId].NpcId.Length)
        {
            NextQuest();
            QuestName = CheckQuest();
        }
    }
    
    public string CheckQuest()
    {
        return _questDate[questId].QuestName;
    }

    private void NextQuest()
    {
        questId += 100;
        questActionIndex = 0;
    }

    private string GetQuestInfo()
    {
        return _questDate[questId].QuestInfo[questActionIndex];
    }
}
