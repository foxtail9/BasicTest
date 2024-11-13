public class Golem : Monster
{
    // ���� ����
    protected override float hpMultiplier => 1.5f;   // ���� HP ����
    protected override float expMultiplier => 1.2f;  // ���� EXP ����
    protected override float goldMultiplier => 1.3f; // ���� ��ȭ ����
    protected override float atkMultiplier => 1f;    // ���� ���ݷ� ����

    private void Start()
    {
        // ������ ����� ���� ���� ����
        SetMonsterStats();
        SetAttackInterval();
    }


    protected override void SetAttackInterval()
    {
        attackInterval = 5f;  // Golem�� ���� �ֱ⸦ 2�ʷ� ����
    }
}
