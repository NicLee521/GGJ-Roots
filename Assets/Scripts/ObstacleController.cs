using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[System.Serializable]
public struct Obstacle {
    public List<Vector3Int> obstaclePositions;
    public List<Vector3Int> triggerHexes;
}
public class ObstacleController : MonoBehaviour
{
    public List<Obstacle> obstacles = new List<Obstacle>();
    public Tile blocker;
    public Tilemap tileMap;
    public void IfTileIsTrigger(Vector3Int tilePos) {
        foreach(Obstacle obs in obstacles) {
            if(obs.triggerHexes.Exists(trigger => tilePos == trigger)) {
                SetObstacle(obs);
                return;
            }
        }
    }

    private void SetObstacle(Obstacle obs) {
        foreach(Vector3Int pos in obs.obstaclePositions) {
            tileMap.SetTile(pos, blocker);
        }
    }

}
