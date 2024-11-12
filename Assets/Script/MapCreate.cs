using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreate : MonoBehaviour
{
    public List<GameObject> tilePrefabs;       // �ٴ� Ÿ�� ������ ����Ʈ
    public List<GameObject> decorationPrefabs;  // �ٹ̱�� ������ ����Ʈ
    public GameObject mapParent;
    public GameObject player;
    private Vector3 playerPosition;

    private int mapWidth = 3;
    private int mapHeight = 3;

    // ���� ������ Ÿ���� �����ϴ� ����Ʈ
    private List<GameObject> newTiles = new List<GameObject>();

    // ������ ������ Ÿ���� �����ϴ� ����Ʈ
    private Queue<GameObject> tileQueue = new Queue<GameObject>();

    // �̵��� Ƚ�� ����
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

        // �� Ÿ�� ���� (�������� ����)
        CreateTile(playerX + 10, playerZ);  // ���� Ÿ��
        CreateTile(playerX - 10, playerZ);  // ���� Ÿ��
        CreateTile(playerX, playerZ + 10);  // ���� Ÿ��
        CreateTile(playerX, playerZ - 10);  // ���� Ÿ��
        CreateTile(playerX, playerZ);       // �÷��̾� ��ġ Ÿ�� ����
    }

    void CreateTile(int x, int z)
    {
        // ���� ��ġ�� �������� ���⿡ Ÿ���� �̹� �ִ��� �˻�
        if (TileExists(x, z))
        {
            return; // Ÿ���� �̹� �����ϸ� �������� ����
        }

        // �ٴ� Ÿ�� ������ �� �ϳ��� �������� �����Ͽ� ����
        GameObject randomTilePrefab = tilePrefabs[Random.Range(0, tilePrefabs.Count)];
        GameObject newTile = Instantiate(randomTilePrefab, new Vector3(x, 0, z), Quaternion.identity, mapParent.transform);
        newTiles.Add(newTile);  // ���� ������ Ÿ���� newTiles ����Ʈ�� �߰�

        // �ٹ̱� ������ ���� �� ��ġ ���� ����
        if (decorationPrefabs.Count > 0)
        {
            GameObject randomDecorationPrefab = decorationPrefabs[Random.Range(0, decorationPrefabs.Count)];

            // ������ 0.5�� �� �ȿ��� ���� ��ġ ����
            float angle = Random.Range(0f, Mathf.PI * 2); // 0���� 2������� ����
            float radius = Random.Range(0f, 5f);        // 0���� 0.5������ ������

            // ���� ������ ��� (Y ��ǥ�� 0.1f�� ����)
            Vector3 randomOffset = new Vector3(Mathf.Cos(angle) * radius, 0.1f, Mathf.Sin(angle) * radius);

            // �ٹ̱� �������� Ÿ�� ���� ���� ��ġ�� ��ġ
            GameObject decoration = Instantiate(randomDecorationPrefab, new Vector3(x+Mathf.Cos(angle) * radius, 0.1f, z+Mathf.Sin(angle) * radius), Quaternion.identity, newTile.transform);
        }

    }

    bool TileExists(int x, int z)
    {
        foreach (GameObject tile in newTiles)
        {
            if (Mathf.Approximately(tile.transform.position.x, x) && Mathf.Approximately(tile.transform.position.z, z))
            {
                return true; // Ÿ���� �̹� ������
            }
        }
        foreach (GameObject tile in tileQueue)
        {
            if (Mathf.Approximately(tile.transform.position.x, x) && Mathf.Approximately(tile.transform.position.z, z))
            {
                return true; // Ÿ���� �̹� ������
            }
        }
        return false; // Ÿ���� �������� ����
    }

    public void PlayerMoveRoomCreate(Vector3 newPosition)
    {
        moveCount++;  // �̵� Ƚ�� ����
        CreateNewTiles(newPosition);

        if (moveCount >= 3)
        {
            DeactivateOldTiles();
            moveCount = 0;  // �̵� Ƚ�� �ʱ�ȭ
        }
    }

    void CreateNewTiles(Vector3 position)
    {
        int playerX = Mathf.FloorToInt(position.x);
        int playerZ = Mathf.FloorToInt(position.z);

        // �ֺ� Ÿ�� ���� (��������)
        CreateTile(playerX + 10, playerZ);  // ���� Ÿ��
        CreateTile(playerX - 10, playerZ);  // ���� Ÿ��
        CreateTile(playerX, playerZ + 10);  // ���� Ÿ��
        CreateTile(playerX, playerZ - 10);  // ���� Ÿ��
        CreateTile(playerX, playerZ);       // �÷��̾� ��ġ Ÿ�� ����
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

        // newTiles ����Ʈ ����
        newTiles.Clear();
    }
}
