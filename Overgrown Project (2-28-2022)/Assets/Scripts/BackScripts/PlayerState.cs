using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public float currentHP = 100;
    public float maxHP = 100;
    public float XP = 0;
    public float speed = 0;
    public float damage = 25;
    //private Vector3 position;
    //private float[] savePosition = new float[3];

    /*public void Awake()
    {
        position = transform.position;
        SerializePosition[0] = position.x;
        SerializePosition[1] = position.y;
        SerializePosition[2] = position.z;
    }
    void Start()
    {
        currentHP = GlobalControl.Instance.currentHP;
        maxHP = GlobalControl.Instance.maxHP;
        XP = GlobalControl.Instance.XP;
        speed = GlobalControl.Instance.speed;
        damage = GlobalControl.Instance.damage;
        SerializePosition = GlobalControl.Instance.position;
    }

    private void Update()
    {
        position = transform.position;
        SerializePosition[0] = position.x;
        SerializePosition[1] = position.y;
        SerializePosition[2] = position.z;

    }*/
}
