using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartLogo : MonoBehaviour {
    public RectTransform Mask;
    public RectTransform StartText;
    private TouchControl cMove;
    private bool mWaitingForAuth = false;
    // Use this for initialization
    private static StartLogo _instance = null;
    public static StartLogo instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("StartLogo is NULL");
            return _instance;
        }
    }
    void Awake () {
        Time.timeScale = 1;
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false /* Don't force refresh */).Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        LeanTween.textAlpha(StartText, 1f, 1.0f).setOnComplete(TextComplete);
        if (Application.platform == RuntimePlatform.Android)
        {
            Application.targetFrameRate = 60;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        
        if (Input.GetMouseButtonDown(0) && !mWaitingForAuth) // 마우스가 눌린 경우 아무키나 터치된 것으로 간주한다.
        {
            mWaitingForAuth = true;
            StartText.GetComponent<Text>().text = "Loading";
            if (Social.localUser.authenticated)
            {
                LoginSuccess();
            }
            if (!Social.localUser.authenticated)
            {
                // Authenticate
                Social.localUser.Authenticate((bool success) =>
                {
                    if (success)
                    {
                        LoginSuccess();
                    }
                    else
                    {
                        StartText.GetComponent<Text>().text = "Authentication failed";
                        mWaitingForAuth = false;
                        //"Authentication failed.";
                    }
                });
            }
        }
    }
    void Complete()
    {
        SceneManager.LoadScene("StageSelect"); // 빌드셋팅에서 StageSelect Scene을 불러온다.
    }
    void TextComplete() {
        if (StartText.GetComponent<Text>().color.a == 0.0f)
        {
            LeanTween.textAlpha(StartText, 1f, 1.0f).setOnComplete(TextComplete);
        }
        else if (StartText.GetComponent<Text>().color.a == 1f) {
            LeanTween.textAlpha(StartText, 0.0f, 1.0f).setOnComplete(TextComplete);
        }
    }
    public void LoginSuccess() {
        Mask.gameObject.SetActive(true); // 페이드아웃용 흰색 패널을 활성화시킨다.
        LeanTween.alpha(Mask, 1f, 1.0f).setOnComplete(Complete); // 알파값을 1f까지 서서히 증가시켜 패널이 나타나게 한다.
    }
}
