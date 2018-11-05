using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStage : MonoBehaviour {

	// Use this for initialization
	void Start () {
        CharacterMove.bossStage = true;
    }
	
	// Update is called once per frame
	void Update () {
        CharacterMove.bossStage = true;
	}
}
