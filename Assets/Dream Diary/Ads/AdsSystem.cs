using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AdsSystem : MonoBehaviour
{
    [SerializeField] StepTracker stepTracker;
    [SerializeField] int distanceToShowAd;
    [SerializeField] float durationOfAd;
    [SerializeField] Image ad;

    private void Awake() {
        ad.enabled = false;
    }

    public void Setup() {

        stepTracker.OnDistanceChanged += HandleOnDistanceChanged;
    }

    public void DisableAd() {
        ad.enabled = false;
    }

    private void HandleOnDistanceChanged(int distance) {
        if(distance >= distanceToShowAd) {
            stepTracker.OnDistanceChanged -= HandleOnDistanceChanged;
            StartCoroutine(ShowAdFor(durationOfAd));
        }
    }

    private IEnumerator ShowAdFor(float duration) {
        ad.enabled = true;
        yield return new WaitForSeconds(duration);
        ad.enabled = false;
    }
}
