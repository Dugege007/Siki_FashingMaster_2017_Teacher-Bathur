using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance { get { return instance; } }

    [Header("UI")]
    public Text oneShootCostText;
    public Text goldText;
    public Text levelText;
    public Text lvNameText;
    public Text smallCountdownText;
    public Text bigCountdownText;
    public Button bigCountdownBtn;
    public Button backBtn;
    public Button settingBtn;
    public Slider expSlider;
    public GameObject levelUpTips;

    public Sprite[] bgSprites;
    public int bgIndex = 0;
    public Image bgImage;

    [Header("Effects")]
    public GameObject fireEffectPrefab;
    public GameObject changeEffect;
    public GameObject levelUpEffect;
    public GameObject goldEffect;
    public GameObject seaWaveEffect;

    [Header("Attribute")]
    public int level = 0;
    public int exp = 0;
    public int gold = 500;
    public const int bigCountdown = 240;
    public const int smallCountdown = 60;
    public float bigTimer = bigCountdown;
    public float smallTimer = smallCountdown;
    public Color goldColor;

    [Header("Other")]
    public Transform bulletHolder;
    public GameObject[] gunGos;
    public GameObject[] bullet1Gos;
    public GameObject[] bullet2Gos;
    public GameObject[] bullet3Gos;
    public GameObject[] bullet4Gos;
    public GameObject[] bullet5Gos;

    //使用的是第几档的炮弹
    private int costIndex = 0;
    //每一炮所需的金币数和造成的伤害值
    private int[] oneShootCosts = { 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };
    private string[] lvNames = { "新手", "入门", "钢铁", "青铜", "白银", "黄金", "铂金", "钻石", "达人", "大师" };

    private void Awake()
    {
        instance = this;
        goldColor = goldText.color;
    }

    private void Start()
    {
        gold = PlayerPrefs.GetInt("gold", gold);
        level = PlayerPrefs.GetInt("level", level);
        exp = PlayerPrefs.GetInt("exp", exp);
        smallTimer = PlayerPrefs.GetFloat("smallCountdown", smallCountdown);
        bigTimer = PlayerPrefs.GetFloat("bigCountdown", bigCountdown);
        UpdateUI();
    }

    private void Update()
    {
        ChangeBulletCost();
        Fire();
        UpdateUI();
        ChangeBackground();
    }

    private void ChangeBackground()
    {
        if (bgIndex != level / 25)
        {
            bgIndex = level / 25;

            AudioManager.Instance.PlayEffectSound(AudioManager.Instance.seaWaveClip);
            Instantiate(seaWaveEffect);
            if (bgIndex >= 3)
            {
                bgIndex = 3;
            }
            bgImage.sprite = bgSprites[bgIndex];
        }
    }

    private void UpdateUI()
    {
        bigTimer -= Time.deltaTime;
        smallTimer -= Time.deltaTime;
        if (smallTimer <= 0)
        {
            smallTimer = smallCountdown;
            gold += 50;
        }
        if (bigTimer <= 0 && bigCountdownBtn.gameObject.activeSelf == false)
        {
            bigCountdownText.gameObject.SetActive(false);
            bigCountdownBtn.gameObject.SetActive(true);

        }

        //经验等级换算：升级所需经验=1000+200*当前等级
        while (exp >= 1000 + 200 * level)
        {
            exp = exp - (1000 + 200 * level);

            level++;
            levelUpTips.gameObject.SetActive(true);
            levelUpTips.transform.Find("Level Text").GetComponent<Text>().text = level.ToString();
            StartCoroutine(levelUpTips.GetComponent<EF_HideSelf>().HideSelf(0.6f));
            AudioManager.Instance.PlayEffectSound(AudioManager.Instance.levelUpClip);
            Instantiate(levelUpEffect);
        }

        goldText.text = "$" + gold;
        levelText.text = level.ToString();

        if (level / 10 <= 9)
            lvNameText.text = lvNames[level / 10];
        else
            lvNameText.text = lvNames[9];

        smallCountdownText.text = (int)smallTimer / 10 + " " + (int)smallTimer % 10;
        bigCountdownText.text = (int)bigTimer + "s";

        expSlider.value = ((float)exp) / (1000 + 200 * level);
    }

    private void Fire()
    {
        GameObject[] useBullets = bullet5Gos;
        int bulletIndex;
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            if (gold - oneShootCosts[costIndex] >= 0)
            {
                switch (costIndex / 4)
                {
                    case 0: useBullets = bullet1Gos; break;
                    case 1: useBullets = bullet2Gos; break;
                    case 2: useBullets = bullet3Gos; break;
                    case 3: useBullets = bullet4Gos; break;
                    case 4: useBullets = bullet5Gos; break;
                }
                bulletIndex = (level % 10 >= 9) ? 9 : level % 10;
                gold -= oneShootCosts[costIndex];
                AudioManager.Instance.PlayEffectSound(AudioManager.Instance.fireClip);
                GameObject fireEffect = Instantiate(fireEffectPrefab);
                fireEffect.transform.position = gunGos[costIndex / 4].transform.Find("FirePos").transform.position;
                fireEffect.transform.rotation = gunGos[costIndex / 4].transform.Find("FirePos").transform.rotation;

                GameObject bullet = Instantiate(useBullets[bulletIndex]);
                bullet.transform.SetParent(bulletHolder, false);
                bullet.transform.position = gunGos[costIndex / 4].transform.Find("FirePos").transform.position;
                bullet.transform.rotation = gunGos[costIndex / 4].transform.Find("FirePos").transform.rotation;
                bullet.GetComponent<BulletAttribute>().damage = oneShootCosts[costIndex];
                bullet.AddComponent<EF_AutoMove>().dir = Vector3.up;
                bullet.GetComponent<EF_AutoMove>().speed = bullet.GetComponent<BulletAttribute>().speed;
            }
            else
            {
                // Flash The Text
                StartCoroutine("GoldNotEnough");
            }
        }
    }

    private void ChangeBulletCost()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
            OnButtonMDown();
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
            OnButtonPDown();
    }

    public void OnButtonPDown()
    {
        gunGos[costIndex / 4].gameObject.SetActive(false);
        costIndex++;
        AudioManager.Instance.PlayEffectSound(AudioManager.Instance.changeClip);
        Instantiate(changeEffect);
        costIndex = (costIndex > oneShootCosts.Length - 1) ? 0 : costIndex;
        gunGos[costIndex / 4].gameObject.SetActive(true);
        oneShootCostText.text = "$" + oneShootCosts[costIndex];
    }

    public void OnButtonMDown()
    {
        gunGos[costIndex / 4].gameObject.SetActive(false);
        costIndex--;
        AudioManager.Instance.PlayEffectSound(AudioManager.Instance.changeClip);
        Instantiate(changeEffect);
        costIndex = (costIndex < 0) ? oneShootCosts.Length - 1 : costIndex;
        gunGos[costIndex / 4].gameObject.SetActive(true);
        oneShootCostText.text = "$" + oneShootCosts[costIndex];
    }

    public void OnBigCountdownButtonDown()
    {
        gold += 500;
        AudioManager.Instance.PlayEffectSound(AudioManager.Instance.rewardClip);
        Instantiate(goldEffect);
        bigCountdownBtn.gameObject.SetActive(false);
        bigCountdownText.gameObject.SetActive(true);
        bigTimer = bigCountdown;
    }

    private IEnumerator GoldNotEnough()
    {
        goldText.color = Color.red;

        yield return new WaitForSeconds(0.5f);
        goldText.color = goldColor;
    }
}
