using UnityEngine;

public class Monster : MonoBehaviour
{
    public int monsterCurHP;         // ���� HP
    public int monsterMaxHP;         // �ִ� HP
    public int monsterEXP;           // óġ �� �ִ� ����ġ
    public int monsterGold;          // óġ �� �ִ� ��ȭ
    public int monsterAtk;           // ���ݷ�
    public float attackInterval = 1f; // �⺻ ���� ���� (��)

    public Player player;            // �÷��̾� ����
    private bool isInRange = false;  // ���� ������ ������ ����
    private float attackTime = 0f;   // ������ ���� �ð��� ��� 
    // �ɷ�ġ ������ �����ϱ� ���� �Ӽ�
    protected virtual float hpMultiplier => 1f; // �⺻ HP ����
    protected virtual float expMultiplier => 1f; // �⺻ EXP ����
    protected virtual float goldMultiplier => 1f; // �⺻ ��ȭ ����
    protected virtual float atkMultiplier => 1f; // �⺻ ���ݷ� ����


    private void Start()
    {
        if (Player.Instance != null)  // Player�� �̱������� �ʱ�ȭ�� �Ŀ��� ����
        {
            player = Player.Instance; // Player �ν��Ͻ� ����
            SetMonsterStats(); // ���� ������ �÷��̾� ���ȿ� ����ϵ��� ����
        }
        else
        {
            Debug.LogError("Player instance is not initialized!");
            return;
        }
        monsterCurHP = monsterMaxHP;
        SetAttackInterval();  // ���ͺ��� ���� ���� ����
    }

    public void Update()
    {
        if (player == null) return;  // �÷��̾ ������ ������Ʈ�� �����.
        if (isInRange)
        {
            if (Time.time - attackTime >= attackInterval)
            {
                AutoAttack();
                attackTime = Time.time;  // ������ ���� �ð��� ���� �ð����� ����
            }
        }
        else
        {
            MoveToPlayer();
        }
    }
    public void SetMonsterStats()
    {
        // ������ �⺻ ���� ����
        monsterMaxHP = player.playerLevel * 10;  // �⺻ HP�� ���� * 10
        monsterAtk = player.playerLevel * 3;     // �⺻ ���ݷ��� ���� * 3
        monsterEXP = player.playerLevel * 4;     // �⺻ ����ġ�� ���� * 4
        monsterGold = player.playerLevel * 5;    // �⺻ ��ȭ�� ���� * 5

        // ���� ����
        monsterMaxHP = Mathf.RoundToInt(monsterMaxHP * hpMultiplier);  // HP ���� ����
        monsterAtk = Mathf.RoundToInt(monsterAtk * atkMultiplier);    // ���ݷ� ���� ����
        monsterEXP = Mathf.RoundToInt(monsterEXP * expMultiplier);    // ����ġ ���� ����
        monsterGold = Mathf.RoundToInt(monsterGold * goldMultiplier); // ��ȭ ���� ����

        monsterCurHP = monsterMaxHP;  // �ʱ� HP�� �ִ� HP�� ����
    }

    public void TakeDamage(int damage)
    {
        monsterCurHP -= damage;

        if (monsterCurHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (player != null)
        {
            player.GainExp(monsterEXP);
            player.playerGold += monsterGold;
        }
        Destroy(gameObject);
    }

    // ���Ͱ� �÷��̾�� �̵�
    public void MoveToPlayer()
    {
        if (player == null) return;  // �÷��̾ ���� ��� �̵����� ����

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= 1f)  // ���� ������ ���� ������ ����
        {
            isInRange = true;
        }
        else
        {
            isInRange = false;
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.position += direction * Time.deltaTime * 2f; // �ӵ��� 2�� ����
        }
    }

    public void AutoAttack()
    {
        if (player != null)
        {
            // �÷��̾�� ������ �ؼ� HP�� ���ҽ�Ų��.
            player.TakeDamage(monsterAtk);  // �÷��̾��� TakeDamage �޼��� ȣ��
        }
    }

    // �� ���Ϳ� �´� ���� ������ �����ϴ� �޼���
    protected virtual void SetAttackInterval()
    {
        attackInterval = 1f;  // �⺻ ���� ������ 1�ʷ� ����
    }
}
