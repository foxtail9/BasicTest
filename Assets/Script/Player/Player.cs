using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public int playerLevel = 1;
    public int playerExp = 0;
    public int playerAtk = 50;
    public int playerHP = 1000;
    public int playerMAXHP = 1000;
    public int playerMP = 50;
    public int playerMAXMP = 500;
    public int playerGold = 0;

    private PlayerUIControll playerUI;

    private float expIncreaseTimer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        this.gameObject.transform.position = new Vector3(0, 1, 0);
        playerUI = FindObjectOfType<PlayerUIControll>();
        playerUI.UpdateUI();
    }

    private void Update()
    {
        IncreaseExpOverTime();
        playerUI.UpdateUI();
    }

    private void IncreaseExpOverTime()
    {
        expIncreaseTimer += Time.deltaTime;

        if (expIncreaseTimer >= 1f)
        {
            expIncreaseTimer = 0f;
            GainExp(1);
        }
    }

    public void GainExp(int expAmount)
    {
        playerExp += expAmount;

        while (playerExp >= GetRequiredExp())
        {
            playerExp -= GetRequiredExp();
            playerLevel++;
            UpdateLevelUI();
        }
    }

    public int GetRequiredExp()
    {
        return playerLevel * 10;
    }

    private void UpdateLevelUI()
    {
        playerUI.levelText.text = "Lv " + playerLevel;
    }

    // 몬스터의 공격에 의한 데미지를 받는 메서드
    public void TakeDamage(int damage)
    {
        playerHP -= damage;

        if (playerHP <= 0)
        {
            playerHP = 0;
            Die();
        }
    }

    // 플레이어가 죽었을 때 처리
    private void Die()
    {
        Debug.Log("Player has died!");
        // 죽음 처리 (예: 게임 오버 화면 표시)
    }
}
