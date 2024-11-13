using UnityEngine;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    public List<GameObject> monsterPrefabs;  // ���� ������ ����Ʈ
    public List<Monster> monsters = new List<Monster>();  // ������ ���� ����Ʈ
    public GameObject mapParent;             // ���� ������ �θ� ��ü

    // Ÿ�Ͽ��� ���� ����
    public void SpawnMonsterOnTile(Vector3 tilePosition)
    {
        tilePosition.y = 1f; // y���� 1�� �����Ͽ� ���Ͱ� �׻� y = 1���� �����ǵ��� ��

        int numberOfMonsters = Random.Range(0, 4);  // ���� ���� ����

        float spawnRadius = 5f;  // ���Ͱ� ������ ����

        for (int i = 0; i < numberOfMonsters; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius));
            Vector3 spawnPosition = tilePosition + randomOffset;

            // �������� ���͸� �����Ͽ� ����
            GameObject monsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Count)];
            GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity, mapParent.transform);

            Monster monsterScript = monster.GetComponent<Monster>();
            if (monsterScript != null)
            {
                monsterScript.player = Player.Instance; // Player ��ü�� �Ҵ�
                monsters.Add(monsterScript);  // ������ ���͸� ����Ʈ�� �߰�
            }
        }
    }

    public List<Monster> GetMonstersAtPosition(Vector3 position)
    {
        List<Monster> monstersAtPosition = new List<Monster>();

        foreach (Monster monster in monsters)
        {
            if (Vector3.Distance(monster.transform.position, position) < 1f)  // 1f �̳��� ��ġ�� ����
            {
                monstersAtPosition.Add(monster);
            }
        }

        return monstersAtPosition;
    }

    // ���Ͱ� ������ ����Ʈ���� ����
    public void RemoveMonster(Monster monster)
    {
        if (monsters.Count == 0)
        {
            Debug.LogWarning("���� ����Ʈ�� ��� �ֽ��ϴ�. ���͸� ������ �� �����ϴ�.");
            return;  
        }
        int index = monsters.IndexOf(monster);  // ������ �ε����� ã��
        if (index >= 0 && index < monsters.Count)
        {
            monsters.RemoveAt(index);  // ��ȿ�� �ε����� ��� ����
        }
    }

}
