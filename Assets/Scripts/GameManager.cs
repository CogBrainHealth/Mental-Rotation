using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    //UI Animation
    public Animator information;

    //Scene UI
    public GameObject start;
    public GameObject game;
    public GameObject over;

    //-------------Pilot Information-------------
    //input
    public GameObject inputWarning;

    public TextMeshProUGUI inputNickName;
    public TextMeshProUGUI inputAge;

    string nickName;
    string userAge;
    string userGender = "여성"; //default

    //output
    List<StageScore> stageScore = new List<StageScore>();

    public TextMeshProUGUI userInfo;
    public TextMeshProUGUI totalScore;
    public TextMeshProUGUI result;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this); 
    }

    void Start()
    {
        GameReady();
    }

    //Start UI
    public void skip()
    {
        information.SetBool("SKIP", true);
    }

    public void female()
    {
        userGender = "여성";
    }

    public void male()
    {
        userGender = "남성";
    }

    private bool SetInfo()
    {
        foreach (char c in inputAge.text) //Does age consist of numbers
        {
            if (c >= '0' && c <= '9' || "\0\n \u200B\t".Contains(c))
                continue;

            inputWarning.SetActive(true);
            return false;
        }

        userAge = inputAge.text;
        nickName = inputNickName.text;
        return true;
    }

    //UI 세팅
    public void GameReady()
    {
        start.SetActive(true);
        game.SetActive(false);
        over.SetActive(false);
    }

    public void GameStart()
    {
        if (SetInfo()) //나이, 성별 입력 정보가 적절하면
        {
            start.SetActive(false);
            game.SetActive(true);
            over.SetActive(false);
        }
    }

    public void GameOver()
    {
        start.SetActive(false);
        game.SetActive(false);
        over.SetActive(true);

        Debug.Log("게임 종료");
        displayResult();
    }

    //결과 점수 보여주기
    public void StoreScore(StageScore ss)
    {
        stageScore.Add(ss);
    }

    private void displayResult()
    {
        string str = "";
        int countCorrect = 0;

        foreach (StageScore ss in stageScore)
        {
            string correct = "오답";
            if (ss.correct)
            {
                countCorrect++;
                correct = "정답";
            }
            str += ss.name +"번 문제: " + ss.time.ToString("F3") + "초 / " + correct + "\n";
        }

        result.text = str;
        totalScore.text = "총 점수: " + countCorrect.ToString();
        userInfo.text = "닉네임: " + nickName + "\n" +
                        "성별: " + userGender + " / " + "나이: " + userAge;
    }
}

public class StageScore
{
    static int totalScore = 0;

    public int name;
    public float time;
    public bool correct;

    public StageScore(int n, float t, bool correct)
    {
        if(correct)
            totalScore++;

        this.name = n;
        this.correct = correct;
        this.time = t;
    }
}
