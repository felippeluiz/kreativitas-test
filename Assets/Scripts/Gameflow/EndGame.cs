using UnityEngine;
using UnityEngine.Events;

namespace Gameflow
{
    public class EndGame : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onEndGame, _onLose, _onWin;
        private bool _gameRunning = true;
        public void Win()
        {
            if(!_gameRunning) return;
            _gameRunning = false;
            
            _onEndGame.Invoke();
            _onWin.Invoke();
        }

        public void Lose()
        {
            if(!_gameRunning) return;
            _gameRunning = false;
            
            _onEndGame.Invoke();
            _onLose.Invoke();
        }
    }
}