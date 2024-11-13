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
    public MonsterSpawner monsterSpawner;
    public PlayerController playerController;
    public RewardUI rewardUI;
    public bool isInBattle = false;  // ���� �� ����
    public bool autoPlay = false;

    private void Awake()
    {
        mapCreate = FindObjectOfType<MapCreate>();
        monsterSpawner = FindObjectOfType<MonsterSpawner>();
        playerController = FindObjectOfType<PlayerController>();
        rewardUI = FindObjectOfType<RewardUI>();

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

    void Update()
    {
        if (autoPlay)
        {
            InvokeRepeating("RandomMove", 0f, 1.5f); 
        }
        else
        {
            CancelInvoke("RandomMove");
        }
    }

    void RandomMove()
    {
        playerController.RandomMove();  // �÷��̾ �������� �̵�
    }
    public void EndBattle()
    {
         rewardUI.ShowRewardUI();
    }
    // ���� ���� ���� üũ
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

    public void StartAutoPlay()
    {
        if(autoPlay == false)
        {
            autoPlay = true;
        }
        else
        {
            autoPlay = false;
        }
    }
    // �÷��̾ �̵��ϸ� �ش� Ÿ�Ͽ��� ���͸� Ȯ���ϰ� ó��
    public void PlayerMoveEvent(Vector3 newPosition)
    {
        mapCreate.PlayerMoveRoomCreate(newPosition); // ���ο� Ÿ�� ����
        monsterSpawner.mapParent = mapCreate.mapParent; // mapParent�� MonsterSpawner�� �Ҵ�
        monsterSpawner.SpawnMonsterOnTile(newPosition); // �̵��� Ÿ�Ͽ� ���͸� ����

        CheckBattle();  // �̵� �� ���� ���¿� ���� ���� ���� ���θ� üũ
    }
}
