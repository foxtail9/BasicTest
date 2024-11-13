using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreate : MonoBehaviour
{
    public List<GameObject> tilePrefabs;
    public List<GameObject> decorationPrefabs;
    public GameObject mapParent;
    public GameObject player;

    private Vector3 playerPosition;
    private int moveCount = 0;

    private int mapWidth = 3;
    private int mapHeight = 3;

    public List<GameObject> newTiles = new List<GameObject>();
    private Queue<GameObject> tileQueue = new Queue<GameObject>();

    void Awake()
    {
        playerPosition = new Vector3(0, 1, 0);
        CreateInitialMap();
    }

    public void PlayerMoveRoomCreate(Vector3 newPosition) //이동 이벤트를 받아 실행
    {
        moveCount++;
        CreateNewTiles(newPosition);
    }

    void CreateNewTiles(Vector3 position) //타일을 생성하는 함수
    {
        int playerX = Mathf.FloorToInt(position.x);
        int playerZ = Mathf.FloorToInt(position.z);

        // 주변 타일 생성 (동서남북)
        CreateTile(playerX + 10, playerZ);  // 동쪽 타일
        CreateTile(playerX - 10, playerZ);  // 서쪽 타일
        CreateTile(playerX, playerZ + 10);  // 북쪽 타일
        CreateTile(playerX, playerZ - 10);  // 남쪽 타일
    }

    void CreateInitialMap() //첫씬 로드 기본 판넬 5개 생성
    {
        int playerX = Mathf.FloorToInt(playerPosition.x);
        int playerZ = Mathf.FloorToInt(playerPosition.z);

        CreateTile(playerX + 10, playerZ);  // 동쪽 타일
        CreateTile(playerX - 10, playerZ);  // 서쪽 타일
        CreateTile(playerX, playerZ + 10);  // 북쪽 타일
        CreateTile(playerX, playerZ - 10);  // 남쪽 타일
        CreateTile(playerX, playerZ);       // 플레이어 위치 타일 생성
    }

    void CreateTile(int x, int z)
    {
        if (TileExists(x, z))
        {
            return;
        }

        // 새로운 타일 생성
        GameObject randomTilePrefab = tilePrefabs[Random.Range(0, tilePrefabs.Count)];
        GameObject newTile = Instantiate(randomTilePrefab, new Vector3(x, 0, z), Quaternion.identity, mapParent.transform);
        newTiles.Add(newTile);

        // 데코레이션 추가
        if (decorationPrefabs.Count > 0)
        {
            GameObject randomDecorationPrefab = decorationPrefabs[Random.Range(0, decorationPrefabs.Count)];

            float angle = Random.Range(0f, Mathf.PI * 2);
            float radius = Random.Range(0f, 5f);

            Vector3 randomOffset = new Vector3(Mathf.Cos(angle) * radius, 0.1f, Mathf.Sin(angle) * radius);
            GameObject decoration = Instantiate(randomDecorationPrefab, new Vector3(x + Mathf.Cos(angle) * radius, 0.1f, z + Mathf.Sin(angle) * radius), Quaternion.identity, newTile.transform);
        }
    }

    bool TileExists(int x, int z)
    {
        foreach (GameObject tile in newTiles)
        {
            if (tile != null && Mathf.Approximately(tile.transform.position.x, x) && Mathf.Approximately(tile.transform.position.z, z))
            {
                return true;
            }
        }
        foreach (GameObject tile in tileQueue)
        {
            if (tile != null && Mathf.Approximately(tile.transform.position.x, x) && Mathf.Approximately(tile.transform.position.z, z))
            {
                return true;
            }
        }
        return false;
    }

}
