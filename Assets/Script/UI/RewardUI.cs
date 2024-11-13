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

    // ���� ���� �� ���� UI Ȱ��ȭ
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

    // ���� ���� ���� ��ȯ
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
                return "Equipment!";  // ���� ���߿� ���� ����
            default:
                return "";
        }
    }

    // ī�� Ŭ�� �� ���� ó��
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
                // ��� �߰� ó�� (�̱��������� �⺻ �ؽ�Ʈ�� ǥ��)
                Debug.Log("Equipment gained: Basic Sword");
                break;
        }

        CloseRewardUI();  // ���� �� UI �ݱ�
    }

    // Reward UI ��Ȱ��ȭ �� ī�� ����Ʈ �ʱ�ȭ
    private void CloseRewardUI()
    {
        rewardUIPanel.SetActive(false);
        Time.timeScale = 1f;  // �� �ð� �簳
        foreach (Transform child in cardListParent)
        {
            Destroy(child.gameObject);  // ī�� �ʱ�ȭ
        }
    }
}
