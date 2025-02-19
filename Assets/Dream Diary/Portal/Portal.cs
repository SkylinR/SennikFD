using UnityEngine;

public class Portal : MonoBehaviour {
    public Transform SpawnPoint => spawnPoint;
    public Transform SpawnPoint2 => spawnPoint2;

    [SerializeField] Portal exitPortal;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform spawnPoint2;

    LevelCell _parent;

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            var playerController = other.GetComponent<PlayerController>();
            var exitPoint = exitPortal.SpawnPoint.position;
            
            exitPortal.SpawnPoint.gameObject.SetActive(true);
            exitPortal.SpawnPoint2.gameObject.SetActive(true);

            if (_parent != null) {
                if (transform.rotation.eulerAngles.y == 90 && _parent.IsNorthWallActive && !_parent.IsSouthWallActive) {
                    exitPoint = exitPortal.SpawnPoint2.position;
                    exitPortal.SpawnPoint.gameObject.SetActive(false);
                }
                else if(transform.rotation.eulerAngles.y == 0 && _parent.IsWestWallActive && !_parent.IsEastWallActive) {
                    exitPoint = exitPortal.SpawnPoint2.position;
                    exitPortal.SpawnPoint.gameObject.SetActive(false);
                } else {
                    exitPortal.SpawnPoint2.gameObject.SetActive(false);
                }
            }

            exitPoint.y = other.transform.position.y;
            other.transform.position = exitPoint;

            if (exitPortal.transform.rotation.eulerAngles.y == 90) {
                playerController.SetRotationY(0);
            } else {
                playerController.SetRotationY(-90);
            }
        }
    }

    public void SetParentCell(LevelCell cell) {
        _parent = cell;
    }

    public void SetExitPortal(Portal exitPortal) {
        this.exitPortal = exitPortal;
    }
}
