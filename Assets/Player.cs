using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int playerLevel = 1;
    private int playerExp = 0;
    private int playeratk = 50;
    private int playerHP = 250;
    private int playerMP = 50;
    private int playerGold = 0;

    public void Awake()
    {
        this.gameObject.transform.position = new Vector3(0, 1, 0);
    }
}
