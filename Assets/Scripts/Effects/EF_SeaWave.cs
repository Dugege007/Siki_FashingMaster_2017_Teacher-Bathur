using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EF_SeaWave : MonoBehaviour
{
    private Vector3 tempPos;

    private void Start()
    {
        tempPos = -transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, tempPos, 10 * Time.deltaTime);
    }
}
