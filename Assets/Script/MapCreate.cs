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

    public void PlayerMoveRoomCreate(Vector3 newPosition) //�̵� �̺�Ʈ�� �޾� ����
    {
        moveCount++;
        CreateNewTiles(newPosition);
    }

    void CreateNewTiles(Vector3 position) //Ÿ���� �����ϴ� �Լ�
    {
        int playerX = Mathf.FloorToInt(position.x);
        int playerZ = Mathf.FloorToInt(position.z);

        // �ֺ� Ÿ�� ���� (��������)
        CreateTile(playerX + 10, playerZ);  // ���� Ÿ��
        CreateTile(playerX - 10, playerZ);  // ���� Ÿ��
        CreateTile(playerX, playerZ + 10);  // ���� Ÿ��
        CreateTile(playerX, playerZ - 10);  // ���� Ÿ��
    }

    void CreateInitialMap() //ù�� �ε� �⺻ �ǳ� 5�� ����
    {
        int playerX = Mathf.FloorToInt(playerPosition.x);
        int playerZ = Mathf.FloorToInt(playerPosition.z);

        CreateTile(playerX + 10, playerZ);  // ���� Ÿ��
        CreateTile(playerX - 10, playerZ);  // ���� Ÿ��
        CreateTile(playerX, playerZ + 10);  // ���� Ÿ��
        CreateTile(playerX, playerZ - 10);  // ���� Ÿ��
        CreateTile(playerX, playerZ);       // �÷��̾� ��ġ Ÿ�� ����
    }

    void CreateTile(int x, int z)
    {
        if (TileExists(x, z))
        {
            return;
        }

        // ���ο� Ÿ�� ����
        GameObject randomTilePrefab = tilePrefabs[Random.Range(0, tilePrefabs.Count)];
        GameObject newTile = Instantiate(randomTilePrefab, new Vector3(x, 0, z), Quaternion.identity, mapParent.transform);
        newTiles.Add(newTile);

        // ���ڷ��̼� �߰�
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
