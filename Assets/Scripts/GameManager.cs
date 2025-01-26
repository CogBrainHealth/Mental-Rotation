using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //UI Animation
    public Animator information;

    //Scene UI
    public GameObject start;
    public GameObject game;
    public GameObject over;

    //Pilot Information
    public GameObject inputWarning;

    public TextMeshProUGUI inputAge;

    string userAge;
    string userGender = "여성"; //default

    //Game
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
        return true;
    }

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
    }
}
