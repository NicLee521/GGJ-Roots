using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[System.Serializable]
public struct Obstacle {
    public List<Vector3Int> obstaclePositions;
    public List<Vector3Int> triggerHexes;
    public Tile obstacleTile;
}
public class ObstacleController : MonoBehaviour
{
    public List<Obstacle> obstacles = new List<Obstacle>();
    public Tilemap tileMap;
    public MapController map;
    public TutorialController tutorial;
    public void IfTileIsTrigger(Vector3Int tilePos) {
        foreach(Obstacle obs in obstacles) {
            if(obs.triggerHexes.Exists(trigger => tilePos == trigger)) {
                if(tutorial != null && tutorial.step == 3) {
                    tutorial.PlayStep();
                }
                SetObstacle(obs);
                return;
            }
        }
    }

    private void SetObstacle(Obstacle obs) {
        foreach(Vector3Int pos in obs.obstaclePositions) {
            tileMap.SetTile(pos, obs.obstacleTile);
            TileData newTile =  map.mapDict[pos];
            newTile.tile = obs.obstacleTile;
            map.mapDict[pos] = newTile;
        }
    }

}
