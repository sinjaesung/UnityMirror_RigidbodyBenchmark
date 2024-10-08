using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("")]
public class RigidbodyBenchmarkNetworkManager : NetworkManager
{
    [Header("Spawns")]
    public GameObject spawnPrefab;
    public int spawnAmount = 2000;
    public float interleave = 2;

    void SpawnAll()
    {
        Debug.Log("RigidbodyBenchmarkManager SpawnAll>>");

        // calculate sqrt so we can spawn N * N = Amount
        float sqrt = Mathf.Sqrt(spawnAmount);

        // calculate spawn xz start positions
        // based on spawnAmount * distance
        float offset = -sqrt / 2 * interleave;
        Debug.Log("RigidbodyBenchmarkManager SpawnAll>>"+ offset);
        // spawn exactly the amount, not one more.
        int spawned = 0;
        for (int spawnX = 0; spawnX < sqrt; ++spawnX)
        {
            for (int spawnZ = 0; spawnZ < sqrt; ++spawnZ)
            {
                // spawn exactly the amount, not any more
                // (our sqrt method isn't 100% precise)
                if (spawned < spawnAmount)
                {
                    // instantiate & position
                    GameObject go = Instantiate(spawnPrefab);
                    float x = offset + spawnX * interleave;
                    float z = offset + spawnZ * interleave;
                    go.transform.position = new Vector3(x, 0, z);
                    Debug.Log(spawned+"| RigidbodyBenchmarkManager SpawnAll>>" + new Vector3(x, 0, z));

                    // spawn
                    NetworkServer.Spawn(go);
                    ++spawned;
                }
            }
        }
    }

    public override void OnStartServer()
    {
        Debug.Log("RigidbodyBenchmarkManager OnStartServer>>");
        base.OnStartServer();
        SpawnAll();
    }
}