using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Tes : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    private void Update()
    {
        //Debug.Log(-player.directionTrasform.up);
    }

    public void Login(string tempString)
    {
        PlayerPrefs.SetString("First Time Play", tempString);
        GameManager.instance.dbManager.Login(null);
    }
}
