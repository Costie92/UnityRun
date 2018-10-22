using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelect : MonoBehaviour {
    private static UISelect _instance = null;
    public static UISelect instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("UISelect is NULL");
            return _instance;
        }
    }
    EditorManager editor;
    // Use this for initialization
    void Start () {
        editor = GameObject.Find("EditorManager").GetComponent<EditorManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnClickExit() {
        this.gameObject.SetActive(false);
    }
    public void OnClickObjBtn(Button btn) {
        editor.ClickedObj.GetComponent<Image>().sprite = btn.gameObject.GetComponent<Image>().sprite;
        this.gameObject.SetActive(false);
    }
}
