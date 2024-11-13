using UnityEngine;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    public List<GameObject> monsterPrefabs;  // 몬스터 프리팹 리스트
    public List<Monster> monsters = new List<Monster>();  // 스폰된 몬스터 리스트
    public GameObject mapParent;             // 몬스터 스폰될 부모 객체

    // 타일에서 몬스터 스폰
    public void SpawnMonsterOnTile(Vector3 tilePosition)
    {
        tilePosition.y = 1f; // y값을 1로 설정하여 몬스터가 항상 y = 1에서 스폰되도록 함

        int numberOfMonsters = Random.Range(0, 4);  // 몬스터 스폰 개수

        float spawnRadius = 5f;  // 몬스터가 스폰될 범위

        for (int i = 0; i < numberOfMonsters; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius));
            Vector3 spawnPosition = tilePosition + randomOffset;

            // 랜덤으로 몬스터를 선택하여 스폰
            GameObject monsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Count)];
            GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity, mapParent.transform);

            Monster monsterScript = monster.GetComponent<Monster>();
            if (monsterScript != null)
            {
                monsterScript.player = Player.Instance; // Player 객체를 할당
                monsters.Add(monsterScript);  // 스폰된 몬스터를 리스트에 추가
            }
        }
    }

    public List<Monster> GetMonstersAtPosition(Vector3 position)
    {
        List<Monster> monstersAtPosition = new List<Monster>();

        foreach (Monster monster in monsters)
        {
            if (Vector3.Distance(monster.transform.position, position) < 1f)  // 1f 이내에 위치한 몬스터
            {
                monstersAtPosition.Add(monster);
            }
        }

        return monstersAtPosition;
    }

    // 몬스터가 죽으면 리스트에서 제거
    public void RemoveMonster(Monster monster)
    {
        if (monsters.Count == 0)
        {
            Debug.LogWarning("몬스터 리스트가 비어 있습니다. 몬스터를 제거할 수 없습니다.");
            return;  
        }
        int index = monsters.IndexOf(monster);  // 몬스터의 인덱스를 찾음
        if (index >= 0 && index < monsters.Count)
        {
            monsters.RemoveAt(index);  // 유효한 인덱스일 경우 제거
        }
    }

}
