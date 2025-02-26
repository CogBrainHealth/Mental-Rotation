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
    List<int> shuffledStageTypes = new List<int>(); // Random stageType

    //GameData
    public int totalStageNum;
    public TextMeshProUGUI stageNumber; // StageNum UI

    //private List<GameObject> clonedObjects = new List<GameObject>(); //Rotation Table of Ex (Answer List)

    private TableController answerTable;
    private int thisStageNum = 1; //Start at 1 stage
    private int answer; 
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

        //PilotDataSetting(); // Allocate quest Data
    }

    public void Update()
    {
        time += Time.deltaTime;
    }

    //Game Setting
    public void Game()
    {
        if (thisStageNum <= totalStageNum) //Quest Count Check
        {
            createStage();
            thisStageNum++;
        }
        else
        {
            gm.GameOver();
        }
    }

    //Game Setting
    private void createStage()
    {
        //stageNumber UI
        stageNumber.text = $"{thisStageNum} / {totalStageNum} ";

        //Random Choice Stage Info
        int spriteNum = Random.Range(3, 5);
        tg.SettingSpritePool(spriteNum);

        //create Ex Table
        tg.TableGenerate(tableEx);

        //select correct answer number
        answer = Random.Range(0, 3);

        //답안 테이블 구성
        bool last = false;

        for (int i = 0; i < 3; i++)
        {
            if (answer == i) //정답 테이블의 경우 
            {
                if (answerTable != null)
                    Destroy(answerTable.gameObject);
                answerTable = Instantiate(tableEx, tableEx.transform.parent);
                answerTable.RotateTable(Random.Range(0, 3));
                answerTable.transform.position = new Vector3(-1.3f + (1.3f * i), -3.3f, 0);
            }
            else //오답 테이블의 경우
            {
                table[i].gameObject.SetActive(true);

                if (!last)
                {
                    last = true;

                    do
                    {
                        tg.TableGenerate(table[i]);
                    }
                    while (table[i].CompareTable(tableEx)); // if it is in answerArray
                }
                else
                {
                    last = false;

                    int j = (answer == 0) ? 1 : 0; //선행 오답의 인덱스
                    //Debug.Log($"answer: {answer} / 선행 오답: {j}");
                    do
                    {
                        tg.TableGenerate(table[i]);
                    }
                    while (table[i].CompareTable(tableEx) || table[i].CompareTable(table[j])); // if it is in answerArray
                }
            }
        }

        time = 0f;
    }

    //답안 선택
    public void choice(int n)
    {
        bool correct;

        //all active false
        for (int i = 0; i < 3; i++)
        {
            //Debug.Log(i + "th setActive false");
            table[i].gameObject.SetActive(false);
        }

        if (n == answer)
        {
            Debug.Log("정답");
            correct = true;
        }
        else
        {
            Debug.Log("오답");
            correct = false;
        }

        gm.StoreScore(new StageScore(thisStageNum, time, correct));
        Game(); // Next Stage Call
    }

    ////////////////////// Pilot //////////////////////
    //PilotGame Setting
    //private void PilotDataSetting()
    //{
    //    totalStageNum = 10;

    //    // shuffle stageType
    //    List<int> stageTypeList = new List<int>(pilotStageType);

    //    while (stageTypeList.Count > 0)
    //    {
    //        Debug.Log($"stageTypeList.Count: {stageTypeList.Count}");
    //        int index = Random.Range(0, stageTypeList.Count);
    //        shuffledStageTypes.Add(stageTypeList[index]);
    //        stageTypeList.RemoveAt(index);
    //    }
    //}

    //// Game Start and Next Game
    //public void pilotTest()
    //{
    //    if (thisStageNum < totalStageNum) //Quest Count Check
    //    {
    //        pilotStage(thisStageNum++);
    //    }
    //    else
    //    {
    //        gm.GameOver();
    //    }
    //}

    ////this Stage Setting
    //private void pilotStage(int stageNum)
    //{
    //    // Clean existed cloned object
    //    foreach (GameObject obj in clonedObjects)
    //    {
    //        Destroy(obj);
    //    }
    //    clonedObjects.Clear();

    //    // stageNumber UI
    //    stageNumber.text = $"{thisStageNum} / {totalStageNum} ";

    //    // select stageType
    //    int stageType = shuffledStageTypes[thisStageNum - 1]; // index니까
    //    tg.TableGeneratePilot(tableEx, stageType, true); // TableEx

    //    Debug.Log($"stageType: {stageType}");

    //    // Rotation Table of Ex (Answer List)
    //    TableController[] answerArray = new TableController[3];
    //    for (int i = 0; i < 3; i++) // rotation 90, 180, 270
    //    {
    //        //generate answer array
    //        TableController rotatedTable = Instantiate(tableEx, tableEx.transform.parent);
    //        rotatedTable.RotateTable(i);
    //        answerArray[i] = rotatedTable;

    //        // add in cloned object list
    //        clonedObjects.Add(rotatedTable.gameObject);
    //    }

    //    //select correct answer number
    //    answer = Random.Range(0, 3);
    //    Debug.Log($"정답: {answer + 1}");

    //    //generate choice tables
    //    for (int i = 0; i < 3; i++)
    //    {
    //        if (i == answer) //correct answer
    //        {
    //            //정답 테이블 선지로 이동
    //            int correctAnswerIndex = tg.RotateAngle(stageType);

    //            TableController answerTable = answerArray[correctAnswerIndex];
    //            answerTable.transform.position = new Vector3(-1.3f + (1.3f * i), -3.3f, 0);
    //        }
    //        else //incorrect answer
    //        {
    //            table[i].gameObject.SetActive(true);
    //            do
    //            {
    //                tg.TableGeneratePilot(table[i], stageType, false);
    //            }
    //            while ( table[i].CompareTable(tableEx) ||
    //                    (i > 0 && answer != 0 && table[i].CompareTable(table[0])) ||
    //                    (i == 2 && answer != 1 && table[i].CompareTable(table[1]))
    //                   ); // 보기 및 이미 생성된 오답 선지와 같으면 다시 생성
    //        }
    //    }

    //    time = 0f;
    //}
    ////////////////////// Pilot End //////////////////////
}
