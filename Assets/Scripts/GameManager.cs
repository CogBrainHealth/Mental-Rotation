using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    //UI Animation
    public Animator information;
    public Animator startAnim;

    //Scene UI
    public GameObject start;
    public GameObject game;
    public GameObject over;

    //out data
    //private int totalStage = 0; // 점수 평균 내기 위해?
    private float Score = 0;
    public bool isGameOver = false;

    //Over UI
    public Image ScoreBar;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI messageTitle;
    public TextMeshProUGUI message;

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

        startAnim.SetBool("StageStart", true);
    }

    public void GameOver()
    {
        isGameOver = true;
        start.SetActive(false);
        game.SetActive(false);
        over.SetActive(true);

        Debug.Log("게임 종료");
        displayResult();
    }

    //결과 점수 보여주기
    ///////////////////이거 고쳐야 됨///////////////////
    public void StoreScore(StageScore ss)
    {
        if (ss.correct)
            Score += ss.stageScore;
        Debug.Log(Score + "점");
    }

    private void displayResult()
    {
        ScoreText.text = (Score * 100 / 40).ToString();
        ScoreBar.fillAmount = Score / 40;
        //Score.text = (correct * 100 / total).ToString() + "점";
        //scoreBar.fillAmount = (float)correct / total;

        //pilot
        //string str = "";
        //int countCorrect = 0;

        //foreach (StageScore ss in stageScore)
        //{
        //    string correct = "오답";
        //    if (ss.correct)
        //    {
        //        countCorrect++;
        //        correct = "정답";
        //    }
        //    str += ss.name +"번 문제: " + ss.time.ToString("F3") + "초 / " + correct + "\n";
        //}

        //result.text = str;
        //totalScore.text = "총 점수: " + countCorrect.ToString();
        //userInfo.text = "닉네임: " + nickName + "\n" +
        //                "성별: " + userGender + " / " + "나이: " + userAge;

        messageTitle.text = "";
        message.text = "";
        
        if (Score > 96.0f)
        {
            messageTitle.text += "20대";

            message.text += "그림 돌리기 게임을 통해 공간 지각 능력을 검사했어요!\n\n" +
                            "최고 수준의 공간 지각 능력!\n" +
                            "000님은 20대 수준의 공간 지각 능력을 가지고 있어요!\n" +
                            "주어진 공간에서 위치를 빠르게 파악하고, 다양한 형태나 구조를 정확히 이해할 수 있는 능력이 뛰어나네요.\n\n" +
                            "이 수준을 유지하려면?\n" +
                            "- 다양한 공간적 배열을 시각적으로 상상하며 훈련하세요.\n" +
                            "- 3D 퍼즐이나 레고 조립처럼 입체적인 활동을 통해 더욱 향상시킬 수 있어요!";
        }

        else if (Score > 80.0f)
        {
            messageTitle.text += "30대";

            message.text += "그림 돌리기 게임을 통해 공간 지각 능력을 검사했어요!\n\n" +
                             "우수한 공간 지각 능력!\n" +
                             "000님은 30대 수준의 공간 지각 능력을 가지고 있어요!\n" +
                             "공간을 빠르게 이해하고 구조를 직관적으로 파악하는 능력이 뛰어나며, 여러 방향을 동시에 고려할 수 있어요.\n\n" +
                             "이 능력을 더 키우려면?\n" +
                             "- 다양한 방향으로 움직이며 공간을 회전시키는 연습을 해보세요.\n" +
                             "- 도형을 그려보며 공간적인 변화를 머릿속으로 상상해보는 것이 효과적입니다.";
        }

        else if (Score > 70.0f)
        {
            messageTitle.text += "40대";

            message.text += "그림 돌리기 게임을 통해 공간 지각 능력을 검사했어요!\n\n" +
                             "균형 잡힌 공간 지각 능력!\n" +
                             "000님은 40대 수준의 공간 지각 능력을 가지고 있어요!\n" +
                             "전체적인 공간을 잘 인지하고 지각하고 있어요.\n\n" +
                             "좀 더 향상시키려면?\n" +
                             "- 좀 더 복잡한 사물을 활용해보세요.\n" +
                             "- 공간을 인지할 때 이 사물을 돌리면 어떤 모습이 될까 연상해보는 연습이 효과적입니다.";
        }

        else if (Score > 60.0f)
        {
            messageTitle.text += "50대";

            message.text += "그림 돌리기 게임을 통해 공간 지각 능력을 검사했어요!\n\n" +
                             "훈련하면 더 좋아질 수 있어요!\n" +
                             "000님은 50대 수준의 공간 지각 능력을 가지고 있어요!\n" +
                             "공간을 이해하는 데 시간이 조금 더 걸리거나, 경로와 위치를 기억하는 데 어려움이 있을 수 있어요.\n\n" +
                             "더 나은 능력을 원한다면?\n" +
                             "- 주변 환경을 이용해보세요!\n" +
                             "  예: \"사과를 거꾸로 뒤집으면  어떤 모양이 될까?.\"\n" +
                             "  이런 방식으로 사물을 여러 방향으로 뒤집어 보면 공간지각능력이 향상됩니다!";

        }

        else if (Score > 50.0f)
        {
            messageTitle.text += "60대";

            message.text += "그림 돌리기 게임을 통해 공간 지각 능력을 검사했어요!\n\n" +
                             "조금 더 자주 훈련하면 좋아질 수 있어요!\n" +
                             "000님은 60대 수준의 공간 지각 능력을 가지고 있어요!\n" +
                             "공간을 빠르게 이해하는 데 어려움이 있을 수 있으며, 방향을 기억하는 데 시간이 더 걸릴 수 있어요.\n\n" +
                             "훈련을 위해?\n" +
                             "- 우리에게 익숙한 사물을 돌려보세요!";

        }

        else
        {
            messageTitle.text += " 70대";

            message.text += "그림 돌리기 게임을 통해 공간 지각 능력을 검사했어요!\n\n" +
                             "조금 더 연습하면 공간 지각 능력이 향상될 거예요!\n" +
                             "000님은 70대 수준의 공간 지각 능력을 가지고 있어요!\n" +
                             "공간을 인식하는데 어려움이 있을 수 있지만, 꾸준한 훈련을 통해 점차 개선될 수 있어요.\n\n" +
                             "훈련을 위해?\n" +
                             "- 간단한 도형을 돌려보세요!\n" +
                             "- 예를 들어, 간단한 도형을 다향한 방식으로 회전해보세요.";
        }
    }

    public void Terminate()
    {
        Application.Quit();
    }
}


public class StageScore
{
    public float stageScore = 0;

    public int name;
    public float time;
    public bool correct;

    public StageScore(int n, float t, bool correct)
    {
        float timeBonus;
        float rotateBonus = 0;
        
        if (correct)
        {
            // 응답 시간
            if (t < 5) timeBonus = 2;
            else if (t < 10) timeBonus = 1;
            else timeBonus = 0;

            // 난이도
            switch (QuestManager.Instance.shuffledStageTypes[QuestManager.Instance.thisStageNum - 1])
            {
                case 1:
                case 4:
                case 9:
                    rotateBonus = 1;
                    break;
                
                case 2:
                case 5:
                case 7:
                case 10:
                    rotateBonus = 2;
                    break;
                
                case 3:
                case 6:
                case 8:
                    rotateBonus = 3;
                    break;
            }

            stageScore = timeBonus * rotateBonus;
        }

        this.name = n;
        this.correct = correct;
        this.time = t;
    }
}
