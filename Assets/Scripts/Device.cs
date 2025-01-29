using UnityEngine;

public class Device : MonoBehaviour
{
    void Awake()
    {
        setRect();
    }

    public void setRect()
    {
        // 현재 GameObject에 부착된 Camera 컴포넌트를 가져오는 코드
        Camera cam = GetComponent<Camera>();

        // 현재 카메라의 뷰포트 영역을 가져오는 코드
        Rect viewportRect = cam.rect;

        float deviceWidth = (float)Screen.width;
        float deviceHeight = (float)Screen.height;
        float targetWidth = 1080f;
        float targetHeight = 2340f;

        //Screen.SetResolution(targetWidth, (int)(((float)deviceHeight / deviceWidth) * targetWidth), FullScreenMode.Windowed);

        if (targetWidth / targetHeight < (float)deviceWidth / deviceHeight)
        {
            float newWidth = ((float)targetWidth / targetHeight) / ((float)deviceWidth / deviceHeight);
            //Screen.SetResolution((int)newWidth, (int)deviceHeight, FullScreenMode.Windowed);
            cam.rect = new Rect((1f - newWidth) / 2f, 0, newWidth, 1f);
            Debug.Log("너비 조정");
        }
        else
        {
            float newHeight = (deviceWidth / deviceHeight) / (targetWidth / targetHeight);
            //Screen.SetResolution((int)deviceWidth, (int)newHeight, FullScreenMode.Windowed);
            cam.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
            Debug.Log("높이 조정");
        }
    }

    private void OnPreCull()
    {
        GL.Clear(true, true, new Color(250 / 256f, 203 / 256f, 103 / 256f, 1f));
    }
}
