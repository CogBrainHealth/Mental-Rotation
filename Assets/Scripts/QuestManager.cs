using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    GameManager gm;
    TableGenerator tg;

    public TableController tableEx;
    public TableController table1;
    public TableController table2;
    public TableController table3;

    //Test Data
    public bool pilotFlag = true;

    List<Stage> pilotTableData = new List<Stage>();

    //GameData
    public int totalStageNum = 1;
    private int thisStageNum = 0;

    private int answer = 0;

    public static QuestManager Instance { get; private set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }

    public void Start()
    {
        gm = GameManager.Instance;
        tg = TableGenerator.Instance;

        PilotDataSetting(); //문제 할당
    }

    //게임 세팅
    public void Game()
    {
        if (thisStageNum < totalStageNum) //정해진 스테이지 수보다 적으면 문제 생성
        {
            createStage();
            thisStageNum++;
        }
        else
        {
            gm.GameOver();
        }

    }

    private void createStage()
    {
        Debug.Log(thisStageNum + "번째 Stage 시작");
    }

    private void PilotDataSetting() 
    {
        Stage s1 = new Stage(1, new int[] { 0, 0, 0, 0 }, new int[] { 1, 1, 1, 1 }, new int[] { 2, 2, 2, 2 }, new int[] { 3, 3, 3, 3 });

        pilotTableData.Add(s1);
    }

    //파일럿 게임 세팅
    public void pilotTest() //게임 시작 and 다음 게임
    {
        if (thisStageNum < totalStageNum)
        {
            pilotStage(thisStageNum++);
        }
        else
        {
            gm.GameOver();
        }
    }

    private void pilotStage(int stageNum) //이번 게임 세팅
    {
        Debug.Log(stageNum + "번째 스테이지 추출");

        Stage thisStage = pilotTableData[stageNum];

        tg.PilotTableGenerate(tableEx, thisStage.exam);
        tg.PilotTableGenerate(table1, thisStage.table1);
        tg.PilotTableGenerate(table2, thisStage.table2);
        tg.PilotTableGenerate(table3, thisStage.table3);
    }

    //답안 선택

    public void choice(int i)
    {
        if (i == answer)
        {
            Debug.Log("정답");
        }
        else
        {
            Debug.Log("오답");
        }

        pilotTest();
    }
}

//pilot 더미 데이터용
public class Stage
{
    public int[] exam { get; set; }
    public int[] table1 { get; set; }
    public int[] table2 { get; set; }
    public int[] table3 { get; set; }

    int answer;

    public Stage(int a, int[] e, int[] t1, int[] t2, int[] t3)
    {
        answer = a;

        exam = e;
        table1 = t1;
        table2 = t2;
        table3 = t3;
    }
}
