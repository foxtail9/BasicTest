using UnityEngine;

public class Monster : MonoBehaviour
{
    public int monsterCurHP;         // 현재 HP
    public int monsterMaxHP;         // 최대 HP
    public int monsterEXP;           // 처치 시 주는 경험치
    public int monsterGold;          // 처치 시 주는 금화
    public int monsterAtk;           // 공격력
    public float attackInterval = 1f; // 기본 공격 간격 (초)

    public Player player;            // 플레이어 참조
    private bool isInRange = false;  // 공격 범위에 들어갔는지 여부
    private float attackTime = 0f;   // 마지막 공격 시간을 기록 
    // 능력치 배율을 적용하기 위한 속성
    protected virtual float hpMultiplier => 1f; // 기본 HP 배율
    protected virtual float expMultiplier => 1f; // 기본 EXP 배율
    protected virtual float goldMultiplier => 1f; // 기본 금화 배율
    protected virtual float atkMultiplier => 1f; // 기본 공격력 배율


    private void Start()
    {
        if (Player.Instance != null)  // Player가 싱글턴으로 초기화된 후에만 접근
        {
            player = Player.Instance; // Player 인스턴스 참조
            SetMonsterStats(); // 몬스터 스탯을 플레이어 스탯에 비례하도록 설정
        }
        else
        {
            Debug.LogError("Player instance is not initialized!");
            return;
        }
        monsterCurHP = monsterMaxHP;
        SetAttackInterval();  // 몬스터별로 공격 간격 설정
    }

    public void Update()
    {
        if (player == null) return;  // 플레이어가 없으면 업데이트를 멈춘다.
        if (isInRange)
        {
            if (Time.time - attackTime >= attackInterval)
            {
                AutoAttack();
                attackTime = Time.time;  // 마지막 공격 시간을 현재 시간으로 설정
            }
        }
        else
        {
            MoveToPlayer();
        }
    }
    public void SetMonsterStats()
    {
        // 몬스터의 기본 스탯 설정
        monsterMaxHP = player.playerLevel * 10;  // 기본 HP는 레벨 * 10
        monsterAtk = player.playerLevel * 3;     // 기본 공격력은 레벨 * 3
        monsterEXP = player.playerLevel * 4;     // 기본 경험치는 레벨 * 4
        monsterGold = player.playerLevel * 5;    // 기본 금화는 레벨 * 5

        // 배율 적용
        monsterMaxHP = Mathf.RoundToInt(monsterMaxHP * hpMultiplier);  // HP 배율 적용
        monsterAtk = Mathf.RoundToInt(monsterAtk * atkMultiplier);    // 공격력 배율 적용
        monsterEXP = Mathf.RoundToInt(monsterEXP * expMultiplier);    // 경험치 배율 적용
        monsterGold = Mathf.RoundToInt(monsterGold * goldMultiplier); // 금화 배율 적용

        monsterCurHP = monsterMaxHP;  // 초기 HP는 최대 HP로 설정
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

    // 몬스터가 플레이어에게 이동
    public void MoveToPlayer()
    {
        if (player == null) return;  // 플레이어가 없는 경우 이동하지 않음

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= 1f)  // 공격 범위에 들어가면 공격을 시작
        {
            isInRange = true;
        }
        else
        {
            isInRange = false;
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.position += direction * Time.deltaTime * 2f; // 속도는 2로 설정
        }
    }

    public void AutoAttack()
    {
        if (player != null)
        {
            // 플레이어에게 공격을 해서 HP를 감소시킨다.
            player.TakeDamage(monsterAtk);  // 플레이어의 TakeDamage 메서드 호출
        }
    }

    // 각 몬스터에 맞는 공격 간격을 설정하는 메서드
    protected virtual void SetAttackInterval()
    {
        attackInterval = 1f;  // 기본 공격 간격을 1초로 설정
    }
}
