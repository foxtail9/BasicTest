using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveDistance = 10f;
    private Vector3 curPos;
    private Vector3 moveValue;
    public float moveTime = 0.4f;
    private bool isMoving = false;
    private Vector3 targetPos;

    public Transform chick = null;
    public bool isDead = false;

    private BoxCollider boxCollider;  // �÷��̾��� BoxCollider
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        moveValue = Vector3.zero;
        curPos = transform.position;
        boxCollider = GetComponent<BoxCollider>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (isMoving) return;
        if (gameManager.isInBattle) return;

        Vector3 input = context.ReadValue<Vector3>();

        if (input.magnitude > 1f) return;

        if (context.performed)
        {
            if (input.magnitude == 0f)
            {
                Moving(transform.position + moveValue);
                Rotate(moveValue);
                moveValue = Vector3.zero;
            }
            else
            {
                moveValue = input * moveDistance;
            }
        }
    }

    void Moving(Vector3 pos)
    {
        targetPos = pos;
        curPos = transform.position;
        isMoving = true;
        StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(curPos, targetPos, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;

        // �浹 ���� Plane�� mapParent�� ���� �Ҵ�
        CheckCollisionAndAssignPlane();

        GameManager.Instance.PlayerMoveEvent(transform.position); //�̵��� ������ Ÿ�� �����Լ� ȣ��
    }

    void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            chick.rotation = Quaternion.Euler(0, angle, 0);
        }
    }

    // �÷��̾�� �浹�� Plane�� mapParent�� �������� �Ҵ�
    void CheckCollisionAndAssignPlane()
    {
        // �÷��̾��� BoxCollider�� �浹�� Plane�� Ȯ��
        Collider[] hitColliders = Physics.OverlapBox(boxCollider.bounds.center, boxCollider.bounds.extents, Quaternion.identity);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Plane"))  // �浹�� ��ü�� Plane���� Ȯ��
            {
                GameObject plane = hitCollider.gameObject;
                // mapParent�� ���� �Ҵ�
                if (GameManager.Instance != null && GameManager.Instance.mapCreate != null)
                {
                    GameManager.Instance.mapCreate.mapParent = plane;  // mapParent�� Plane���� �Ҵ�
                }
            }
        }
    }

    // �������� w, a, s, d �Է��� �����Ͽ� �̵� ó��
    public void RandomMove()
    {
        if (isMoving) return;
        if (gameManager.isInBattle) return;

        // �������� ������ ���� (w, a, s, d�� �����ϴ� ����)
        Vector3 randomDirection = GetRandomDirection();

        moveValue = randomDirection * moveDistance;

        // �̵� ó��
        Moving(transform.position + moveValue);
        Rotate(moveValue);
    }

    // w, a, s, d�� �����ϴ� ���͸� ��ȯ�ϴ� �Լ�
    Vector3 GetRandomDirection()
    {
        // 4���� ������ �������� ���� (W, A, S, D)
        int randomChoice = Random.Range(0, 4);

        switch (randomChoice)
        {
            case 0: return Vector3.forward;  // W -> ��
            case 1: return Vector3.left;     // A -> ����
            case 2: return Vector3.back;     // S -> ��
            case 3: return Vector3.right;    // D -> ������
            default: return Vector3.zero;
        }
    }

}
