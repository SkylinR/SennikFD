using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelCell : MonoBehaviour {

    public bool IsWestWallActive => westWall.activeSelf;
    public bool IsNorthWallActive => northWall.activeSelf;
    public bool IsEastWallActive => eastWall.activeSelf;
    public bool IsSouthWallActive => southWall.activeSelf;

    [SerializeField] List<Transform> groundDecorationsTransforms; 
    [SerializeField] List<Transform> wallDecorationsTransforms;
    [SerializeField] List<Transform> groundWallDecorationsTransforms;

    public enum WallSide {
        West,
        North,
        East,
        South
    }

    [SerializeField] GameObject westWall;
    [SerializeField] GameObject northWall;
    [SerializeField] GameObject eastWall;
    [SerializeField] GameObject southWall;

    public bool IsVisited { get; private set; }

    public int X { get; private set; }
    public int Z { get; private set; }

    public void SetupIndexes(int x, int z) {
        X = x;
        Z = z;
    }

    public void Visit() {
        IsVisited = true;
    }

    public void ClearWall(WallSide wallSide) {
        switch (wallSide) {
            case WallSide.West:
                westWall.SetActive(false);
                break;
            case WallSide.North:
                northWall.SetActive(false);
                break;
            case WallSide.East:
                eastWall.SetActive(false);
                break;
            case WallSide.South:
                southWall.SetActive(false);
                break;
        }
    }

    public Transform GetRandomWallDecorationTransform(System.Random random) {
        List<Transform> activeObjects = wallDecorationsTransforms.Where(obj => obj.gameObject.activeInHierarchy).ToList();

        int index = random?.Next(activeObjects.Count) ?? 0;
        if(activeObjects.Count < index) return null;
        return activeObjects[index];
    }

    public Transform GetRandomGroundDecorationTransform(System.Random random) {
        List<Transform> activeObjects = groundDecorationsTransforms.Where(obj => obj.gameObject.activeInHierarchy).ToList();

        int index = random?.Next(activeObjects.Count) ?? 0;
        return activeObjects[index];
    }

    public Transform GetRandomGroundWallDecorationTransform(System.Random random, Transform excluded = null) {
        List<Transform> activeObjects = groundWallDecorationsTransforms.Where(obj => obj.gameObject.activeInHierarchy && obj != excluded).ToList();

        if(activeObjects != null && activeObjects.Count > 0) {
            int index = random?.Next(activeObjects.Count) ?? 0;
            return activeObjects[index];
        } else {
            return null;
        }
    }
}
