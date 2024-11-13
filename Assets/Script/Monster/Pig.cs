public class Pig : Monster
{
    // ���� ����
    protected override float hpMultiplier => 0.6f;   // ���� HP ����
    protected override float expMultiplier => 0.6f;  // ���� EXP ����
    protected override float goldMultiplier => 0.7f; // ���� ��ȭ ����
    protected override float atkMultiplier => 0.6f;  // ���� ���ݷ� ����

    private void Start()
    {
        SetMonsterStats();
        // ���� �ֱ� ����
        SetAttackInterval();
    }

    protected override void SetAttackInterval()
    {
        attackInterval = 4f;  // Pig�� ���� �ֱ⸦ 1.5�ʷ� ����
    }
}
