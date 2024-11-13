using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class RewardUI : MonoBehaviour
{
    public GameObject rewardUIPanel;   
    public Transform cardListParent;   
    public GameObject cardPrefab;     
    public Player player;             

    private List<string> rewardTypes = new List<string>
    {
        "Gold", "Experience", "AttackPower", "HP_MP_Recovery", "Equipment"
    };

    private void Start()
    {
        rewardUIPanel.SetActive(false);  
    }

    // 전투 종료 후 보상 UI 활성화
    public void ShowRewardUI()
    {
        rewardUIPanel.SetActive(true);
        GenerateRandomRewards();
        Time.timeScale = 0f;  
    }

    private void GenerateRandomRewards()
    {
        foreach (Transform child in cardListParent)
        {
            Destroy(child.gameObject);  
        }

        for (int i = 0; i < 3; i++)
        {
            string randomReward = rewardTypes[Random.Range(0, rewardTypes.Count)];
            GameObject card = Instantiate(cardPrefab, cardListParent);
            TextMeshProUGUI cardText = card.GetComponentInChildren<TextMeshProUGUI>();

            cardText.text = GetRewardDescription(randomReward);
            card.GetComponent<Button>().onClick.AddListener(() => OnCardClicked(randomReward));
        }
    }

    // 랜덤 보상 설명 반환
    private string GetRewardDescription(string rewardType)
    {
        switch (rewardType)
        {
            case "Gold":
                return "GOLD\n + " + Random.Range(50, 200);
            case "Experience":
                return "EXP+\n + " + Random.Range(100, 500);
            case "AttackPower":
                return "ATK + " + Random.Range(5, 20);
            case "HP_MP_Recovery":
                return "HP,MP\n Recovery";
            case "Equipment":
                return "Equipment!";  // 장비는 나중에 구현 예정
            default:
                return "";
        }
    }

    // 카드 클릭 시 보상 처리
    private void OnCardClicked(string rewardType)
    {
        switch (rewardType)
        {
            case "Gold":
                player.playerGold += Random.Range(50, 200);
                break;
            case "Experience":
                player.GainExp(Random.Range(100, 500));
                break;
            case "AttackPower":
                player.playerAtk += Random.Range(5, 20);
                break;
            case "HP_MP_Recovery":
                player.playerHP = Mathf.Min(player.playerHP + 200, player.playerMAXHP);
                player.playerMP = Mathf.Min(player.playerMP + 50, player.playerMAXMP);
                break;
            case "Equipment":
                // 장비 추가 처리 (미구현이지만 기본 텍스트로 표시)
                Debug.Log("Equipment gained: Basic Sword");
                break;
        }

        CloseRewardUI();  // 보상 후 UI 닫기
    }

    // Reward UI 비활성화 및 카드 리스트 초기화
    private void CloseRewardUI()
    {
        rewardUIPanel.SetActive(false);
        Time.timeScale = 1f;  // 씬 시간 재개
        foreach (Transform child in cardListParent)
        {
            Destroy(child.gameObject);  // 카드 초기화
        }
    }
}
