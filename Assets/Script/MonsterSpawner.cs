using UnityEngine;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    public List<GameObject> monsterPrefabs;  // ���� ������ ����Ʈ
    public GameObject mapParent;             // ���� ������ �θ� ��ü

    // Ÿ�Ͽ��� ���� ����
    public void SpawnMonsterOnTile(Vector3 tilePosition)
    {
        // Ÿ���� y���� 1�� �����Ͽ� ���Ͱ� �׻� y = 1���� �����ǵ��� ��
        tilePosition.y = 1f;

        // ���͸� ������ ���� (��: 3����)
        int numberOfMonsters = 3;

        // ���Ͱ� ������ ���� (�÷��̾� ��ġ �������� �ణ�� ���� �������� �ֱ�)
        float spawnRadius = 5f;

        // mapParent�� �Ҵ�Ǿ� ���� ������, �������� ����
        if (mapParent == null)
        {
            Debug.LogError("mapParent is not assigned!");
            return;
        }

        for (int i = 0; i < numberOfMonsters; i++)
        {
            // Ÿ���� �������� �����ϰ� �ణ�� �������� �༭ ���͸� ��ġ
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                0f,
                Random.Range(-spawnRadius, spawnRadius)
            );

            // ���� ���� ��ġ ���
            Vector3 spawnPosition = tilePosition + randomOffset;

            // �������� ���͸� �����Ͽ� ����
            GameObject monsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Count)];
            GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity, mapParent.transform);

            // ���� ���� �� Player ��ü �Ҵ�
            Monster monsterScript = monster.GetComponent<Monster>();
            if (monsterScript != null)
            {
                monsterScript.player = Player.Instance; // Player ��ü�� �Ҵ�
            }
        }
    }


}
