using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public bool autoMove = true;
    public GameObject Player = null;
    public float speed = 0.25f;
    public Vector3 offset;

   
    void Start()
    {
        if (Player == null)
        {
            Player = GameObject.FindWithTag("Player");  
        }
        offset = transform.position - Player.transform.position;
        this.transform.position = Player.transform.position + offset;
    }
    void Update()
    {
        if (autoMove)
        {
            Vector3 targetPosition = Player.transform.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}
