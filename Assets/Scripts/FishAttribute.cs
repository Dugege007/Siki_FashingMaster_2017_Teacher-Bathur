using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAttribute : MonoBehaviour
{
    public int health;
    public int exp;
    public int gold;
    public int maxNum;
    public int maxSpeed;

    public GameObject deathPrefab;
    public GameObject goldPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        if (health<=0)
        {
            GameController.Instance.gold += gold;
            GameController.Instance.exp += exp;

            GameObject death = Instantiate(deathPrefab);
            death.transform.SetParent(transform.parent, false);
            death.transform.position = transform.position;
            death.transform.rotation = transform.rotation;

            GameObject goldCoin = Instantiate(goldPrefab);
            goldCoin.transform.SetParent(transform.parent, false);
            goldCoin.transform.position = transform.position;
            goldCoin.transform.rotation = transform.rotation;

            if (gameObject.GetComponent<EF_PlayEffect>() != null)
            {
                AudioManager.Instance.PlayEffectSound(AudioManager.Instance.rewardClip);
                gameObject.GetComponent<EF_PlayEffect>().PlayEffect();
            }

            Destroy(gameObject);
        }
    }
}
