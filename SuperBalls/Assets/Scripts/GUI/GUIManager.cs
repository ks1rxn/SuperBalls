using GameLogic;
using TMPro;
using UnityEngine;

namespace GUI {

    public class GUIManager : MonoBehaviour {
        [SerializeField]
        private TextMeshProUGUI _currentPoints;
        [SerializeField]
        private TextMeshProUGUI _timeLeft;
        [SerializeField]
        private GameFinishedPanel _finishPanel;
    
        [SerializeField]
        private GameController _gameController;

        private void Awake() {
            _gameController.GameStarted += OnGameStart;
            _gameController.GameFinished += OnGameFinish;
        }

        private void OnDestroy() {
            _gameController.GameStarted -= OnGameStart;
            _gameController.GameFinished -= OnGameFinish;
        }
    
        private void OnGameStart() {
            _finishPanel.Hide();
        }

        private void OnGameFinish(int finalPoints) {
            _finishPanel.Show(finalPoints);
        }
    
        private void Update() {
            _currentPoints.text = _gameController.CurrentPoints.ToString();
            _timeLeft.text = _gameController.TimeLeft.ToString("F0");
        }
    
    }

}