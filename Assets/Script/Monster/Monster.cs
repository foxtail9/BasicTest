using UnityEngine;
using UnityEngine.UI;  
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

    // ü�¹� UI
    public GameObject healthBarUI;  // ü�¹� UI
    public Image healthBarImage;   // ü�¹� �̹��� ������Ʈ
    public Camera mainCamera;      // ī�޶� ����

    private void Awake()
    {
        mainCamera = Camera.main;  // �⺻ ī�޶� ����
    }
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

        // ü�¹� �̹��� ������Ʈ�� ������
        if (healthBarUI != null)
        {
            healthBarImage = healthBarUI.GetComponent<Image>();
        }

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
            MoveToPlayer();  // �÷��̾�� �̵�
        }
       
        healthBarUI.transform.LookAt(mainCamera.transform);
        healthBarUI.transform.Rotate(0, 180, 0);  // �ݴ�� ������ �ʵ��� 180�� ȸ��

        // ü�¹��� fillAmount�� ü�¿� �°� ����
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = (float)monsterCurHP / (float)monsterMaxHP;
        }
    }

    // ���� ������ �÷��̾� ������ ���� ����
    public void SetMonsterStats()
    {
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

    // �÷��̾��� ������ �޴� �޼���
    public void TakeDamage(int damage)
    {
        monsterCurHP -= damage;

        if (monsterCurHP <= 0)
        {
            Die();
        }
    }

    // ���Ͱ� ���� �� ȣ��Ǵ� �޼���
    private void Die()
    {
        if (player != null)
        {
            player.GainExp(monsterEXP);  // ���� ��� �� ����ġ ����
            player.playerGold += monsterGold;  // ���� ��� �� ��ȭ ����
        }
        Destroy(gameObject);  // ���� GameObject ����
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

    // �ڵ����� �÷��̾ �����ϴ� �޼���
    public void AutoAttack()
    {
        if (player != null)
        {
            player.TakeDamage(monsterAtk);  // �÷��̾��� TakeDamage �޼��� ȣ��
        }
    }

    // �� ���Ϳ� �´� ���� ������ �����ϴ� �޼���
    protected virtual void SetAttackInterval()
    {
        attackInterval = 1f;  // �⺻ ���� ������ 1�ʷ� ����
    }

    // ���Ͱ� ����ִ��� Ȯ���ϴ� �޼���
    public bool IsDead()
    {
        return monsterCurHP <= 0;
    }
}
