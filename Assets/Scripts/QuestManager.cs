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

    private List<GameObject> clonedObjects = new List<GameObject>(); //Rotation Table of Ex (Answer List)

    private int thisStageNum = 1; //Start at 1 stage
    private int answer = 0; 
    private float time = 0f;

    public static QuestManager Instance { get; private set; }

    //-------------------Method-------------------
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

    //Game Setting - 미완(Uncomplete)
    private void createStage()
    {
        // Clean existed cloned object
        foreach (GameObject obj in clonedObjects)
        {
            Destroy(obj);
        }
        clonedObjects.Clear();

        //stageNumber UI
        stageNumber.text = $"{thisStageNum-1} / {totalStageNum} ";

        //create Ex Table
        tg.TableGenerate(tableEx);

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

        //답안 테이블 구성
        for (int i = 0; i < 3; i++)
        {
            if (answer == i) //정답 테이블의 경우 
            {
                TableController answerTable = answerArray[Random.Range(0, 3)];
                answerTable.transform.position = new Vector3(-1.3f + (1.3f * i), -3.3f, 0);
            }
            else //오답 테이블의 경우
            {
                table[i].gameObject.SetActive(true);
                do
                {
                    tg.TableGenerate(table[i]);
                }
                while (table[i].CompareTable(tableEx)); // if it is in answerArray
            }
        }

        time = 0f;
    }

    //PilotGame Setting
    private void PilotDataSetting()
    {
        totalStageNum = 8;

        // shuffle stageType
        List<int> stageTypeList = new List<int>(pilotStageType);

        while (stageTypeList.Count > 0)
        {
            Debug.Log($"stageTypeList.Count: {stageTypeList.Count}");
            int index = Random.Range(0, stageTypeList.Count);
            shuffledStageTypes.Add(stageTypeList[index]);
            stageTypeList.RemoveAt(index);
        }
    }

    // Game Start and Next Game
    public void pilotTest()
    {
        if (thisStageNum <= totalStageNum) //Quest Count Check
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
        // Clean existed cloned object
        foreach (GameObject obj in clonedObjects)
        {
            Destroy(obj);
        }
        clonedObjects.Clear();

        // stageNumber UI
        stageNumber.text = $"{thisStageNum - 1} / {totalStageNum} ";

        // select stageType
        int stageType = shuffledStageTypes[thisStageNum - 2]; // pilotStage(thisStageNum++);
        tg.TableGeneratePilot(tableEx, stageType, true); // TableEx

        Debug.Log($"stageType: {stageType}");

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

        //generate choice tables
        for (int i = 0; i < 3; i++)
        {
            if (i == answer) //correct answer
            {
                //-----------정답 테이블 선지로 복제
                //// find answer rotation degree
                //int correctAnswerIndex = tg.RotateAngle(stageType);
                //TableController answerTable;
                //try
                //{
                //    answerTable = Instantiate(answerArray[correctAnswerIndex], answerArray[correctAnswerIndex].transform.parent);

                //    // Transform to choice Table
                //    answerTable.transform.position = new Vector3(-1.3f+(1.3f*i), -3.3f, 0);
                //    answerTable.transform.localScale = new Vector3(0.21f, 0.21f, 0);

                //    // add in cloned object list
                //    clonedObjects.Add(answerTable.gameObject);
                //}
                //catch (IndexOutOfRangeException)
                //{
                //    Debug.LogError($"Invalid correctAnswerIndex: {correctAnswerIndex}");
                //}

                //------------정답 테이블 선지로 이동
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
                }
                while (table[i].CompareTable(tableEx)); // is it in answerArray
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
            Debug.Log("correct");
            correct = true;
        }
        else
        {
            Debug.Log("incorrect");
            correct = false;
        }

        gm.StoreScore(new StageScore(thisStageNum, time, correct));
        pilotTest(); // Next Stage Call
    }
}
