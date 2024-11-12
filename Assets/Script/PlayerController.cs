using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveDistance = 10f; // 이동할 거리
    private Vector3 curPos;
    private Vector3 moveValue;
    public float moveTime = 0.4f;
    private bool isMoving = false;
    private Vector3 targetPos;

    public Transform chick = null;  // 플레이어의 회전용 (카메라 등)
    public bool isDead = false;

    void Start()
    {
        moveValue = Vector3.zero;
        curPos = transform.position;
    }

    // InputSystem을 통해 이동 처리
    public void Move(InputAction.CallbackContext context)
    {
        if (isMoving) return; // 이미 이동 중이면 처리하지 않음

        // 이동 방향 처리
        Vector3 input = context.ReadValue<Vector3>();

        // 입력이 0,0이면 이동 안함
        if (input.magnitude > 1f) return;

        // 이동할 방향 계산
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

    // 이동 메소드
    void Moving(Vector3 pos)
    {
        targetPos = pos;
        curPos = transform.position;
        isMoving = true;
        StartCoroutine(MoveCoroutine());
    }

    // 이동을 위한 코루틴
    IEnumerator MoveCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(curPos, targetPos, elapsedTime / moveTime); // 부드럽게 이동
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;  // 최종 목표 위치로 정확히 이동
        isMoving = false;

        // 방 생성 로직 호출
        GameManager.Instance.PlayerMoveEvent(transform.position);
    }

    // 플레이어의 회전 처리 (W, A, S, D 방향에 맞게 회전)
    void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero) // 방향이 있을 때만 회전
        {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            chick.rotation = Quaternion.Euler(0, angle, 0); // y축을 기준으로 회전
        }
    }
}
