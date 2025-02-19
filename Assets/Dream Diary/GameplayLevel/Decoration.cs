using System;
using UnityEngine;

[Serializable]
public class WallDecoration : Decoration {

}

[Serializable]
public class GroundDecoration : Decoration {

}

[Serializable]
public class GroundWallDecoration : Decoration {

}

[Serializable]
public class Decoration {
    public GameObject Prefab => prefab;
    public float Probability => probability;
    public bool CanRandomizeRotation => canRandomRotate;

    [SerializeField] GameObject prefab;
    [Range(0f, 1f)]
    [SerializeField] float probability;
    [SerializeField] bool canRandomRotate;
}
