using TMPro;
using UnityEngine;

public class NativeAPI
{
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    public static extern void sendMessageToMobileApp(string message);
#endif
}

public class MessageToReact : MonoBehaviour
{
    public string inputScore;

    public int allocScore(string inputScore)
    {
        int score;
        if (int.TryParse(inputScore, out score))
        {
            return score;
        }
        else
        {
            Debug.LogError("inputScore is not a valid number: " + inputScore);
            return 0;
        }
    }

    public void ButtonPressed()
    {
        // 1. 점수 UI 오브젝트 찾기
        var scoreText = GameObject.FindGameObjectWithTag("Score")
            .GetComponent<TextMeshProUGUI>();

        // 2. 텍스트 가져오기
        inputScore = scoreText.text;

        // 3. 점수 정수로 변환
        int scoreValue = allocScore(inputScore);

        // 4. JSON 생성
        ScoreData score = new ScoreData(scoreValue);
        string scoreJSON = JsonUtility.ToJson(score);

        Debug.Log("scoreJSON: " + scoreJSON);

        // 5. 플랫폼별 메시지 전송
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.azesmwayreactnativeunity.ReactNativeUnityViewManager"))
            {
                jc.CallStatic("sendMessageToMobileApp", scoreJSON);
                Debug.Log("Score sent to Android");
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS && !UNITY_EDITOR
            NativeAPI.sendMessageToMobileApp(scoreJSON);
            Debug.Log("Score sent to iOS");
#endif
        }
    }
}

// JSON으로 보낼 구조체
[System.Serializable]
public class ScoreData
{
    public int score;
    // 필요한 경우 날짜나 추가 필드도 포함 가능

    public ScoreData(int s)
    {
        score = s;
    }
}