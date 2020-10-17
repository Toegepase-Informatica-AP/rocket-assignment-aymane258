using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coinscript : MonoBehaviour
{
    public static int coins = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(30 * Time.deltaTime, 0, 0);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        coins++;
        Destroy(gameObject);
        GameObject.Find("CoinsText").GetComponent<Text>().text= "Coins : " + coins;
    }
}
