using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public struct Hex {
    Vector3 topPoint;
    Vector3 topRightPoint;
    Vector3 botRightPoint;
    Vector3 botPoint;
    Vector3 botLeftPoint;
    Vector3 topLeftPoint;
    public Vector3 center;
    public Vector3[] pointPositions;
    public Hex(Vector3 tpos) {
        this.center = new Vector3(tpos.x, tpos.y ,0);;
        this.topPoint = new Vector3(tpos.x, tpos.y + .5f,0);
        this.topRightPoint = new Vector3(tpos.x + .5f, tpos.y + .25f, 0);
        this.botRightPoint = new Vector3(tpos.x + .5f, tpos.y - .25f, 0);
        this.botPoint = new Vector3(tpos.x, tpos.y - .5f, 0);
        this.botLeftPoint = new Vector3(tpos.x - .5f, tpos.y - .25f, 0);
        this.topLeftPoint = new Vector3(tpos.x - .5f, tpos.y + .25f, 0);
        this.pointPositions = new Vector3[]{this.topPoint, this.topRightPoint, this.botRightPoint, this.botPoint, this.botLeftPoint, this.topLeftPoint};
    }

    public Vector2 GetCenterToPointByIndex(int index) {
        return this.pointPositions[index] - this.center;
    }
}
public class PlayerController : MonoBehaviour
{
    

    [SerializeField] Tilemap tileMap;
    [SerializeField] LineRenderer lineRenderer1;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] GameObject root;


    // Update is called once per frame
    void Update()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tilePos = tileMap.WorldToCell(worldPoint);
        Vector3 tpos = tileMap.GetCellCenterWorld(tilePos);
        Hex hex = new Hex(tpos);
        lineRenderer1.positionCount  = hex.pointPositions.Length;
        lineRenderer1.loop = true;
        lineRenderer1.SetPositions(hex.pointPositions);
        lineRenderer.positionCount  = 2;
        Vector3[] linePos = CheckMouse(worldPoint, hex);
        lineRenderer.SetPositions(linePos);
        if(Input.GetMouseButtonDown(0)) {
            Mesh lineMesh = new Mesh();
            lineRenderer.BakeMesh(lineMesh);
            root.GetComponent<MeshCollider>().sharedMesh = lineMesh;
            GameObject newRoot = Instantiate(root, linePos[0], Quaternion.identity);
        }
    }

    Vector3[] CheckMouse(Vector3 worldPoint, Hex hex) {
        for(int i = 0; i < hex.pointPositions.Length; i++) {
            int k = i + 1;
            if(k == hex.pointPositions.Length) {
                k = 0;
            }
            Vector2 positionIToCenter = hex.GetCenterToPointByIndex(i);
            Vector2 postitionKToCenter = hex.GetCenterToPointByIndex(k);
            float angleArea = Vector2.Angle(positionIToCenter, postitionKToCenter);
            if(CheckIfInTriangle(worldPoint, hex.center,  positionIToCenter, postitionKToCenter, angleArea)) {
                return new Vector3[]{hex.pointPositions[i], hex.pointPositions[k]};
            }
        }

        return new Vector3[0];
    }

    bool CheckIfInTriangle(Vector2 worldPoint, Vector2 center, Vector2 first, Vector2 second, float angle) {
        float angleOfFirst = Vector2.Angle(worldPoint - center, first );
        float angleOfSecond = Vector2.Angle(worldPoint - center, second);
        return (angleOfFirst < angle && angleOfSecond < angle);
    }
}
