using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelDecorator : MonoBehaviour {

    public Action OnEndGameReached;

    [SerializeField] LevelDecorationsConfig levelDecorationsConfig;
    [Tooltip("It must be even value because portals are working in pairs")]
    [SerializeField] int portalsCount;

    List<LevelCell> _levelCellsList;
    List<Vector3> _portalPositions;
    EndGamePortal _spawnedEndGamePortal;

    public void Decorate(LevelCell[,] levelGrid, int seed) {
        
        if(levelGrid == null || levelGrid.Length < 1) return;
        _levelCellsList = new List<LevelCell>();

        for (int i = 0; i < levelGrid.GetLength(0); i++) {
            for(int j = 0; j < levelGrid.GetLength(1); j++) {
                if(levelGrid[i, j] != null) {
                    _levelCellsList.Add(levelGrid[i, j]);
                }
            }
        }

        SpawnPortals(seed);
        SpawnDecorations(seed);
    }

    void SpawnPortals(int seed) {

        System.Random random = new System.Random(seed);

        int portalsToSpawnCount = portalsCount;
        if (portalsToSpawnCount < 2) {
            Debug.Log($"Portals count is smaller than 2.");
            return;
        }

        if (portalsToSpawnCount % 2 != 0) {
            Debug.Log($"Portals count is odd value so it will be decreased by 1.");
            portalsToSpawnCount = portalsCount - 1;
        }

        var spawnedPortals = new List<Portal>();
        _portalPositions = new();
        HashSet<int> usedIndexes = new HashSet<int>();

        while (usedIndexes.Count < _levelCellsList.Count) {
            int index;
            do {
                index = random.Next(0, _levelCellsList.Count);
            } while (usedIndexes.Contains(index));

            usedIndexes.Add(index);

            var cell = _levelCellsList[index];
            var portalPosition = cell.transform.position;

            if(_spawnedEndGamePortal == null) {
                if (IsValidPosition(portalPosition, _portalPositions)) {
                    _portalPositions.Add(portalPosition);
                    _spawnedEndGamePortal = Instantiate(levelDecorationsConfig.EndGamePortal, cell.transform);
                    _spawnedEndGamePortal.OnEndGameReached += OnEndGameReached;
                    if (cell.IsEastWallActive && cell.IsWestWallActive) {
                        _spawnedEndGamePortal.transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                }
            } 
            else if (spawnedPortals.Count < portalsToSpawnCount) {
                if (IsValidPosition(portalPosition, _portalPositions)) {
                    _portalPositions.Add(portalPosition);
                    var portalSpawned = Instantiate(levelDecorationsConfig.Portal, cell.transform);
                    if (cell.IsEastWallActive && cell.IsWestWallActive) {
                        portalSpawned.transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                    portalSpawned.SetParentCell(cell);
                    spawnedPortals.Add(portalSpawned);
                }
            }
        }

        if (ValidatePortals(spawnedPortals)) {
            ConnectPortals(spawnedPortals);
        }

    }

    void SpawnDecorations(int seed) {
        System.Random random = new System.Random(seed);

        foreach (var cell in _levelCellsList) {
            DecorateWall(cell, random);
            DecorateGround(cell, random);

            //We want to decorate items on floor only when there is no portal
            if (_portalPositions.Contains(cell.transform.position) == false) {
                DecorateWithItems(cell, random);
            }
        }
    }

    void DecorateWall(LevelCell cell, System.Random random) {
        if(random.NextDouble() < levelDecorationsConfig.WallDecorationsProbability) {
            var decoration = GetRandomDecoration<WallDecoration>(levelDecorationsConfig.WallDecorations, random);
            if (decoration != null) {
                var decorationParent = cell.GetRandomWallDecorationTransform(random);
                var spawnedDecoration = Instantiate(decoration.Prefab, decorationParent);
                if (decoration.CanRandomizeRotation) {
                    var randomRotation = random.Next(0, 360);
                    spawnedDecoration.transform.rotation = Quaternion.Euler(0f, randomRotation, 0f);
                }
            }
        }
    }

    void DecorateGround(LevelCell cell, System.Random random) {
        if (random.NextDouble() < levelDecorationsConfig.GroundDecorationsProbability) {
            var decoration = GetRandomDecoration<GroundDecoration>(levelDecorationsConfig.GroundDecoration, random);
            if (decoration != null) {
                var decorationParent = cell.GetRandomGroundDecorationTransform(random);
                var spawnedDecoration = Instantiate(decoration.Prefab, decorationParent);
                if (decoration.CanRandomizeRotation) {
                    var randomRotation = random.Next(0, 360);
                    spawnedDecoration.transform.rotation = Quaternion.Euler(0f, randomRotation, 0f);
                }
            }
        }
    }

    void DecorateWithItems(LevelCell cell, System.Random random) {
        if (random.NextDouble() < levelDecorationsConfig.GroundWallDecorationsProbability) {
            var decoration = GetRandomDecoration<GroundWallDecoration>(levelDecorationsConfig.GroundWallDecoration, random);
            if (decoration != null) {
                var decorationParent = cell.GetRandomGroundWallDecorationTransform(random);
                var spawnedDecoration = Instantiate(decoration.Prefab, decorationParent);
                if (decoration.CanRandomizeRotation) {
                    var randomRotation = random.Next(0, 360);
                    spawnedDecoration.transform.rotation = Quaternion.Euler(0f, randomRotation, 0f);
                }

                if(random.Next(0, 2) > 0) {
                    var decorationParent2 = cell.GetRandomGroundWallDecorationTransform(random, decorationParent);
                    if(decorationParent2 == null) {
                        return;
                    }
                    var spawnedDecoration2 = Instantiate(decoration.Prefab, decorationParent);
                    if (decoration.CanRandomizeRotation) {
                        var randomRotation = random.Next(0, 360);
                        spawnedDecoration.transform.rotation = Quaternion.Euler(0f, randomRotation, 0f);
                    }
                }
            }
        }
    }

    T GetRandomDecoration<T>(List<T> decorations, System.Random random) {
        List<Decoration> listOfDecorations = decorations.Cast<Decoration>().ToList();
        if(listOfDecorations == null) {
            return default(T);
        }

        float totalWeight = listOfDecorations.Sum(d => d.Probability);
        float roll = (float)random.NextDouble() * totalWeight; 

        float cumulative = 0f;
        for (int i = 0; i < listOfDecorations.Count; i++) {
            cumulative += listOfDecorations[i].Probability;

            if (roll < cumulative)
                return decorations[i];
        }

        return default;
    }

    bool ValidatePortals(List<Portal> portalsToConnect) {
        if (portalsToConnect != null && portalsToConnect.Count > 0) {
            if(portalsToConnect.Count % 2 != 0) {
                var index = portalsToConnect.Count - 1;
                Destroy(portalsToConnect[index]);
                portalsToConnect.RemoveAt(index);
            }

            return true;
        } else {
            return false;
        }
    }

    void ConnectPortals(List<Portal> portalsToConnect) {
        if (portalsToConnect == null || portalsToConnect.Count < 2) {
            Debug.LogError($"Portals count:{portalsToConnect?.Count} - Cannot connect portals because there are not enough spawned portals");
            return;
        }

        for (int i = 0; i < portalsToConnect.Count; i += 2) {
            if (portalsToConnect.Count <= i + 1) {
                Debug.LogError($"Index:{i +1} - Cannot connect last portal because there is no second portal for it");
                return;
            }
            portalsToConnect[i].SetExitPortal(portalsToConnect[i + 1]);
            portalsToConnect[i + 1].SetExitPortal(portalsToConnect[i]);
        }
    }

    public void Cleanup() {
        _levelCellsList.Clear();
        _portalPositions?.Clear();
        _spawnedEndGamePortal = null;
    }

    bool IsValidPosition(Vector3 pos, List<Vector3> positions) {
        //TODO Calculate it based on amount of portals and size of level
        var minDistance = 20f;
        foreach (var placedPortal in positions) {
            if (Vector3.Distance(placedPortal, pos) < minDistance)
            {
                return false;
            }
        }
        return true;
    }
}
