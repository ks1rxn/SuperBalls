using System.Collections.Generic;
using UnityEngine;

namespace GameLogic {

    public class BallsPool : MonoBehaviour {
        [SerializeField]
        private Ball _ballPrefab;
    
        private readonly Queue<Ball> _balls = new Queue<Ball>();

        public Ball Get() {
            if (_balls.Count == 0) {
                AddBall();    
            }
            return _balls.Dequeue();
        }

        public void ReturnToPool(Ball ball) {
            ball.gameObject.SetActive(false);
            _balls.Enqueue(ball);
        }
    
        private void AddBall() {
            Ball ball = Instantiate(_ballPrefab, transform);
            ball.gameObject.SetActive(false);
            _balls.Enqueue(ball); 
        }
    
    }

}