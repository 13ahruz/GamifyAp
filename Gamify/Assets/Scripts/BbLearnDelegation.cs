using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BbLearnDelegation : MonoBehaviour
{
    public GameObject spawnObject;
    public List<GameObject> spawns = new List<GameObject>();

    public void CreateSpawn()
    {
        var e = Instantiate(spawnObject.transform,
                        transform.position,
                        transform.rotation).gameObject;
        spawns.Add(e);
    }

    public void DeleteSpawn(int spawnIndex)
    {
        var e = spawns[spawnIndex];
        Destroy(e);
        spawns.RemoveAt(spawnIndex);
    }
}