public class Wolf : Monster
{
    // ���� ����
    protected override float hpMultiplier => 1f;    // ���� HP ����
    protected override float expMultiplier => 1f;   // ���� EXP ����
    protected override float goldMultiplier => 1f;  // ���� ��ȭ ����
    protected override float atkMultiplier => 1.5f; // ���� ���ݷ� ����

    private void Start()
    {
        // ������ ����� ���� ���� ����
        SetMonsterStats();
        // ���� �ֱ� ����
        SetAttackInterval();
    }


    // Wolf ���ʹ� ���� �ֱ⸦ 0.8�ʷ� ����
    protected override void SetAttackInterval()
    {
        attackInterval = 0.8f;  // Wolf�� ���� �ֱ⸦ 0.8�ʷ� ����
    }
}
