using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    public float currentHP;
    public float maxHP;
    public float XP;
    public float speed;
    public float damage;
    public float[] position = new float[3];

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayer()
    {
        GlobalControl.Instance.currentHP = currentHP;
        GlobalControl.Instance.maxHP = maxHP;
        GlobalControl.Instance.XP = XP;
        GlobalControl.Instance.speed = speed;
        GlobalControl.Instance.damage = damage;
        GlobalControl.Instance.position = position;
    }


}