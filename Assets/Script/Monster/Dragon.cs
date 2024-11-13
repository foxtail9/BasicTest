public class Dragon : Monster
{
    // 배율 설정
    protected override float hpMultiplier => 2f;     // 매우 높은 HP 배율
    protected override float expMultiplier => 2f;    // 매우 높은 EXP 배율
    protected override float goldMultiplier => 2f;   // 매우 높은 금화 배율
    protected override float atkMultiplier => 2f;    // 매우 높은 공격력 배율
    private void Start()
    {
        SetMonsterStats();
        SetAttackInterval();
    }

    // Dragon 몬스터는 공격 주기를 1초로 설정
    protected override void SetAttackInterval()
    {
        attackInterval = 1f;  // Dragon의 공격 주기를 1초로 설정
    }
}
