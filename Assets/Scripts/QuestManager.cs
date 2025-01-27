using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class QuestManager : MonoBehaviour
{
    GameManager gm;
    TableGenerator tg;

    public TableController tableEx;
    public TableController[] table = new TableController[3];

    // ??? ???? ???? ???
    private List<GameObject> clonedObjects = new List<GameObject>();

    //TestData
    public bool pilotFlag = true;
    public int[] pilotStageType = { 1, 2, 3, 4, 5, 6, 7, 8 }; // stageType ??
    List<int> shuffledStageTypes = new List<int>(); // Type ?? ?? ?? ?? 

    //List<Stage> pilotTableData = new List<Stage>();

    //GameData
    public int totalStageNum;
    public TextMeshProUGUI stageNumber; // StageNum UI

    private int thisStageNum = 1; //???? ??? 1?? ??
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

        PilotDataSetting(); //?? ??
    }

    public void Update()
    {
        time += Time.deltaTime;
    }

    //Game Setting
    public void Game()
    {
        if (thisStageNum <= totalStageNum) //??? ???? ??? ??? ?? ??
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
        Debug.Log($"stage {thisStageNum} start");
        // stageNumber.text = $"{thisStageNum-1} / {totalStageNum} ";

        //?? ???? ???? ??
        tg.TableGenerate(tableEx);

        //?? ?? ??
        answer = Random.Range(0, 3);

        //?? ??? ??
        

        time = 0f;
    }

    //PilotGame Setting
    private void PilotDataSetting()
    {
        // Stage s1 = new Stage(0, new int[] { 0, 0, 0, 0 }, new int[] { 1, 1, 1, 1 }, new int[] { 2, 2, 2, 2 }, new int[] { 3, 3, 3, 3 });

        // pilotTableData.Add(s1);

        totalStageNum = 8;

        // stageType ??
        List<int> stageTypeList = new List<int>(pilotStageType);

        while (stageTypeList.Count > 0)
        {
            Debug.Log($"stageTypeList.Count: {stageTypeList.Count}");
            int index = Random.Range(0, stageTypeList.Count);
            shuffledStageTypes.Add(stageTypeList[index]);
            stageTypeList.RemoveAt(index);
        }
    }

    public void pilotTest() //?? ?? and ?? ??
    {
        if (thisStageNum <= totalStageNum)
        {
            Debug.Log("------------------------------------------");
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
        Debug.Log($"stage {stageNum} extract");

        //if (stageNum > pilotTableData.Count) return; //??? ???? ? ??
        //Stage thisStage = pilotTableData[stageNum - 1]; //??? ???? ??. ????? -1

        //?? ??
        //answer = thisStage.answer;

        // ?? ??? ???? ?? ??
        foreach (GameObject obj in clonedObjects)
        {
            Destroy(obj);
        }
        clonedObjects.Clear();

        stageNumber.text = $"{thisStageNum - 1} / {totalStageNum} ";

        // select stageType
        int stageType = shuffledStageTypes[thisStageNum - 2]; // pilotStage(thisStageNum++); ??? ????? 2~9? ??? ?? 
        Debug.Log($"stageType: {stageType}");
        tg.TableGeneratePilot(tableEx, stageType);

        //correct answer array
        TableController[] answerArray = new TableController[3];
        for (int i = 0; i < 3; i++)
        {
            // RotateEx? ???? ??
            TableController rotatedTable = Instantiate(tableEx, tableEx.transform.parent);
            rotatedTable.RotateTable(90 * (i+1)); // 90?? ??
            rotatedTable.transform.position = new Vector3(-5, 1.2f * i, 0);
            rotatedTable.transform.localScale = new Vector3(0.21f, 0.21f, 0);
            answerArray[i] = rotatedTable;

            // ??? ??? ???? ??
            clonedObjects.Add(rotatedTable.gameObject);
        }

        //select correct answer number
        answer = Random.Range(0, 3);
        Debug.Log($"answer: {answer}");

        //?? ??? ??
        for (int i = 0; i < 3; i++)
        {
            if (i == answer) //?? ???? ?? 
            {
                Debug.Log("Generate Correct Table");

                // ?? ?? ??
                int correctAnswerIndex = tg.RotateAngle(stageType);
                TableController answerTable;
                if (correctAnswerIndex >= 0 && correctAnswerIndex < answerArray.Length)
                {
                    answerTable = Instantiate(answerArray[correctAnswerIndex], answerArray[correctAnswerIndex].transform.parent);

                    // ??? ??? ?? ??? ??
                    answerTable.transform.position = new Vector3(-1.3f+(1.3f*i), -3.3f, 0);
                    answerTable.transform.localScale = new Vector3(0.21f, 0.21f, 0);

                    // ??? ??? ???? ??
                    clonedObjects.Add(answerTable.gameObject);
                }
                else
                {
                    Debug.LogError($"Invalid correctAnswerIndex: {correctAnswerIndex}");
                }
            }
            else //?? ???? ??
            {
                do
                {
                    Debug.Log("Generate Incorrect Table");
                    table[i].gameObject.SetActive(true);
                    tg.TableGeneratePilot(table[i], stageType);
                }
                while (table[i].CompareTable()); // tableEx? answerArray? ????? ??
            }
        }

        time = 0f;

        // init answerArray
        for (int i = 0; i < 3; i++)
        {
            answerArray[i] = null;
        }
    }

    //?? ??
    public void choice(int n)
    {
        bool correct;

        // ?? ???? ????
        for (int i = 0; i < 3; i++)
        {
            Debug.Log(i + "th setActive false");
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

////pilot ?? ????
//public class Stage
//{
//    public int[] exam { get; set; }
//    public int[] table1 { get; set; }
//    public int[] table2 { get; set; }
//    public int[] table3 { get; set; }

//    public int answer;

//    public Stage(int a, int[] e, int[] t1, int[] t2, int[] t3)
//    {
//        answer = a;

//        exam = e;
//        table1 = t1;
//        table2 = t2;
//        table3 = t3;
//    }
//}