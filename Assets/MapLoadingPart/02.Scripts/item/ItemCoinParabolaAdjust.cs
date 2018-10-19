using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCoinParabolaAdjust : MonoBehaviour {
    public GameObject huddle;
    public GameObject fire;
   
    private void Awake()
    {
        huddle = transform.Find("Huddle").gameObject;
        fire = transform.Find("Fire").gameObject;
    }
    private void OnEnable()
    {
        int g = Random.Range(0, 3);
        if (g.Equals(0))
        {
            huddle.SetActive(false);
            fire.SetActive(false);
            return;
        }
        if (g.Equals(1))
        {
            huddle.SetActive(true);
            fire.SetActive(false);
            return;
        }
        if (g.Equals(2))
        {
            huddle.SetActive(false);
            fire.SetActive(true);
            return;
        }
    }
    private void OnDisable()
    {
        huddle.transform.position = transform.position;
        huddle.transform.rotation = transform.rotation;
    }
}
