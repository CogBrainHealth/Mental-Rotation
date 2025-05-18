using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

using Random = UnityEngine.Random;

public class QuestManager : MonoBehaviour
{
    GameManager gm;
    TableGenerator tg;

    public TableController tableEx;
    public TableController[] table = new TableController[3];

    //TestData
    public bool pilotFlag = true;
    public int[] pilotStageType; // stageType
    public List<int> shuffledStageTypes = new List<int>(); // Random stageType

    //GameData
    public int totalStageNum;
    
    public TextMeshProUGUI stageNumber; // StageNum UI

    private List<GameObject> clonedObjects = new List<GameObject>(); //Rotation Table of Ex (Answer List)

    private TableController answerTable;
    
    public int thisStageNum = 0;
    private int answer; 
    private float time = 0f;

    public Timer timer;

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

        PilotDataSetting(); // Allocate quest Data
    }

    public void Update()
    {
        time += Time.deltaTime;
    }
    
    //답안 선택
    public void choice(int n)
    {
        bool correct;
        Debug.Log("남은 시간:" + time);

        //all active false
        for (int i = 0; i < 3; i++)
        {
            //Debug.Log(i + "th setActive false");
            table[i].gameObject.SetActive(false);
        }

        if (n == answer)
        {
            Debug.Log(thisStageNum + "번: 정답");
            correct = true;
        }
        else
        {
            Debug.Log(thisStageNum + "번: 오답");
            correct = false;
        }

        gm.StoreScore(new StageScore(thisStageNum, time, correct));
        //Game(); // Next Stage Call
        pilotTest();
    }

    ////////////////////// Pilot //////////////////////
    //PilotGame Setting
    private void PilotDataSetting()
    {
        totalStageNum = 10;

        // shuffle stageType
        List<int> stageTypeList = new List<int>(pilotStageType);

        while (stageTypeList.Count > 0)
        {
            // Debug.Log($"stageTypeList.Count: {stageTypeList.Count}");
            int index = Random.Range(0, stageTypeList.Count);
            shuffledStageTypes.Add(stageTypeList[index]);
            stageTypeList.RemoveAt(index);
        }
    }

    // Game Start and Next Game
    public void pilotTest()
    {
        if (thisStageNum < totalStageNum) //Quest Count Check
        {
            pilotStage(thisStageNum++);
        }
        else
        {
            gm.GameOver();
        }
    }

    //this Stage Setting
    private void pilotStage(int stageNum)
    {
        timer.StartTimer();

        // Clean existed cloned object
        foreach (GameObject obj in clonedObjects)
        {
            Destroy(obj);
        }

        clonedObjects.Clear();

        // stageNumber UI
        stageNumber.text = $"{thisStageNum} / {totalStageNum} ";

        // select stageType
        int stageType = shuffledStageTypes[thisStageNum - 1]; // index니까
        tg.TableGeneratePilot(tableEx, stageType, true); // TableEx

        // Debug.Log($"stageType: {stageType}");

        // Rotation Table of Ex (Answer List)
        TableController[] answerArray = new TableController[3];
        for (int i = 0; i < 3; i++) // rotation 90, 180, 270
        {
            //generate answer array
            TableController rotatedTable = Instantiate(tableEx, tableEx.transform.parent);
            rotatedTable.RotateTable(i);
            answerArray[i] = rotatedTable;

            // add in cloned object list
            clonedObjects.Add(rotatedTable.gameObject);
        }

        //select correct answer number
        answer = Random.Range(0, 3);
        // Debug.Log($"정답: {answer + 1}");

        //generate choice tables
        for (int i = 0; i < 3; i++)
        {
            if (i == answer) //correct answer
            {
                //정답 테이블 선지로 이동
                int correctAnswerIndex = tg.RotateAngle(stageType);

                TableController answerTable = answerArray[correctAnswerIndex];
                answerTable.transform.position = new Vector3(-1.3f + (1.3f * i), -3.3f, 0);
            }
            else //incorrect answer
            {
                table[i].gameObject.SetActive(true);
                do
                {
                    tg.TableGeneratePilot(table[i], stageType, false);
                } while (table[i].CompareTable(tableEx) ||
                         (i > 0 && answer != 0 && table[i].CompareTable(table[0])) ||
                         (i == 2 && answer != 1 && table[i].CompareTable(table[1]))
                        ); // 보기 및 이미 생성된 오답 선지와 같으면 다시 생성
            }
        }

        time = 0f;
    }
}
