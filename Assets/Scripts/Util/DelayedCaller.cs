using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class DelayedCaller : MonoBehaviour
    {
        [SerializeField] private UnityEvent _callDelayed;
        [SerializeField] private float _delayToCall;

        public void CallDelayed()
        {
            StartCoroutine(CallWithDelay());
        }
        private IEnumerator CallWithDelay()
        {
            yield return new WaitForSeconds(_delayToCall);

            _callDelayed?.Invoke();
        }
    }
}