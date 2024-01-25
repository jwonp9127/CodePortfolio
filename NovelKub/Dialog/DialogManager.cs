using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    private QuestManager _questManager;
    private Dictionary<int, string[]> _dialogData;
    private ObjectNum _objectNum;

    public int[,] _QuestItem;
    public int[,] _MiniGame;
    public ItemData[] QuestItemDatas = new ItemData[8]; //추가
    public bool IsMiniGame;

    private void Awake()
    {
        _questManager = GetComponent<QuestManager>();
        _dialogData = new Dictionary<int, string[]>();
        GenerateDialogData();
        GenerateQuestDialogData();
        _QuestItem  = new int[8, 4] { { (int)ObjectNum.NPC1 + (int)QuestNum.First, 0, 13, 0 }, { (int)ObjectNum.NPC2 + (int)QuestNum.Second, 0, 5, 1 }, { (int)ObjectNum.NPC2 + (int)QuestNum.Second + 1, 0, 10, 2 }, { (int)ObjectNum.InteractableObject1 + (int)QuestNum.Third, 0, 4,3 }, { (int)ObjectNum.NPC3 + (int)QuestNum.Third + 2, 0, 5, 4 }, { (int)ObjectNum.NPC4 + (int)QuestNum.Forth + 1, 0, 10, 5 }, { (int)ObjectNum.NPC6 + (int)QuestNum.Sixth + 1, 0, 8, 6 }, { (int)ObjectNum.NPC7 + (int)QuestNum.Seventh + 1, 0, 4, 7 } }; // 어는 부분에 미니게임을 시작할지 넣는 부분
        _MiniGame = new int[5, 3] { { 2200, 0, 5 }, {3301 , 0, 5 }, { 4400, 0, 8 }, { 5500, 0, 7 }, { 5501, 0, 10 } };
    }

    private void GenerateDialogData()
    {
        _dialogData.Add((int)ObjectNum.NPC1, new string[]{"탐정님 정말 반갑습니다!:1"});
        _dialogData.Add((int)ObjectNum.NPC2, new string[]{"탐정양반! 일자리가 필요하다면 언제나 오게나!:1"});
		_dialogData.Add((int)ObjectNum.NPC3, new string[]{"우리 동네에 살인사건이라니..에잉 쯧쯧:1"});
		_dialogData.Add((int)ObjectNum.NPC4, new string[]{"손님들이 그만 좀 왔으면 좋겠네요:1"});
		_dialogData.Add((int)ObjectNum.NPC5, new string[]{"이..이제 해..행복할 수 있어!:1" });
		_dialogData.Add((int)ObjectNum.NPC6, new string[]{"쓰디쓴 술엔 달콤한 사탕이 어울리죠:1" });
		_dialogData.Add((int)ObjectNum.NPC7, new string[]{"뭘 보죠?:1" });
		_dialogData.Add((int)ObjectNum.InteractableObject1, new string[]{"펍으로 가는 입구다.:0"});
    }

    private void GenerateQuestDialogData()
    {
        // 해당 obj + quest + order
        _dialogData.Add((int)ObjectNum.NPC1 + (int)QuestNum.First,
                        new string[] { "오! 탐정님 오랜만에 뵙는군요! 이쪽으로 오시죠.:1",              
                                       "밥 경관님 감사합니다. 오랜만이군요.:0",
									   "혹시 조사가 어느정도까지 진행되었습니까?:0",
									   "일단 피해자는 사업가고 40대 남성입니다. \n사망원인은 질식사로 보여집니다.:1",
									   "범행도구는 발견됐나요?:0",
									   "아뇨, 범행도구는 현장에선 발견되지 않았습니다.:1",
									   "사망추정시간은 어제 오후 22시에서 23시 사이로 보고있습니다.:1",
									   "언제나 고마워요 밥.:0",
									   "천만에요. 이번에도 저번처럼 멋지게 해결해주실거라 믿고 있습니다.:1",
									   "모두가 다 밥 경관님처럼 생각하진 않는 것 같군요...:0",
									   "(다른 경찰들의 따가운 시선이 느껴진다):0",
									   "아직 다들 탐정님을 경험해보지 못해서 그럴테죠...:1",
									   "전 이만 조사하러 가보겠습니다. 그럼 고생하세요.:0",
									   "탐정님도 항상 조심하세요.\n새로운 사실이 나오면 말씀드리도록 하겠습니다.:1",
									   "(현장과 가까운 중식당부터 조사해보자):0"});
        _dialogData.Add((int)ObjectNum.NPC2 + (int)QuestNum.Second,
                        new string[] { "실례합니다. 혹시 사장님 계십니까?:0",
									   "바빠 죽겠는데 누구쇼? 처음 보는 얼굴인 것 같은데..:1",
									   "어제 발생한 살인사건을 조사하고 있는 사설탐정입니다.:0",
									   "탐정 양반이 여긴 무슨일로 왔수?:1",
									   "혹시 어제 22시에서 23시 사이에 호텔 근처에서 \n수상한 사람이나 수상한 물건같은거 보신적 있으십니까?.:0",
									   "글쎄.. 본 것 같기도 하고 아닌거 같기도 하고.. \n일 좀 도와주면 생각날거 같기도 한데...:1",
						               });
		//미니게임 구현
		_dialogData.Add((int)ObjectNum.NPC2 + (int)QuestNum.Second +1,
						new string[] { "탐정양반.. 우리 가게에서 일해볼 생각 없는가?.:1",
									   "자네 손이 정말 빠르구만.. 아주 탐나!!.:1",
									   "후.. 이제 좀 생각 나십니까?.:0",
									   "가만 보자.. 그러니까.. 그래 어제 가게 마감하고 문닫을 때, \n누군가 호텔에서 나오더구만.:1",
									   "어두워서 누군지는 못봤지만 호텔에서 나오는 건 분명히 봤네.:1",
									   "흠.. 한명뿐이었나요?:0",
									   "그치 누군지는 몰라도 한명인건 확실하네!:1",
									   "협조해주셔서 감사합니다.:0",
									   "바쁜데 일 도와줘서 내가 더 고맙지 뭐, \n아무튼 도움이 됐길 바라네.:1",
									   "(누군가 나오는게 보였다는 호텔 정문쪽으로 가보자):0"});

		_dialogData.Add((int)ObjectNum.InteractableObject1 + (int)QuestNum.Third,
						new string[] { "(호텔 앞을 조사하던중 그 곳을 비추고있는 CCTV를 발견했다):0",
									   "(CCTV는 가까운 펍에서 관리하는 듯 하다):0",
									   "아직 이른 시간이라 문이 굳게 닫혀있다.:0",
									   "(근처에 있는 할아버지에게 CCTV에 대해 물어보자):0" });

		_dialogData.Add((int)ObjectNum.NPC3 + (int)QuestNum.Third + 1,
						new string[] { "어르신, 실례지만 위에 CCTV 어디서 확인할 수 있는지 아시나요?:0",
									   "CCTV는 왜? 처음보는 얼굴이구만..:1",
									   "어제 있었던 살인사건을 조사하러 나온 사설탐정입니다.:0",
									   "이 곳에서 수상한 사람을 봤다는 사람이 있어서 \nCCTV로 확인해보려합니다.:0",
									   "젊은 양반이 예의는 좀 아는구먼, 큼큼:1",
									   "그러면 저쪽에 있는 쓰레기들 좀 깔끔하게 정리해줘봐 \n내가 나이가 먹어서 그런지 허리가 아파서 영~ 힘들구만:1" });
		//미니게임 구현
		_dialogData.Add((int)ObjectNum.NPC3 + (int)QuestNum.Third +2,
						new string[] { "이제 좀 깔끔해졌구만, 자네가 쓰레기를 정리하는동안 내가 핸드폰으로 CCTV좀 찾아봤네.:1",
									   "네? 어르신께서요?:0",
									   "그래 내가 설치한 CCTV니까 나 말곤 못 볼꺼야.:1",
									   "자 얼른 보고 범인좀 잡아주게 \n아침부터 동네가 시끄러워서 무릎이 쑤셔오는구먼..:1",
									   "(CCTV엔 호텔에서 나온 누군가가 피자가게로 곧장 들어가는 모습이 찍혀있었다):0",
									   "(피자가게로 가보자).:0"});

		_dialogData.Add((int)ObjectNum.NPC4 + (int)QuestNum.Forth,
						new string[] { "실례합니ㄷ..:0",
									   "곧 나옵니다! 조금만 기다려주세요 손님!!:1",
									   "(점심시간이라 그런지 피자가게에 손님이 너무 많다.. 설마 또..?:0",
									   "몇 분이세요?:1",
									   "저.. 먹으러 온건 아니ㄱ..:0",
									   "드시러 오신거 아니면 좀 만 기다리세요!:1",
									   "하.. 사람도 많아서 바빠 죽겠는데!:1",
									   "(직원은 뭐가 그리도 바쁜지 말도 다 마치기 전에 주방으로 바쁘게 걸어갔다):0",
									   "정 할거 없으시면 좀 도와주던가요!!:1"});

		//미니게임
		_dialogData.Add((int)ObjectNum.NPC4 + (int)QuestNum.Forth +1,
						new string[] { "후.. 오늘 왜이렇게 손님이 많은지.. \n아저씨 없었으면 죽을뻔했네요..:1",
									   "아, 아저씨 뭐 때문에 오셨다고 했었죠?:1",
									   "후.. 아저씨는 아니고, \n어제 일어난 살인사건을 조사하고있는 사설탐정입니다.:0",
									   "혹시 어제 오후 22시 30분 경에 들어온 손님 기억하나요?:0",
									   "어제요..? 아 기억나네요 23시에 마감해야하는데 \n22시 반에 들어와서 주문했던 아저씨 있었는데..:1",
									   "하.. 그 아저씨 괜히 와서 오븐스파게티 하나 꼴랑 시켜가지고 \n결국 어제 23시 반에 퇴근했다구요!:1",
									   "혹시 누군지 아시나요?:0",
									   "알죠! 출근할때마다 보는데, \n요 앞에 지하철역 출구에서 맨날 쭈그려 앉아있는 노숙자 아저씨에요.:1",
									   "아마 지금은 지하철역 출구에 있거나 아니면 근처 공원에 있을거에요.:1",
									   "(당신의 수사가 마음에 안드는 경찰들은 \n수사를 훼방놓기 위해 당신을 찾으러 다니고 있다):0",
									   "(돌아다니는 경찰들의 눈을 피해 지하쳘역 출구 주변과, \n공원에서 노숙자를 찾은뒤 대화를 진행하자.):0"});

		_dialogData.Add((int)ObjectNum.NPC5 + (int)QuestNum.Fifth,
						new string[] { "혹시 어젯밤에 저쪽 피자가게에 간적이 있으신가요?:0",
									   "그치그치 어제 내가 갔어! 갔지..:1",
									   "혹시 어제 드신게 오븐스파게티 맞나요? 22시 반쯤에..:0",
									   "혹시 어제 오후 22시 30분 경에 들어온 손님 기억하나요?:0",
									   "오븐 스파게티! 맛.. 맛있었지 그럼 그럼 \n스파게티는 거..거기가 최고야 최고:1",
									   "혹시 어제 호텔도 들리셨죠? 호텔에서 뭘 하고 나오셨나요?:0",
									   "ㄴ..내가 수..숨겨놓은 내꺼! \nㅇ..어제 숨겨놨는데 까..까먹었어 분명 이 주변 어디에 놔뒀었는데!!:1",
									   "그..그거 찾아주면 내가 마..말해줄께!:1"});

		//미니게임
		_dialogData.Add((int)ObjectNum.NPC5 + (int)QuestNum.Fifth +1,
						new string[] { "헤..헤헤 ㅈ..잘 찾아왔네! 이거야 이거!:1",
									   "이건... 마약 아닙니까?:0",
									   "쉬..쉬쉿!! 여..여기 주변에 겨..경찰 많아! 죠용히 해야지!!:1",
									   "아 네.. 근데 이건 어디서 나신겁니까?:0",
									   "호..호텔.. 어제 호텔에서 이..이거 사온거야.. \n마..마약상이랑 거..거기서 거래 했거든 ..거..거래 헤헤:1",
									   "그럼 혹시...:0",
									   "아..아니야!! 어젠 이..이거 하지도 않..않았다고! \n그..급하게 숨겨놓느라 하지도 모..못했는데..:1",
									   "아 마..마약상 저..저기 펍..그래 펍! \n거..거기서 바..바텐더로 일하던..그..그래 그남자야!:1", 
									   "나한테 그..그거 판거 그 바..바텐더라고!!:1",
									   "(아무래도 더이상 노숙자와의 대화는 힘들것 같다. \n노숙자의 말에 신뢰가 가진 않지만 우선 마약을 판매한 건 바텐더일수도 있을 것 같다):0",
									   "(곧 저녁이니 펍이 열었을 수도 있다. 펍에 가서 바텐더와 대화해보자):0"});

		_dialogData.Add((int)ObjectNum.NPC6 + (int)QuestNum.Sixth,
						new string[] { "실례합니다.:0",
									   "어서오세요. 어떤걸로 드릴까요?:1",
									   "아. 손님은 아니고 어젯밤에 일어난 살인사건을 조사하고있는 사설탐정입니다. \n잠깐 시간 괜찮으십니까?:0",
									   "저희 지금 영업중이라 개인적으로 시간을 내기는 힘듭니다. \n영업이 끝나고 와주시겠어요?:1",
									   "어제 오후 22시30분 경 호텔 안에서 노숙자 한명에게 마약 판매 하셨죠?:0",
									   "제가요? 허 어디서 정신나간 노숙자한테 \n무슨 소리를 듣고 오신지는 몰라도 제가요? 마약?:1", 
									   "전 그런 불법적인거랑은 거리가 먼 선량한 바텐더입니다! 무례하시군요:1",
									   "(바텐더의 입꼬리가 살짝 떨리는것 같았지만 \n이대로 증거 없이는 끝까지 발뺌할 것 같다..):0",
									   "(펍 안에서 바텐더가 마약을 판매했다는 증거를 찾아서 다시 말을 걸어보자):0"});

		//미니게임
		_dialogData.Add((int)ObjectNum.NPC6 + (int)QuestNum.Sixth +1,
						new string[] { "또 오셨군요. 이번엔 어떤거 때문에 그러시죠?:1",
									   "//암구호를 말한다:0",
									   "다..당신이 그걸 어떻게....후우...그래요.. 맞아요 제가 팔았습니다 그 노숙자한테!:1",
									   "근데 저도 그거 부탁받고 판매한거라고요!!:1",
									   "부탁이요? 누구한테서 부탁받은거죠?:0",
									   "그..그건.. 하.. 어젯밤에 죽었다던 그 사업가의 부인한테 부탁받았습니다.. \n젠장 이렇게 큰 사건하고 엮일 줄 알았다면 이딴 부탁 받지 말았어야했는데!!:1",
									   "후... 전 그 살인사건이랑 전혀 관련없어요 아마.. \n그 여자일꺼에요 죽은 남자의 부인!! 나는 이용당한것 뿐이라고요!!!:1",
									   "(바텐더는 넋이 나가 더 이상 대화가 어려울 것 같다.):0",
									   "(바텐더의 말에 따라 죽은 남자의 부인을 찾아가 추궁을 해보자.):0"});

		_dialogData.Add((int)ObjectNum.NPC7 + (int)QuestNum.Seventh,
						new string[] { "(어딘가 급해보이는 부인이 호텔을 나서고 있다):0",
									   "누..누구시죠?:1",
									   "그래서! 그게 내가 남편을 죽였다는 증거가 되나요?:1" , 
									   "지금 그게 남편을 하루 아침에 잃은 사람한테 할 소리인가요?:1", 
									   "저는 사건 당일 남편이 자는 걸 보고 혼자 외출을 했었어요:1",
									   "전날 남편이 저녁 8시~9시경 술을 먹고 들어와서 \n뻗어있는 채로 있길래 냅두고 외출했다구요!:1",
									   "(경찰에게 받은 단서를 보여주며) \n남편분은 저녁 10시 쯤이 피해 추정 시각입니다:0",
									   "부인이 외출했던 시각 이후에 죽었다는 거죠:0",
									   "그래요! 그럼 나는 당연히 용의자에서 제외되는 것 아닌가요!:1",
									   "하지만 살해시각에 호텔에서 누군가 나왔다는 진술이 있었습니다:0",
									   "그리고 그 사람이 피자가게로 들어가는 장면이 \n호텔 앞 건물에 있는 cctv에 확인되었습니다:0",
									   "그..그게 저랑 무슨 상관인 거죠? 전 그 시각 전부터 나가있었다구요!:1",
									   "피자가게에 들어온 사람은 다름 아닌 노숙자더군요. \n혹시 이분을 아시나요?:0",
									   "제가 그런 더럽고 근본 없는 사람을 알 리가 없잖아요!:1",
									   "그렇군요. 그럼 혹시 노숙자가 여기 펍의 바텐더 분과 \n마약거래를 했다는 것도 모른다고 말씀하실 건가요?:0",
									   "마약이요? 허, 제가 지금 마약에 연루가 되어 있다는 건가요?!:1",
									   "바텐더의 말로는 당신의 부탁을 받고 호텔에서 거래를 했다더군요:0",
									   "이..이런...입이 가벼운 사람들 같으니!:1"});

		
	}



    public string GetDialog(int objectId, int dialogIndex, out string dialogObject)
    {
        if (!_dialogData.ContainsKey(objectId))
        {
            if (!_dialogData.ContainsKey(objectId - objectId % 100))
            {
                return GetDialog(objectId - objectId % 1000, dialogIndex, out dialogObject);
            }
            else
            {
                return GetDialog(objectId - objectId % 100, dialogIndex, out dialogObject);
            }
        }
        if (dialogIndex == _dialogData[objectId].Length)
        {
	        dialogObject = null;
            return null;
        }
        else
        {
	        string[] dialog = _dialogData[objectId][dialogIndex].Split(':');
	        dialogObject = GetDialogObject(objectId - objectId % 1000, int.Parse(dialog[1]));
            return dialog[0];
        }
    }

    private string GetDialogObject(int objectId, int dialogTag)
    {
	    if (dialogTag == 0)
	    {
		    return "player";
	    }

	    switch ( objectId )
	    {
		    case (int)ObjectNum.NPC1:
			    return "밥 경관";
		    case (int)ObjectNum.NPC2:
			    return "중식당 사장";
		    case (int)ObjectNum.NPC3:
			    return "건물주";
		    case (int)ObjectNum.NPC4:
			    return "피자가게 직원";
		    case (int)ObjectNum.NPC5:
			    return "노숙자";
		    case (int)ObjectNum.NPC6:
			    return "바텐더";
		    case (int)ObjectNum.NPC7:
			    return "부인";
		    case (int)ObjectNum.InteractableObject1:
			    return "Pub";
		    default:
			    return "누구야";
	    }
    }
}
