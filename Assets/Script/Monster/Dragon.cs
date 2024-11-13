public class Dragon : Monster
{
    // ���� ����
    protected override float hpMultiplier => 2f;     // �ſ� ���� HP ����
    protected override float expMultiplier => 2f;    // �ſ� ���� EXP ����
    protected override float goldMultiplier => 2f;   // �ſ� ���� ��ȭ ����
    protected override float atkMultiplier => 2f;    // �ſ� ���� ���ݷ� ����
    private void Start()
    {
        SetMonsterStats();
        SetAttackInterval();
    }

    // Dragon ���ʹ� ���� �ֱ⸦ 1�ʷ� ����
    protected override void SetAttackInterval()
    {
        attackInterval = 1f;  // Dragon�� ���� �ֱ⸦ 1�ʷ� ����
    }
}
