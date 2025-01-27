using UnityEngine;
using System.Collections.Generic;

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

    private int thisStageNum = 1; //스테이지 넘버만 1부터 시작합니다!
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

        PilotDataSetting(); //문제 할당
    }

    public void Update()
    {
        time += Time.deltaTime;
    }

    //게임 세팅
    public void Game()
    {
        if (thisStageNum <= totalStageNum) //정해진 스테이지 수보다 적으면 문제 생성
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

        //보기 테이블은 랜덤으로 구성
        tg.TableGenerate(tableEx);

        //정답 번호 선택
        answer = Random.Range(0, 3);

        //답안 테이블 구성
        for (int i = 0; i < 3; i++)
        {
            if (answer == i) //정답 테이블의 경우 
            {
                //table[i] <=  tableEx의 회전 중 하나 선택
            }
            else //오답 테이블의 경우
            {
                do 
                    tg.TableGenerate(table[i]);
                while(false); // tableEx의 answer배열에 포함되는지 확인
            }
        }

        time = 0f;
    }

    //파일럿 게임 세팅
    private void PilotDataSetting() 
    {
        Stage s1 = new Stage(0, new int[] { 0, 0, 0, 0 }, new int[] { 1, 1, 1, 1 }, new int[] { 2, 2, 2, 2 }, new int[] { 3, 3, 3, 3 });

        pilotTableData.Add(s1);

        totalStageNum = pilotTableData.Count;
    }

    public void pilotTest() //게임 시작 and 다음 게임
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

    private void pilotStage(int stageNum) //이번 게임 세팅
    {
        Debug.Log(stageNum + "번째 스테이지 추출");
        if (stageNum > pilotTableData.Count) return; //파일럿 스테이지 수 초과
        Stage thisStage = pilotTableData[stageNum-1]; //파일럿 스테이지 추출. 인덱스라서 -1

        //문제 구성
        answer = thisStage.answer;

        tg.PilotTableGenerate(tableEx, thisStage.exam);
        tg.PilotTableGenerate(table[0], thisStage.table1);
        tg.PilotTableGenerate(table[1], thisStage.table2);
        tg.PilotTableGenerate(table[2], thisStage.table3);

        time = 0f;
    }

    //답안 선택
    public void choice(int i)
    {
        bool correct;

        if (i == answer)
        {
            Debug.Log("정답");
            correct = true;
        }
        else
        {
            Debug.Log("오답");
            correct=false;
        }

        gm.StoreScore(new StageScore(thisStageNum, time, correct));
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
