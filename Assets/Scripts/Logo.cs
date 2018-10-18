using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Logo : MonoBehaviour {
    public RectTransform Mask;
    public RectTransform StartText;
    private TouchControl cMove;
	// Use this for initialization
	void Start () {
        LeanTween.textAlpha(StartText, 1f, 1.0f).setOnComplete(TextComplete);
    }
	
	// Update is called once per frame
	void Update () {

        
       if (Input.GetMouseButtonDown(0)) // 마우스가 눌린 경우 아무키나 터치된 것으로 간주한다.
        {
            Mask.gameObject.SetActive(true); // 페이드아웃용 흰색 패널을 활성화시킨다.
            LeanTween.alpha(Mask, 1f, 1.0f).setOnComplete(Complete); // 알파값을 1f까지 서서히 증가시켜 패널이 나타나게 한다.
        }
    }
    void Complete()
    {
        SceneManager.LoadScene(1); // 빌드셋팅에서 1번에 설정된 Scene을 불러온다.
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
}
