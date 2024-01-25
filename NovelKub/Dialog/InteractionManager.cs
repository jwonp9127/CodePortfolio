using TMPro;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public DialogManager DialogManager { get; private set; }
    public QuestManager QuestManager { get; private set; }

    [Header("DialogUI")]
    public GameObject dialogUI;
    public GameObject nameUI;
    public TMP_Text dialogName;
    public TMP_Text dialogText;

    [Header("QuestInfoUI")]
    public GameObject questInfoUI;
    public TMP_Text questNameText;
    public TMP_Text questInfoText;

    [Header("Etc")]
    public GameObject scanObject;
    public string dialogObject;
    public bool isAction;
    public int dialogIndex;
    public bool isDialogObject;

    [Header("MiniGame")]
    public GameObject miniGameUI;
    public TMP_Text miniGameNameText;
    public TMP_Text miniGameText;
    // public bool IsMiniGame;

    public static InteractionManager instance;


    private void Awake()
    {
        instance = this;
        DialogManager = GetComponent<DialogManager>();
        QuestManager = GetComponent<QuestManager>();
    }

    private void Start()
    {
        isDialogObject = false;
        miniGameUI.SetActive(false);
        Debug.Log(QuestManager.CheckQuest());
        questInfoUI.SetActive(true);
        dialogUI.SetActive(false);
        ShowQuestInfo("탐정의 등장", "밥 경관과의 대화를 통해 상황파악 하기");
    }

    public void Interaction(GameObject obj)
    {
        scanObject = obj;
        ObjectData scanObjectData = scanObject.GetComponent<ObjectData>();
        Dialog(scanObjectData.objectId, scanObjectData.withQuest);
    }
    
    private void Dialog(int objectId, bool withQuest)
    {
        //if (DialogManager.IsMiniGame)
        //{
        //    Debug.Log("이곳은 못가");
        //    dialogText.text = "퀘스트를 먼저 진행해주세요";
        //    //ExitDialog(out dialogIndex);
        //    dialogUI.SetActive(DialogManager.IsMiniGame);
        //    DialogManager.IsMiniGame = !DialogManager.IsMiniGame;
        //    return;
        //}


        int questDialogIndex = QuestManager.GetQuestDialogIndex();
        string dialogData = DialogManager.GetDialog(objectId + questDialogIndex, dialogIndex, out dialogObject);
        Debug.Log(objectId + questDialogIndex);
        Debug.Log(dialogIndex);

        CheckAddItem(objectId + questDialogIndex, QuestManager.questActionIndex, dialogIndex);
        CheckMiniGame(objectId + questDialogIndex, QuestManager.questActionIndex, dialogIndex);
        if (DialogManager.IsMiniGame)
        {
            Debug.Log("이곳은 못가");
            dialogText.text = "퀘스트를 먼저 진행해주세요";
            //ExitDialog(out dialogIndex);
            dialogUI.SetActive(DialogManager.IsMiniGame);
            DialogManager.IsMiniGame = !DialogManager.IsMiniGame;
            return;
        }


        if (dialogData == null)
        {
            QuestManager.CheckQuest(objectId);
            ShowQuestInfo(QuestManager.QuestName, QuestManager.QuestInfo);
            ExitDialog(out dialogIndex);
            return;
        }

        if (withQuest)
        {
            dialogText.text = dialogData;

            nameUI.SetActive(true);
        }
        else
        {
            dialogText.text = dialogData;

            nameUI.SetActive(false);
        }
        OnDialog();
        if (!DialogManager.IsMiniGame)
        {
            dialogIndex++;
        }
    }

    private void OnDialog()
    {
        isAction = true;
        dialogUI.SetActive(true);
        ShowDialogName(dialogObject);
    }

    public void ExitDialog(out int index)
    {
        isAction = false;
        dialogUI.SetActive(false);
        index = 0;
    }

    private void ShowQuestInfo(string qName, string qInfo)
    {
        questNameText.text = qName;
        questInfoText.text = qInfo;
    }

    private void ShowDialogName(string dName)
    {
        if (dName == "player")
        {
            nameUI.SetActive(false);
        }
        else
        {
            nameUI.SetActive(true);
        }
        dialogName.text = dName;
    }

    public void CheckAddItem(int Questid, int npcIdex, int talkIndex)
    {
        for (int i = 0; i < 7; i++)
        {
            if (Questid == DialogManager._QuestItem[i, 0]  && talkIndex == DialogManager._QuestItem[i, 2])
            {
                Inventory.instance.AddItem(DialogManager.QuestItemDatas[DialogManager._QuestItem[i, 3]]);
                Debug.Log(DialogManager._QuestItem[i, 3] + "ADD인벤토리하기");
            }
        }
    }
    //&& npcIdex == DialogManager._QuestItem[i, 1]
    public void CheckMiniGame(int questid, int npcIndex, int talkIndex)
    {
        for (int i = 0; i <4; i++) //이거 2라고 써있는 것은 바꿔야 한당.
        {
            if (questid == DialogManager._MiniGame[i, 0]  && talkIndex == DialogManager._MiniGame[i, 2])
            {
                Debug.Log(DialogManager._MiniGame[i, 0]);
                Debug.Log("캐릭터 인데스" + npcIndex);
                Debug.Log(DialogManager._MiniGame[i, 1]);
                Debug.Log(talkIndex);
                Debug.Log(DialogManager._MiniGame[i, 2]);

                Debug.Log("시작");
                DialogManager.IsMiniGame = true;
                switch (i)
                {
                    case (int)QuestMiniGame.First:
                        OnMiniGame("중식당 사장", "중식당 사장을 도와 식당을 도와주자");
                        break;
                    case (int)QuestMiniGame.Second:
                        OnMiniGame("할아버지", "쓰레기 도와주기");
                        break;
                    case (int)QuestMiniGame.Third:
                        OnMiniGame("피자가게", "피자가게 도와주기");
                        break;
                    case (int)QuestMiniGame.Forth:
                        OnMiniGame("노숙자 도와주기", "노숙자의 마약 찾아주기");
                        break;
                }

                //Debug.Log(DialogManager._QuestItem[i, 3] + "ADD인벤토리하기");
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            FinishMiniGame();
        }
    }

    public void FinishMiniGame()
    {
        // 각자의 미니게임에서 이메서드를 통해 미니 게임이 끝난 것을 알려주세요
        DialogManager.IsMiniGame = false;
        dialogIndex++;
        OffMiniGame();
    }

    public void OnMiniGame(string mininame, string miniContent)
    {
        questInfoUI.SetActive(false);
        miniGameUI.SetActive(true);
        miniGameNameText.text = mininame;
        miniGameText.text = miniContent;
    }

    public void OffMiniGame()
    {
        questInfoUI.SetActive(true);
        miniGameUI.SetActive(false);
        miniGameNameText.text = "";
        miniGameText.text = "";
    }

}
