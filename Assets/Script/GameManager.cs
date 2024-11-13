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
    public MonsterSpawner monsterSpawner;  // MonsterSpawner 참조 추가

    private void Awake()
    {
        mapCreate = FindObjectOfType<MapCreate>();
        monsterSpawner = FindObjectOfType<MonsterSpawner>();  // MonsterSpawner 찾기

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
        mapCreate.PlayerMoveRoomCreate(newPosition); // 새로운 타일 생성
        monsterSpawner.mapParent = mapCreate.mapParent; // mapParent를 MonsterSpawner에 할당
        monsterSpawner.SpawnMonsterOnTile(newPosition); // 이동한 타일에 몬스터를 스폰
    }
}
