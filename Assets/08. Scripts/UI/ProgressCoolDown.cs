using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressCoolDown : MonoBehaviour {
    public Image CoolDown;
    public float ItemTime;
    public float MaxtTime;
    // Use this for initialization
    private void Awake()
    {
        CoolDown = this.GetComponent<Image>();
    }
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        ItemTime -= Time.deltaTime;
        CoolDown.fillAmount = ItemTime / MaxtTime;
	}

    void OnEnable() {
        ItemTime = MaxtTime;
        CoolDown.fillAmount = 1f;
    }
}
