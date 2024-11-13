public class Wolf : Monster
{
    // 배율 설정
    protected override float hpMultiplier => 1f;    // 보통 HP 배율
    protected override float expMultiplier => 1f;   // 보통 EXP 배율
    protected override float goldMultiplier => 1f;  // 보통 금화 배율
    protected override float atkMultiplier => 1.5f; // 높은 공격력 배율

    private void Start()
    {
        // 배율이 적용된 몬스터 스탯 설정
        SetMonsterStats();
        // 공격 주기 설정
        SetAttackInterval();
    }


    // Wolf 몬스터는 공격 주기를 0.8초로 설정
    protected override void SetAttackInterval()
    {
        attackInterval = 0.8f;  // Wolf의 공격 주기를 0.8초로 설정
    }
}
