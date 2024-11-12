using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreate : MonoBehaviour
{
    public List<GameObject> tilePrefabs;       // 바닥 타일 프리팹 리스트
    public List<GameObject> decorationPrefabs;  // 꾸미기용 프리팹 리스트
    public GameObject mapParent;
    public GameObject player;
    private Vector3 playerPosition;

    private int mapWidth = 3;
    private int mapHeight = 3;

    // 새로 생성된 타일을 관리하는 리스트
    private List<GameObject> newTiles = new List<GameObject>();

    // 이전에 생성된 타일을 관리하는 리스트
    private Queue<GameObject> tileQueue = new Queue<GameObject>();

    // 이동한 횟수 추적
    private int moveCount = 0;

    void Awake()
    {
        playerPosition = new Vector3(0, 0, 0);
        CreateInitialMap();
    }

    void CreateInitialMap()
    {
        int playerX = Mathf.FloorToInt(playerPosition.x);
        int playerZ = Mathf.FloorToInt(playerPosition.z);

        // 맵 타일 생성 (동서남북 포함)
        CreateTile(playerX + 10, playerZ);  // 동쪽 타일
        CreateTile(playerX - 10, playerZ);  // 서쪽 타일
        CreateTile(playerX, playerZ + 10);  // 북쪽 타일
        CreateTile(playerX, playerZ - 10);  // 남쪽 타일
        CreateTile(playerX, playerZ);       // 플레이어 위치 타일 생성
    }

    void CreateTile(int x, int z)
    {
        // 현재 위치와 동서남북 방향에 타일이 이미 있는지 검사
        if (TileExists(x, z))
        {
            return; // 타일이 이미 존재하면 생성하지 않음
        }

        // 바닥 타일 프리팹 중 하나를 랜덤으로 선택하여 생성
        GameObject randomTilePrefab = tilePrefabs[Random.Range(0, tilePrefabs.Count)];
        GameObject newTile = Instantiate(randomTilePrefab, new Vector3(x, 0, z), Quaternion.identity, mapParent.transform);
        newTiles.Add(newTile);  // 새로 생성된 타일을 newTiles 리스트에 추가

        // 꾸미기 프리팹 생성 및 위치 랜덤 설정
        if (decorationPrefabs.Count > 0)
        {
            GameObject randomDecorationPrefab = decorationPrefabs[Random.Range(0, decorationPrefabs.Count)];

            // 반지름 0.5의 원 안에서 랜덤 위치 생성
            float angle = Random.Range(0f, Mathf.PI * 2); // 0부터 2π까지의 각도
            float radius = Random.Range(0f, 5f);        // 0부터 0.5까지의 반지름

            // 랜덤 오프셋 계산 (Y 좌표는 0.1f로 고정)
            Vector3 randomOffset = new Vector3(Mathf.Cos(angle) * radius, 0.1f, Mathf.Sin(angle) * radius);

            // 꾸미기 프리팹을 타일 위의 랜덤 위치에 배치
            GameObject decoration = Instantiate(randomDecorationPrefab, new Vector3(x+Mathf.Cos(angle) * radius, 0.1f, z+Mathf.Sin(angle) * radius), Quaternion.identity, newTile.transform);
        }

    }

    bool TileExists(int x, int z)
    {
        foreach (GameObject tile in newTiles)
        {
            if (Mathf.Approximately(tile.transform.position.x, x) && Mathf.Approximately(tile.transform.position.z, z))
            {
                return true; // 타일이 이미 존재함
            }
        }
        foreach (GameObject tile in tileQueue)
        {
            if (Mathf.Approximately(tile.transform.position.x, x) && Mathf.Approximately(tile.transform.position.z, z))
            {
                return true; // 타일이 이미 존재함
            }
        }
        return false; // 타일이 존재하지 않음
    }

    public void PlayerMoveRoomCreate(Vector3 newPosition)
    {
        moveCount++;  // 이동 횟수 증가
        CreateNewTiles(newPosition);

        if (moveCount >= 3)
        {
            DeactivateOldTiles();
            moveCount = 0;  // 이동 횟수 초기화
        }
    }

    void CreateNewTiles(Vector3 position)
    {
        int playerX = Mathf.FloorToInt(position.x);
        int playerZ = Mathf.FloorToInt(position.z);

        // 주변 타일 생성 (동서남북)
        CreateTile(playerX + 10, playerZ);  // 동쪽 타일
        CreateTile(playerX - 10, playerZ);  // 서쪽 타일
        CreateTile(playerX, playerZ + 10);  // 북쪽 타일
        CreateTile(playerX, playerZ - 10);  // 남쪽 타일
        CreateTile(playerX, playerZ);       // 플레이어 위치 타일 생성
    }

    void DeactivateOldTiles()
    {
        foreach (GameObject tile in newTiles)
        {
            tileQueue.Enqueue(tile);
        }

        if (tileQueue.Count > 8)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject oldestTile = tileQueue.Dequeue();
                Destroy(oldestTile);
            }
        }

        // newTiles 리스트 비우기
        newTiles.Clear();
    }
}
