using System;
using UnityEngine;

namespace GameLogic {

    [RequireComponent(typeof(BallsManager))]
    public class GameController : MonoBehaviour {
        public event Action<int> GameFinished = delegate { };
        public event Action GameStarted = delegate { };
    
        public float TimeLeft { get; private set; }
        public int CurrentPoints { get; private set; }
        
        [SerializeField]
        private int _totalGameTime;
        private BallsManager _ballsManager;
        
        private GameState _gameState;
    
        private void Awake() {
            _ballsManager = GetComponent<BallsManager>();
            _totalGameTime = Mathf.Max(_totalGameTime, 1);
        }

        private void Start() {
            _gameState = GameState.InProgress;
            
            TimeLeft = _totalGameTime;
            CurrentPoints = 0;
            
            _ballsManager.PlayerKilledBall += OnPlayerKilledBall;
            
            GameStarted();
        }

        private void Update() {
            switch (_gameState) {
                case GameState.InProgress:
                    UpdateGameInProgressState();
                    break;
                default:
                    break;
            }
        }

        private void UpdateGameInProgressState() {
            _ballsManager.UpdateEntity(TimeLeft / _totalGameTime, Time.deltaTime);
            UpdateGameTime();
        }

        private void UpdateGameTime() {
            TimeLeft -= Time.deltaTime;
            if (TimeLeft <= 0) {
                OnTimeExpired();
            }
        }
        
        private void OnTimeExpired() {
            _gameState = GameState.Finished;
            _ballsManager.PlayerKilledBall -= OnPlayerKilledBall;
            GameFinished(CurrentPoints);
        }
    
        private void OnPlayerKilledBall(Ball ball) {
            CurrentPoints += ball.PointsForKill;
        }

        private enum GameState {
            InProgress,
            Finished
        }

    }

}