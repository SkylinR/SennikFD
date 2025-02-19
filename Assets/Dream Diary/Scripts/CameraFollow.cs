using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField]Transform target;
    [SerializeField] float smoothSpeed = 0.5f;
    [SerializeField] Vector3 offset;

    void LateUpdate() {

        if (target != null) {
            Quaternion rotation = Quaternion.Euler(0f, target.eulerAngles.y, 0f);

            Vector3 desiredPosition = target.position - rotation * offset;

            Vector3 origin = transform.position; 
            Vector3 direction = (target.position - origin).normalized; 
            float distance = Vector3.Distance(origin, target.position);

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
            transform.LookAt(target);
        }
    }

    public void SetTarget(Transform target) {
        this.target = target;
    }
}