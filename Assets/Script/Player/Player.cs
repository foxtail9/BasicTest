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
    private GameManager gameManager;  // GameManager ���� �߰�

    private float expIncreaseTimer = 0f;
    private float attackCooldown = 1f; // �⺻ ���� �ֱ� (1�ʷ� ����)
    private float lastAttackTime = 0f; // ������ ���� �ð��� ����ϴ� ����
    private float attackTimeAccumulator = 0f;  // ���� �ð� ���� �߰�

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

        monsterSpawner = FindObjectOfType<MonsterSpawner>();  // MonsterSpawner Ŭ���� ����
        gameManager = FindObjectOfType<GameManager>();  // GameManager Ŭ���� ����
    }

    private void Update()
    {
        IncreaseExpOverTime();
        playerUI.UpdateUI();

        // ���� ���� ���� �ڵ� ��� Ȱ��ȭ
        if (gameManager.isInBattle)
        {
            AutoFight();
        }
    }

    private void IncreaseExpOverTime() //�ڵ� ����ġ ȹ��
    {
        expIncreaseTimer += Time.deltaTime;

        if (expIncreaseTimer >= 1f)
        {
            expIncreaseTimer = 0f;
            GainExp(1);
        }
    }

    public void GainExp(int expAmount)//���� ����ġ ȹ��
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

    private void UpdateLevelUI() //���� ������ó��
    {
        playerUI.levelText.text = "Lv " + playerLevel;
        playerAtk += 8;
        playerMAXHP += 10;
        playerMAXMP += 5;
    }

    // ���� �������� ���� ����Ʈ�� ����Ͽ� �ڵ� ���� ó��
    private void AutoFight()
    {
        if (monsterSpawner.monsters.Count <= 0)
        {
            gameManager.isInBattle = false;
            return;
        }

        // ���� ������ ���� �Ŀ��� ����
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            // ������ ���� ����Ʈ���� ���͸� �ϳ��� ó��
            for (int i = 0; i < monsterSpawner.monsters.Count; i++)
            {
                Monster monster = monsterSpawner.monsters[i];
                if (monster != null && !monster.IsDead())
                {
                    Attack(monster);

                    // ���Ͱ� ������ ����Ʈ���� ����
                    if (monster.IsDead())
                    {
                        monsterSpawner.RemoveMonster(monster);
                        i--; // �ε����� �ϳ� ���ҽ���, ����Ʈ�� ũ�� ��ȭ�� �ݿ�
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
            monster.TakeDamage(playerAtk);  // �÷��̾��� ���ݷ����� ���Ϳ��� ������
            if (monster.IsDead())
            {
                monsterSpawner.RemoveMonster(monster);  // ���� ����Ʈ���� ����
            }
        }
    }

    // ������ ���ݿ� ���� �������� �޴� �޼���
    public void TakeDamage(int damage)
    {
        playerHP -= damage;

        if (playerHP <= 0)
        {
            playerHP = 0;
            Die();
        }
    }

    // �÷��̾ �׾��� �� ó��
    private void Die()
    {
        Debug.Log("Player has died!");
        //gameManager.GameOver();  // ���÷� GameOver ó�� �޼��� �߰�
    }

}
