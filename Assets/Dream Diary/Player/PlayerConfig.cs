using UnityEngine;

[CreateAssetMenu(fileName = nameof(PlayerConfig), menuName = "Config/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [Tooltip("Initial player movment speed")]
    [SerializeField] float movementSpeed;
    float _sessionMovementSpeed = 0;

    public float GetMovementSpeed() {
        if (_sessionMovementSpeed == 0) {
            return movementSpeed;
        } else { 
            return _sessionMovementSpeed;
        }
    }

    public void SetMovementSpeed(float speed) {
        if(speed > 0) {
            _sessionMovementSpeed = speed;
        }
    }
}
