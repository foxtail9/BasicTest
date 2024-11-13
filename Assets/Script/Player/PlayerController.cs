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

    private BoxCollider boxCollider;  // 플레이어의 BoxCollider

    private void Start()
    {
        moveValue = Vector3.zero;
        curPos = transform.position;
        boxCollider = GetComponent<BoxCollider>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (isMoving) return;

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

        // 충돌 중인 Plane을 mapParent에 동적 할당
        CheckCollisionAndAssignPlane();

        GameManager.Instance.PlayerMoveEvent(transform.position); //이동이 끝나고 타일 생성함수 호출
    }

    void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            chick.rotation = Quaternion.Euler(0, angle, 0);
        }
    }

    // 플레이어와 충돌한 Plane을 mapParent에 동적으로 할당
    void CheckCollisionAndAssignPlane()
    {
        // 플레이어의 BoxCollider와 충돌한 Plane을 확인
        Collider[] hitColliders = Physics.OverlapBox(boxCollider.bounds.center, boxCollider.bounds.extents, Quaternion.identity);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Plane"))  // 충돌한 객체가 Plane인지 확인
            {
                GameObject plane = hitCollider.gameObject;
                // mapParent를 동적 할당
                if (GameManager.Instance != null && GameManager.Instance.mapCreate != null)
                {
                    GameManager.Instance.mapCreate.mapParent = plane;  // mapParent를 Plane으로 할당
                }
            }
        }
    }
}
