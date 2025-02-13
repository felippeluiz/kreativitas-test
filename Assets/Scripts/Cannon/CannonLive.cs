using Gameflow;
using UnityEngine;
using UnityEngine.Events;

namespace Cannon
{
    public class CannonLive : MonoBehaviour
    {
        [SerializeField] private EndGame _endGame;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.gameObject.CompareTag("Enemy")) return;
        
            _endGame.Lose();
        }
    }
}
