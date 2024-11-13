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
    public MonsterSpawner monsterSpawner;  // MonsterSpawner ���� �߰�

    private void Awake()
    {
        mapCreate = FindObjectOfType<MapCreate>();
        monsterSpawner = FindObjectOfType<MonsterSpawner>();  // MonsterSpawner ã��

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlayerMoveEvent(Vector3 newPosition)
    {
        mapCreate.PlayerMoveRoomCreate(newPosition); // ���ο� Ÿ�� ����
        monsterSpawner.mapParent = mapCreate.mapParent; // mapParent�� MonsterSpawner�� �Ҵ�
        monsterSpawner.SpawnMonsterOnTile(newPosition); // �̵��� Ÿ�Ͽ� ���͸� ����
    }
}
