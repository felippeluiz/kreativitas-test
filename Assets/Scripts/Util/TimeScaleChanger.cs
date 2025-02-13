using System.Collections;
using UnityEngine;

namespace Gameflow
{
    public class TimeScaleChanger : MonoBehaviour {
        private void Awake()
        {
            Application.targetFrameRate = 60;
            Time.timeScale = 1f;
        }
        public void ChangeTimeScale(float timeScale)
        {
            Time.timeScale = timeScale;

            StartCoroutine(ResetTimeScale(1f));
        }
        private IEnumerator ResetTimeScale(float timeCountdown)
        {
            yield return new WaitForSecondsRealtime(timeCountdown);

            Time.timeScale = 1f;
        }
    }
}