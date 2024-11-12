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
        UpdateUI();
    }

    public void UpdateUI()
    {
        int playerExp = Player.Instance.playerExp;
        int requiredExp = Player.Instance.GetRequiredExp();
        int playerHP = Player.Instance.playerHP;
        int playerMaxHP = Player.Instance.playerMAXHP; // Player의 최대 HP를 실시간으로 가져옴
        int playerMP = Player.Instance.playerMP;
        int playerMaxMP = Player.Instance.playerMAXMP; // Player의 최대 MP를 실시간으로 가져옴
        int playerGold = Player.Instance.playerGold;

        // 경험치와 HP, MP UI 업데이트
        expFill.fillAmount = (float)playerExp / requiredExp;
        hpFill.fillAmount = (float)playerHP / playerMaxHP; // 실시간 최대 HP 반영
        mpFill.fillAmount = (float)playerMP / playerMaxMP; // 실시간 최대 MP 반영

        // Gold와 Level 텍스트 업데이트
        goldText.text = playerGold.ToString("N0") + " G";
        levelText.text = "Lv " + Player.Instance.playerLevel;

        // HP와 MP 텍스트 업데이트 (현재/최대 형식)
        maxHp.text = $"{playerHP} / {playerMaxHP}"; // 현재/최대 HP 표시
        maxMp.text = $"{playerMP} / {playerMaxMP}"; // 현재/최대 MP 표시
    }
}
