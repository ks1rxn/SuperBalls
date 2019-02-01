using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic {

    [RequireComponent(typeof(BallsPool))]
    public class BallsManager : MonoBehaviour {
        public event Action<Ball> PlayerKilledBall = delegate {  };

        [SerializeField, Range(0.1f, 6.0f)]
        private float _minBallSpawnDelay = 0.5f;
        [SerializeField, Range(0.1f, 6.0f)]
        private float _maxBallSpawnDelay = 1.5f;
        private float _timeToNextBallSpawn;
    
        private BallsPool _ballsPool;
        private readonly List<Ball> _balls = new List<Ball>();
        private readonly System.Random _randomGenerator = new System.Random();
    
        private void Awake() {
            _ballsPool = GetComponent<BallsPool>();
        }

        private void OnBallGoOffscreen(Ball ball) {
            RemoveBall(ball);
        }

        private void OnBallClicked(Ball ball) {
            PlayerKilledBall(ball);
            RemoveBall(ball);  
        }

        public void UpdateEntity(float proportinalTimeLeft, float deltaTime) {
            UpdateBalls(proportinalTimeLeft, deltaTime);
            UpdateBallsSpawner(deltaTime);
        }

        private void UpdateBalls(float proportinalTimeLeft, float deltaTime) {
            float speedModifier = 2 - proportinalTimeLeft;
            for (int i = _balls.Count - 1; i >= 0; i--) {
                _balls[i].UpdateEntity(deltaTime, speedModifier);
            }
        }
        
        private void UpdateBallsSpawner(float deltaTime) {
            _timeToNextBallSpawn -= deltaTime;
            if (_timeToNextBallSpawn <= 0) {
                SpawnBall();
                _timeToNextBallSpawn = _minBallSpawnDelay + (float) _randomGenerator.NextDouble() * (_maxBallSpawnDelay - _minBallSpawnDelay);
            }
        }
    
        private void SpawnBall() {
            Ball newBall = _ballsPool.Get();
            _balls.Add(newBall);
            
            float proportionalX = (float) _randomGenerator.NextDouble();
            float proportionalSize = (float) _randomGenerator.NextDouble();
            
            newBall.Spawn(proportionalX, proportionalSize);
            
            newBall.BallClicked += OnBallClicked;
            newBall.BallGoOffscreen += OnBallGoOffscreen;
        }

        private void RemoveBall(Ball ball) {
            ball.BallGoOffscreen -= OnBallGoOffscreen;
            ball.BallClicked -= OnBallClicked;
            
            _ballsPool.ReturnToPool(ball);
            _balls.Remove(ball);
        }
    
    }

}