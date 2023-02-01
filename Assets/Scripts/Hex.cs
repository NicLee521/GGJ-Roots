using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public struct Hex
{
    Vector3 topPoint;
    Vector3 topRightPoint;
    Vector3 botRightPoint;
    Vector3 botPoint;
    Vector3 botLeftPoint;
    Vector3 topLeftPoint;
    public Vector3 center;
    public Vector3[] pointPositions;
    public Hex(Vector3 tpos, Tilemap tileMap) {
        this.center = new Vector3(tpos.x, tpos.y ,0);;
        this.topPoint = new Vector3(tpos.x, tpos.y + tileMap.layoutGrid.cellSize.y/2,0);
        this.topRightPoint = new Vector3(tpos.x + tileMap.layoutGrid.cellSize.x/2, tpos.y + tileMap.layoutGrid.cellSize.y/4, 0);
        this.botRightPoint = new Vector3(tpos.x + tileMap.layoutGrid.cellSize.x/2, tpos.y - tileMap.layoutGrid.cellSize.y/4, 0);
        this.botPoint = new Vector3(tpos.x, tpos.y - tileMap.layoutGrid.cellSize.y/2, 0);
        this.botLeftPoint = new Vector3(tpos.x - tileMap.layoutGrid.cellSize.x/2, tpos.y - tileMap.layoutGrid.cellSize.y/4, 0);
        this.topLeftPoint = new Vector3(tpos.x - tileMap.layoutGrid.cellSize.x/2, tpos.y + tileMap.layoutGrid.cellSize.x/4, 0);
        this.pointPositions = new Vector3[]{this.topPoint, this.topRightPoint, this.botRightPoint, this.botPoint, this.botLeftPoint, this.topLeftPoint};
    }

    public Vector2 GetCenterToPointByIndex(int index) {
        return this.pointPositions[index] - this.center;
    }
}
