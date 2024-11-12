using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    GameObject gameManagerObject = new GameObject("GameManager");
                    instance = gameManagerObject.AddComponent<GameManager>();
                }

                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
    public MapCreate mapCreate;

    private void Awake()
    {
        mapCreate = FindObjectOfType<MapCreate>();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (mapCreate == null)
            {
                mapCreate = FindObjectOfType<MapCreate>();
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlayerMoveEvent(Vector3 newPosition)
    {
        mapCreate.PlayerMoveRoomCreate(newPosition);
    }
}
