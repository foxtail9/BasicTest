public class Golem : Monster
{
    // 배율 설정
    protected override float hpMultiplier => 1.5f;   // 높은 HP 배율
    protected override float expMultiplier => 1.2f;  // 높은 EXP 배율
    protected override float goldMultiplier => 1.3f; // 높은 금화 배율
    protected override float atkMultiplier => 1f;    // 보통 공격력 배율

    private void Start()
    {
        // 배율이 적용된 몬스터 스탯 설정
        SetMonsterStats();
        SetAttackInterval();
    }


    protected override void SetAttackInterval()
    {
        attackInterval = 5f;  // Golem의 공격 주기를 2초로 설정
    }
}
