using System.Collections;
using System.Collections.Generic;
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
    public bool isInBattle = false;  // 전투 중 상태
    public MonsterSpawner monsterSpawner;

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

    // 전투 시작 여부 체크
    public void CheckBattle()
    {
        if(monsterSpawner.monsters.Count >= 0)
        {
            isInBattle = true;
        }
        else
        {
            isInBattle = false;
        }
    }


    // 플레이어가 이동하면 해당 타일에서 몬스터를 확인하고 처리
    public void PlayerMoveEvent(Vector3 newPosition)
    {
        mapCreate.PlayerMoveRoomCreate(newPosition); // 새로운 타일 생성
        monsterSpawner.mapParent = mapCreate.mapParent; // mapParent를 MonsterSpawner에 할당
        monsterSpawner.SpawnMonsterOnTile(newPosition); // 이동한 타일에 몬스터를 스폰

        CheckBattle();  // 이동 후 몬스터 상태에 따라 전투 시작 여부를 체크
    }
}
