using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EF_MoveTo : MonoBehaviour
{
    public float speed = 50;
    private GameObject goldCollect;

    private void Start()
    {
        goldCollect = GameObject.Find("GoldCollect");
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, goldCollect.transform.position, speed * Time.deltaTime);
    }
}
