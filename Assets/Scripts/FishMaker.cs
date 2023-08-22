using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMaker : MonoBehaviour
{
    public Transform fishHolder;
    public Transform[] genPositions;
    public GameObject[] fishPrefabs;

    public float fishGenWaitTime = 0.5f;
    public float waveGenWaitTime = 0.5f;

    private void Start()
    {
        InvokeRepeating("MakeFishes", 1, waveGenWaitTime);
    }

    private void MakeFishes()
    {
        int genPosIndex = Random.Range(0, genPositions.Length);
        int fishPreIndex = Random.Range(0, fishPrefabs.Length);
        int maxNum = fishPrefabs[fishPreIndex].GetComponent<FishAttribute>().maxNum;
        int maxSpeed = fishPrefabs[fishPreIndex].GetComponent<FishAttribute>().maxSpeed;
        int num = Random.Range((maxNum / 2) + 1, maxNum);
        int speed = Random.Range(maxSpeed / 2, maxNum);
        int moveType = Random.Range(0, 2);  //0 直走，1 转弯
        int angOffset;  //仅直走生效，直走的倾斜角
        int angSpeed;   //仅转弯生效，转弯的角速度

        if (moveType == 0)
        {
            // 直走鱼群的生成
            angOffset = Random.Range(-22, 22);
            StartCoroutine(GenStraightFish(genPosIndex, fishPreIndex, num, speed, angOffset));
        }
        else
        {
            // 转弯鱼群的生成
            if (Random.Range(0,2)==0)
                angSpeed = Random.Range(-15, -9);
            else
                angSpeed = Random.Range(9, 15);
            StartCoroutine(GenTurnFish(genPosIndex, fishPreIndex, num, speed, angSpeed));
        }
    }

    private IEnumerator GenStraightFish(int genPosIndex, int fishPreIndex, int num, int speed, int angOffset)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject fish = Instantiate(fishPrefabs[fishPreIndex]);
            fish.transform.SetParent(fishHolder, false);    //false 表示不用再计算一遍世界坐标
            fish.transform.localPosition = genPositions[genPosIndex].localPosition;
            fish.transform.localRotation = genPositions[genPosIndex].localRotation;
            fish.transform.Rotate(0, 0, angOffset);
            fish.GetComponent<SpriteRenderer>().sortingOrder += i;
            fish.AddComponent<EF_AutoMove>().speed = speed;

            yield return new WaitForSeconds(fishGenWaitTime);
        }
    }

    private IEnumerator GenTurnFish(int genPosIndex, int fishPreIndex, int num, int speed, int angSpeed)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject fish = Instantiate(fishPrefabs[fishPreIndex]);
            fish.transform.SetParent(fishHolder, false);    //false 表示不用再计算一遍世界坐标
            fish.transform.localPosition = genPositions[genPosIndex].localPosition;
            fish.transform.localRotation = genPositions[genPosIndex].localRotation;
            fish.GetComponent<SpriteRenderer>().sortingOrder += i;
            fish.AddComponent<EF_AutoMove>().speed = speed;
            fish.AddComponent<EF_AutoRotate>().speed = angSpeed;

            yield return new WaitForSeconds(fishGenWaitTime);
        }
    }
}
