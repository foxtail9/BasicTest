using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIControll : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI levelText;
    [SerializeField] public Image expFill;
    [SerializeField] public Image hpFill;
    [SerializeField] public TextMeshProUGUI maxHp;
    [SerializeField] public Image mpFill;
    [SerializeField] public TextMeshProUGUI maxMp;
    [SerializeField] public TextMeshProUGUI goldText;

    private void Update()
    {
        if (Player.Instance != null)
        {
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("Player.Instance is not initialized yet.");
        }
    }

    public void UpdateUI()
    {
        if (Player.Instance == null) return;

        int playerExp = Player.Instance.playerExp;
        int requiredExp = Player.Instance.GetRequiredExp();
        int playerHP = Player.Instance.playerHP;
        int playerMaxHP = Player.Instance.playerMAXHP;
        int playerMP = Player.Instance.playerMP;
        int playerMaxMP = Player.Instance.playerMAXMP;
        int playerGold = Player.Instance.playerGold;

        expFill.fillAmount = (float)playerExp / requiredExp;
        hpFill.fillAmount = (float)playerHP / playerMaxHP;
        mpFill.fillAmount = (float)playerMP / playerMaxMP;

        goldText.text = playerGold.ToString("N0") + " G";
        levelText.text = "Lv " + Player.Instance.playerLevel;

        maxHp.text = $"{playerHP} / {playerMaxHP}";
        maxMp.text = $"{playerMP} / {playerMaxMP}";
    }
}
