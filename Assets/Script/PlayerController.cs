using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveDistance = 10f; // �̵��� �Ÿ�
    private Vector3 curPos;
    private Vector3 moveValue;
    public float moveTime = 0.4f;
    private bool isMoving = false;
    private Vector3 targetPos;

    public Transform chick = null;  // �÷��̾��� ȸ���� (ī�޶� ��)
    public bool isDead = false;

    void Start()
    {
        moveValue = Vector3.zero;
        curPos = transform.position;
    }

    // InputSystem�� ���� �̵� ó��
    public void Move(InputAction.CallbackContext context)
    {
        if (isMoving) return; // �̹� �̵� ���̸� ó������ ����

        // �̵� ���� ó��
        Vector3 input = context.ReadValue<Vector3>();

        // �Է��� 0,0�̸� �̵� ����
        if (input.magnitude > 1f) return;

        // �̵��� ���� ���
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

    // �̵� �޼ҵ�
    void Moving(Vector3 pos)
    {
        targetPos = pos;
        curPos = transform.position;
        isMoving = true;
        StartCoroutine(MoveCoroutine());
    }

    // �̵��� ���� �ڷ�ƾ
    IEnumerator MoveCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(curPos, targetPos, elapsedTime / moveTime); // �ε巴�� �̵�
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;  // ���� ��ǥ ��ġ�� ��Ȯ�� �̵�
        isMoving = false;

        // �� ���� ���� ȣ��
        GameManager.Instance.PlayerMoveEvent(transform.position);
    }

    // �÷��̾��� ȸ�� ó�� (W, A, S, D ���⿡ �°� ȸ��)
    void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero) // ������ ���� ���� ȸ��
        {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            chick.rotation = Quaternion.Euler(0, angle, 0); // y���� �������� ȸ��
        }
    }
}
