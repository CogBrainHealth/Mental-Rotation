using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //UI Animation
    public Animator information;

    //Scene UI
    public GameObject Start;
    public GameObject Game;
    public GameObject Over;

    //Pilot Information
    public GameObject inpuWarning;

    public TextMeshProUGUI inputAge;

    string userAge;
    string userGender = "여성"; //default

    public static GameManager Instance { get; private set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this); 

        GameReady();
    }

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

            inpuWarning.SetActive(true);
            return false;
        }

        userAge = inputAge.text;
        return true;
    }

    public void GameReady()
    {
        Start.SetActive(true);
        Game.SetActive(false);
        Over.SetActive(false);
    }

    public void GameStart()
    {
        if (SetInfo())
        {
            Start.SetActive(false);
            Game.SetActive(true);
            Over.SetActive(false);
        }
    }


<<<<<<< Updated upstream
=======
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
            str += ss.name-1 +"번 문제: " + ss.time.ToString("F3") + "초 / " + correct + "\n";
        }

        result.text = str;
        totalScore.text = "총 점수: " + countCorrect.ToString();
        userInfo.text = "성별: " + userGender + " / " + "나이: " + userAge;
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
>>>>>>> Stashed changes
}
