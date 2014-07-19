using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleRoomManager : MonoBehaviourBase {

    BlockSpawner[] _spawners;
    readonly List<PushBlock> spawnedBlocks = new List<PushBlock>();

    private GameObject _blockPrefab;

    void Awake()
    {
        _blockPrefab = Resources.Load("Prefabs/PushBlock", typeof (GameObject)) as GameObject;
        _spawners = GetComponentsInChildren<BlockSpawner>();
    }

    void Start()
    {
        StartCoroutine(SpawnBlocks());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(SpawnBlocks());
        }
    }

    IEnumerator SpawnBlocks()
    {
        Coroutine despawnRoutine = null;
        foreach (var spawnedBlock in spawnedBlocks)
        {
            //Let's remember the last coroutine we created so we can wait for it to finish
            despawnRoutine = StartCoroutine(spawnedBlock.Die());
        }
        //I want to wait until the despawn coroutines finish
        //They should all be the same length ~for now~, so let's just wait for the last one to finish
        if (despawnRoutine != null){ yield return despawnRoutine; }
        
        spawnedBlocks.Clear();

        foreach (var blockSpawner in _spawners)
        {
            var newBlock = Instantiate(_blockPrefab, blockSpawner.transform.position, Quaternion.identity) as GameObject;
            if (newBlock != null) spawnedBlocks.Add(newBlock.GetComponentInChildren<PushBlock>());
        }

        if (spawnedBlocks.Count == 0) yield break;
        yield return StartCoroutine(spawnedBlocks[0].SettleBlocks());
    }


}
