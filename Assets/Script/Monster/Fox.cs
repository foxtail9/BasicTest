public class Fox : Monster
{
    // 배율 설정
    protected override float hpMultiplier => 0.5f;   // 낮은 HP 배율
    protected override float expMultiplier => 0.5f;  // 낮은 EXP 배율
    protected override float goldMultiplier => 0.5f; // 낮은 금화 배율
    protected override float atkMultiplier => 0.5f;  // 낮은 공격력 배율
    private void Start()
    {
        SetMonsterStats();
        // 공격 주기 설정
        SetAttackInterval();
    }

    // Pig 몬스터는 공격 주기를 1.5초로 설정
    protected override void SetAttackInterval()
    {
        attackInterval = 1.5f;  // Pig의 공격 주기를 1.5초로 설정
    }
}
