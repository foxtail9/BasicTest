using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public int playerLevel = 1;
    public int playerExp = 0;
    public int playerAtk = 2;
    public int playerHP = 1000;
    public int playerMAXHP = 1000;
    public int playerMP = 50;
    public int playerMAXMP = 500;
    public int playerGold = 0;

    private PlayerUIControll playerUI;
    private MonsterSpawner monsterSpawner;
    private GameManager gameManager;  // GameManager 참조 추가

    private float expIncreaseTimer = 0f;
    private float attackCooldown = 1f; // 기본 공격 주기 (1초로 설정)
    private float lastAttackTime = 0f; // 마지막 공격 시간을 기록하는 변수
    private float attackTimeAccumulator = 0f;  // 누적 시간 변수 추가

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

        monsterSpawner = FindObjectOfType<MonsterSpawner>();  // MonsterSpawner 클래스 참조
        gameManager = FindObjectOfType<GameManager>();  // GameManager 클래스 참조
    }

    private void Update()
    {
        IncreaseExpOverTime();
        playerUI.UpdateUI();

        // 전투 중일 때만 자동 사냥 활성화
        if (gameManager.isInBattle)
        {
            AutoFight();
        }
    }

    private void IncreaseExpOverTime() //자동 경험치 획득
    {
        expIncreaseTimer += Time.deltaTime;

        if (expIncreaseTimer >= 1f)
        {
            expIncreaseTimer = 0f;
            GainExp(1);
        }
    }

    public void GainExp(int expAmount)//유저 경험치 획득
    {
        playerExp += expAmount;
        while (playerExp >= GetRequiredExp())
        {
            playerExp -= GetRequiredExp();
            playerLevel++;
        }
        UpdateLevelUI();
    }

    public int GetRequiredExp() 
    {
        return playerLevel * 10;
    }

    private void UpdateLevelUI() //유저 레벨업처리
    {
        playerUI.levelText.text = "Lv " + playerLevel;
        playerAtk += 8;
        playerMAXHP += 10;
        playerMAXMP += 5;
    }

    // 몬스터 스폰어의 몬스터 리스트를 사용하여 자동 전투 처리
    private void AutoFight()
    {
        if (monsterSpawner.monsters.Count <= 0)
        {
            gameManager.isInBattle = false;
            return;
        }

        // 공격 간격이 지난 후에만 공격
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            // 스폰된 몬스터 리스트에서 몬스터를 하나씩 처리
            for (int i = 0; i < monsterSpawner.monsters.Count; i++)
            {
                Monster monster = monsterSpawner.monsters[i];
                if (monster != null && !monster.IsDead())
                {
                    Attack(monster);

                    // 몬스터가 죽으면 리스트에서 제거
                    if (monster.IsDead())
                    {
                        monsterSpawner.RemoveMonster(monster);
                        i--; // 인덱스를 하나 감소시켜, 리스트의 크기 변화를 반영
                    }
                }
            }
            lastAttackTime = Time.time;
        }
    }

    public void Attack(Monster monster)
    {
        if (monster != null && !monster.IsDead())
        {
            monster.TakeDamage(playerAtk);  // 플레이어의 공격력으로 몬스터에게 데미지
            if (monster.IsDead())
            {
                monsterSpawner.RemoveMonster(monster);  // 몬스터 리스트에서 제거
            }
        }
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
        //gameManager.GameOver();  // 예시로 GameOver 처리 메서드 추가
    }

}
