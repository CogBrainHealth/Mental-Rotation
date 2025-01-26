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


}
