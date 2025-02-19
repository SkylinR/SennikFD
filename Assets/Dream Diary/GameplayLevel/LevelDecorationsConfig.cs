using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LevelDecorationsConfig), menuName = "Config/LevelDecorationsConfig")]
public class LevelDecorationsConfig : ScriptableObject
{
    public Portal Portal => portal;
    public EndGamePortal EndGamePortal => endGamePortal;
    public List<WallDecoration> WallDecorations => wallDecorations;
    public List<GroundDecoration> GroundDecoration => groundDecorations;
    public List<GroundWallDecoration> GroundWallDecoration => groundWallDecorations;

    public float WallDecorationsProbability => wallDecorationsProbability;
    public float GroundDecorationsProbability => groundDecorationsProbability;
    public float GroundWallDecorationsProbability => groundWallDecorationsProbability;

    [Range(0f, 1f)]
    [SerializeField] float wallDecorationsProbability;
    [SerializeField] List<WallDecoration> wallDecorations;
    [Space(20)]
    [Range(0f, 1f)]
    [SerializeField] float groundDecorationsProbability;
    [SerializeField] List<GroundDecoration> groundDecorations;
    [Space(20)]
    [Range(0f, 1f)]
    [SerializeField] float groundWallDecorationsProbability;
    [SerializeField] List<GroundWallDecoration> groundWallDecorations;
    [Space(5)]
    [Header("Portals")]
    [SerializeField] Portal portal;
    [SerializeField] EndGamePortal endGamePortal;
    
}
