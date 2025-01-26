using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class QuestManager : MonoBehaviour
{
    GameManager gm;
    TableGenerator tg;

    public TableController tableEx;
    public TableController[] table = new TableController[3];

    //Test Data
    public bool pilotFlag = true;

    List<Stage> pilotTableData = new List<Stage>();

    //GameData
    public int totalStageNum;
    public TextMeshProUGUI stageNumber;

    private int thisStageNum = 1; //???????? ?????? 1???? ??????????!
    private int answer = 0;
    private float time = 0f;

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

        PilotDataSetting(); //???? ????
    }

    public void Update()
    {
        time += Time.deltaTime;
    }

    //???? ????
    public void Game()
    {
        if (thisStageNum <= totalStageNum) //?????? ???????? ?????? ?????? ???? ????
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
        Debug.Log(thisStageNum + "???? Stage ????");
        // stageNumber.text = $"{thisStageNum-1} / {totalStageNum} ";

        //???? ???????? ???????? ????
        tg.TableGenerate(tableEx);

        //???? ???? ????
        answer = Random.Range(0, 3);

        //???? ?????? ????
        for (int i = 0; i < 3; i++)
        {
            if (answer == i) //???? ???????? ???? 
            {
                //table[i] <=  tableEx?? ???? ?? ???? ????
            }
            else //???? ???????? ????
            {
                do 
                    tg.TableGenerate(table[i]);
                while(false); // tableEx?? answer?????? ?????????? ????
            }
        }

        time = 0f;
    }

    //?????? ???? ????
    private void PilotDataSetting() 
    {
        Stage s1 = new Stage(0, new int[] { 0, 0, 0, 0 }, new int[] { 1, 1, 1, 1 }, new int[] { 2, 2, 2, 2 }, new int[] { 3, 3, 3, 3 });

        pilotTableData.Add(s1);

        //totalStageNum = pilotTableData.Count;
        totalStageNum = 8;
    }

    public void pilotTest() //???? ???? and ???? ????
    {
        if (thisStageNum <= totalStageNum)
        {
            pilotStage(thisStageNum++);
        }
        else
        {
            gm.GameOver();
        }
    }

    private void pilotStage(int stageNum) //???? ???? ????
    {
        Debug.Log(stageNum + "???? ???????? ????");
        stageNumber.text = $"{thisStageNum-1} / {totalStageNum} ";
        if (stageNum > pilotTableData.Count) return; //?????? ???????? ?? ????
        Stage thisStage = pilotTableData[stageNum-1]; //?????? ???????? ????. ?????????? -1

        //???? ????
        answer = thisStage.answer;

        tg.PilotTableGenerate(tableEx, thisStage.exam);
        tg.PilotTableGenerate(table[0], thisStage.table1);
        tg.PilotTableGenerate(table[1], thisStage.table2);
        tg.PilotTableGenerate(table[2], thisStage.table3);

        time = 0f;
    }

    //???? ????
    public void choice(int i)
    {
        bool correct;

        if (i == answer)
        {
            Debug.Log("????");
            correct = true;
        }
        else
        {
            Debug.Log("????");
            correct=false;
        }

        gm.StoreScore(new StageScore(thisStageNum, time, correct));
        pilotTest();
    }
}

//pilot ???? ????????
public class Stage
{
    public int[] exam { get; set; }
    public int[] table1 { get; set; }
    public int[] table2 { get; set; }
    public int[] table3 { get; set; }

    public int answer;

    public Stage(int a, int[] e, int[] t1, int[] t2, int[] t3)
    {
        answer = a;

        exam = e;
        table1 = t1;
        table2 = t2;
        table3 = t3;
    }
}
