using System;
using System.Linq;
using Assets.Scripts.Repository;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleRoomManager : RoomManager {

    BlockSpawner[] _spawners;
    EnemySpawner[] _enemySpawners;
    readonly List<PushBlock> _spawnedBlocks = new List<PushBlock>();
    readonly List<SquishEnemy> _spawnedEnemies = new List<SquishEnemy>();

    private GameObject _blockPrefab;
    private GameObject _enemyPrefab;
    private GameObject _iceEnemyPrefab;
    private int enemyCount;
    private int squishCount = 0;


    //private EntranceDoorway _entrance;
    private ExitDoorway _exit;
    //private RoomWorkflow _workflow;

    //public int RoomIndex;

    new void Awake()
    {
        base.Awake();
        _blockPrefab = Resources.Load("Prefabs/PushBlock", typeof (GameObject)) as GameObject;
        _enemyPrefab = Resources.Load("Prefabs/FireEnemy", typeof (GameObject)) as GameObject;
        _iceEnemyPrefab = Resources.Load("Prefabs/IceEnemy", typeof(GameObject)) as GameObject;

        _spawners = GetComponentsInChildren<BlockSpawner>();
        _enemySpawners = GetComponentsInChildren<EnemySpawner>();

        //_workflow = GetComponentInParent<RoomWorkflow>();
        //_entrance = GetComponentInChildren<EntranceDoorway>();
        _exit = GetComponentInChildren<ExitDoorway>();
    }

    void Start()
    {
        StartCoroutine(RoomInit());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(RoomInit());
        }
    }


    IEnumerator RoomInit()
    {
        yield return StartCoroutine(SpawnBlocks());
        yield return StartCoroutine(SpawnEnemies());
    }

    public override void OnRoomEnter()
    {
        var roomText = DialogRepository.Instance.GetDialogBit(String.Format("Taunt{0}", RoomIndex.ToString("D2")), "01");
        if (roomText != null)
        {
            StartCoroutine(PassiveTextboxDisplay.Instance.DisplayText(roomText.Text, roomText.Name, () => { }));
        }
        base.OnRoomEnter();
    }


    public override void OnRoomExit()
    {

    }

    /*public Transform GetEntrance()
    {
        return _entrance.transform;
    }*/

    public void ActivateExit()
    {
        if (_exit == null) return;
        _exit.Activate();
    }

    /*public RoomManager GetNextRoom()
    {
        return _workflow.NextRoom(RoomIndex);
    }*/

    public void UpdateRoomCompletion()
    {
        bool roomComplete = true;
        foreach (var spawnedEnemy in _spawnedEnemies)
        {
            if (spawnedEnemy.IsSquished) continue;
            roomComplete = false;
            break;
        }

        if (roomComplete)
        {
            if (_spawnedEnemies.Count != 0)
            {
                var roomText =
                    DialogRepository.Instance.GetDialogBit(String.Format("Taunt{0}", RoomIndex.ToString("D2")), "02");
                if (roomText != null)
                {
                    StartCoroutine(PassiveTextboxDisplay.Instance.DisplayText(roomText.Text, roomText.Name, () => { }));
                }
            }
            //Activate door
            ActivateExit();
            Debug.Log("Room Complete!!!");
            return;
        }
        Debug.Log("Room Not Compelte");
    }

    IEnumerator SpawnEnemies()
    {
        Coroutine despawnRoutine = null;
        foreach (var spawnedEnemy in _spawnedEnemies)
        {
            //Let's remember the last coroutine we created so we can wait for it to finish
            despawnRoutine = StartCoroutine(spawnedEnemy.Die());
        }
        //I want to wait until the despawn coroutines finish
        //They should all be the same length ~for now~, so let's just wait for the last one to finish
        if (despawnRoutine != null) { yield return despawnRoutine; }

        _spawnedEnemies.Clear();

        enemyCount = 0; 
        foreach (var enemySpawner in _enemySpawners)
        {
            var prefab = enemySpawner.isIce ? _iceEnemyPrefab : _enemyPrefab;

            var newEnemy = Instantiate(prefab, enemySpawner.transform.position, Quaternion.identity) as GameObject;
            if (newEnemy == null) continue;

            _spawnedEnemies.Add(newEnemy.GetComponentInChildren<SquishEnemy>());
            newEnemy.transform.parent = transform;
            enemyCount++;
        }

        UpdateRoomCompletion();
    }

    IEnumerator SpawnBlocks()
    {
        Coroutine despawnRoutine = null;
        foreach (var spawnedBlock in _spawnedBlocks)
        {
            //Let's remember the last coroutine we created so we can wait for it to finish
            despawnRoutine = StartCoroutine(spawnedBlock.Die());
        }
        //I want to wait until the despawn coroutines finish
        //They should all be the same length ~for now~, so let's just wait for the last one to finish
        if (despawnRoutine != null){ yield return despawnRoutine; }
        
        _spawnedBlocks.Clear();

        foreach (var blockSpawner in _spawners)
        {
            var newBlock = Instantiate(_blockPrefab, blockSpawner.transform.position, Quaternion.identity) as GameObject;
            if (newBlock == null) continue;

            var newPushBlock = newBlock.GetComponentInChildren<PushBlock>();
            newPushBlock.isSlippery = blockSpawner.IsSlippery;
            _spawnedBlocks.Add(newPushBlock);
            newBlock.transform.parent = transform;
        }

        if (_spawnedBlocks.Count == 0) yield break;
        yield return StartCoroutine(_spawnedBlocks[0].SettleBlocks());
    }
}