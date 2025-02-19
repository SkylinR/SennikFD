using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public int Seed { get; private set; } = 0;
    public bool IsLevelGenerated => _mazeParent != null;
    public Vector2 PrefabSize => prefabSize;
    [SerializeField] LevelCell prefab;
    [SerializeField] Vector2 prefabSize;
    [SerializeField] int levelWidth;
    [SerializeField] int levelDepth;
    [SerializeField] LevelDecorator levelDecorator;

    [Space(10)]
    [Tooltip("Seed for deterministic maze generating, for random seed set value to 0")]
    [SerializeField] int generatorSeed;

    LevelCell[,] _levelGrid;
    private GameObject _mazeParent;

    public Vector3 GetRandomCellCenter() {

        System.Random random = new System.Random();
        var randomCell = _levelGrid[random.Next(0, levelWidth), random.Next(0, levelDepth)];
        return randomCell.transform.position;
    }

    public int GetGeneratedMazeSeed() {
        if (_mazeParent != null) {
            return Seed;
        } else {
            return -1;
        }
    }

    public void Generate() {
        Generate(generatorSeed);
        levelDecorator.Decorate(_levelGrid, Seed);
    }

    public void Generate(int seed) {
        ClearLevel();
        
        if (seed == 0) {
            Seed = Random.Range(1, int.MaxValue);
        } else {
            Seed = seed;
        }

        _levelGrid = new LevelCell[levelWidth, levelDepth];
        _mazeParent = new GameObject($"Maze {Seed}");
        _mazeParent.transform.SetParent(transform);
        _mazeParent.transform.localPosition = Vector3.zero;

        var offset = new Vector2(
            -prefabSize.x / 2 * levelWidth,
            -prefabSize.y / 2 * levelDepth
        );
        for (var x = 0; x < levelWidth; x++) {
            for (var y = 0; y < levelDepth; y++) {
                var position = new Vector3(
                    offset.x + x * prefabSize.x,
                    _mazeParent.transform.position.y,
                    offset.y + y * prefabSize.y
                );

                _levelGrid[x, y] = Instantiate(prefab, position, rotation: Quaternion.identity, _mazeParent.transform);
                _levelGrid[x, y].SetupIndexes(x, y);
            }
        }

        System.Random random = new System.Random(Seed);
        GenerateMaze(null, _levelGrid[0, 0], random);
    }

    public void Clear() {
        Seed = 0;
        ClearLevel();
        levelDecorator.Cleanup();
    }

    void GenerateMaze(LevelCell previousCell, LevelCell currentCell, System.Random random) {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        LevelCell nextcell;
        do {
            nextcell = GetNextUnvisitedCell(currentCell, random);

            if (nextcell != null) {
                GenerateMaze(currentCell, nextcell, random);
            }
        }
        while (nextcell != null);
    }

    LevelCell GetNextUnvisitedCell(LevelCell currentCell, System.Random random) {
        var unvisitedCells = GetUnvisitedCells(currentCell);
        return unvisitedCells.OrderBy(_ => random.Next(0, 10)).FirstOrDefault();
    }

    IEnumerable<LevelCell> GetUnvisitedCells(LevelCell currentCell) {

        var cells = new List<LevelCell>();

        if (currentCell.X + 1 < levelWidth) {
            var cellEast = _levelGrid[currentCell.X + 1, currentCell.Z];

            if (cellEast.IsVisited == false) {
                cells.Add(cellEast);
            }
        }

        if (currentCell.X - 1 >= 0) {
            var cellWest = _levelGrid[currentCell.X - 1, currentCell.Z];

            if (cellWest.IsVisited == false) {
                cells.Add(cellWest);
            }
        }

        if (currentCell.Z + 1 < levelDepth) {
            var cellNorth = _levelGrid[currentCell.X, currentCell.Z + 1];

            if (cellNorth.IsVisited == false) {
                cells.Add(cellNorth);
            }
        }

        if (currentCell.Z - 1 >= 0) {
            var cellToSouth = _levelGrid[currentCell.X, currentCell.Z - 1];

            if (cellToSouth.IsVisited == false) {
                cells.Add(cellToSouth);
            }
        }

        return cells;
    }

    void ClearWalls(LevelCell previousCell, LevelCell currentCell) {
        if (previousCell == null) {
            return;
        }

        if (previousCell.X < currentCell.X) {
            previousCell.ClearWall(LevelCell.WallSide.East);
            currentCell.ClearWall(LevelCell.WallSide.West);
            return;
        }

        if (previousCell.X > currentCell.X) {
            previousCell.ClearWall(LevelCell.WallSide.West);
            currentCell.ClearWall(LevelCell.WallSide.East);
            return;
        }

        if (previousCell.Z > currentCell.Z) {
            previousCell.ClearWall(LevelCell.WallSide.South);
            currentCell.ClearWall(LevelCell.WallSide.North);
            return;
        }

        if (previousCell.Z < currentCell.Z) {
            previousCell.ClearWall(LevelCell.WallSide.North);
            currentCell.ClearWall(LevelCell.WallSide.South);
            return;
        }
    }

    void ClearLevel() {
        _levelGrid = null;
        if (_mazeParent == null) {
            return;
        }
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying) {
            DestroyImmediate(_mazeParent);
        } else {
            Destroy(_mazeParent);
        }
#else
        Destroy(_mazeParent);
#endif
    }
}
