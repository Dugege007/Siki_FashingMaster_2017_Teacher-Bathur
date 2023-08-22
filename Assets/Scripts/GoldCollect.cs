﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCollect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gold"))
        {
            AudioManager.Instance.PlayEffectSound(AudioManager.Instance.goldClip);
            Destroy(collision.gameObject);
        }
    }
}
