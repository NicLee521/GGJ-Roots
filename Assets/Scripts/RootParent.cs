using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootParent : MonoBehaviour
{
    MapController map;
    public Root root;
    void Start()
    {
        map = GameObject.FindObjectOfType<MapController>();
    }

    void SetRootsForTiles() {
        foreach(Vector3Int tile in root.connectedTiles) {
            map.mapDict[tile].SetAttachedRoots(gameObject.transform.GetChild(0).gameObject);
        }
    }

    void OnCollisionEnter(Collision col) {
        if(col.gameObject.tag == "Root") {
            if(!root.connections.Contains(col.gameObject)) {
                root.connections.Add(col.gameObject.transform.GetChild(0).gameObject);
            }
            SetRootsForTiles();
        }
    }
}
