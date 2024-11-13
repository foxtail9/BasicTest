using UnityEngine;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    public List<GameObject> monsterPrefabs;  // 몬스터 프리팹 리스트
    public GameObject mapParent;             // 몬스터 스폰될 부모 객체

    // 타일에서 몬스터 스폰
    public void SpawnMonsterOnTile(Vector3 tilePosition)
    {
        // 타일의 y값을 1로 설정하여 몬스터가 항상 y = 1에서 스폰되도록 함
        tilePosition.y = 1f;

        // 몬스터를 스폰할 개수 (예: 3마리)
        int numberOfMonsters = 3;

        // 몬스터가 스폰될 범위 (플레이어 위치 기준으로 약간의 랜덤 오프셋을 주기)
        float spawnRadius = 5f;

        // mapParent가 할당되어 있지 않으면, 스폰하지 않음
        if (mapParent == null)
        {
            Debug.LogError("mapParent is not assigned!");
            return;
        }

        for (int i = 0; i < numberOfMonsters; i++)
        {
            // 타일을 기준으로 랜덤하게 약간씩 오프셋을 줘서 몬스터를 배치
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                0f,
                Random.Range(-spawnRadius, spawnRadius)
            );

            // 몬스터 스폰 위치 계산
            Vector3 spawnPosition = tilePosition + randomOffset;

            // 랜덤으로 몬스터를 선택하여 스폰
            GameObject monsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Count)];
            GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity, mapParent.transform);

            // 몬스터 스폰 후 Player 객체 할당
            Monster monsterScript = monster.GetComponent<Monster>();
            if (monsterScript != null)
            {
                monsterScript.player = Player.Instance; // Player 객체를 할당
            }
        }
    }


}
