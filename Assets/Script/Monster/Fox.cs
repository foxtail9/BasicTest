public class Fox : Monster
{
    // ���� ����
    protected override float hpMultiplier => 0.5f;   // ���� HP ����
    protected override float expMultiplier => 0.5f;  // ���� EXP ����
    protected override float goldMultiplier => 0.5f; // ���� ��ȭ ����
    protected override float atkMultiplier => 0.5f;  // ���� ���ݷ� ����
    private void Start()
    {
        SetMonsterStats();
        // ���� �ֱ� ����
        SetAttackInterval();
    }

    // Pig ���ʹ� ���� �ֱ⸦ 1.5�ʷ� ����
    protected override void SetAttackInterval()
    {
        attackInterval = 1.5f;  // Pig�� ���� �ֱ⸦ 1.5�ʷ� ����
    }
}
