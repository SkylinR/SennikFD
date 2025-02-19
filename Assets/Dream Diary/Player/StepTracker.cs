using System;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class StepTracker : MonoBehaviour {
    public Action<int> OnDistanceChanged;

    private Vector3 _lastPosition;
    private float _totalDistance = 0f;
    private int _roundedDistance = 0;

    private Transform _transformToWatch;

    public void Setup(Transform transformToWatch) {
        _totalDistance = 0;
        _transformToWatch = transformToWatch;
        _lastPosition = _transformToWatch.position;
    }

    private void LateUpdate() {
        if (_transformToWatch == null) return;

        float distance = Vector3.Distance(_lastPosition, _transformToWatch.position);
        _totalDistance += distance;
        _lastPosition = _transformToWatch.position;

        int newRoundedDistance = Mathf.FloorToInt(_totalDistance);
        if (newRoundedDistance > _roundedDistance) {
            _roundedDistance = newRoundedDistance;
            OnDistanceChanged?.Invoke(_roundedDistance);
        }
    }
}
