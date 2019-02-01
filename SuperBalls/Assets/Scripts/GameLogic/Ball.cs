using System;
using UnityEngine;

namespace GameLogic {

    [RequireComponent(typeof(MeshRenderer))]
    public class Ball : MonoBehaviour {
        public event Action<Ball> BallGoOffscreen = delegate {  };
        public event Action<Ball> BallClicked = delegate {  };
        
        public int PointsForKill { get; private set; }
        
        private MeshRenderer _renderer;
        
        [SerializeField, Range(0.25f, 3)]
        private float _maxScale = 2;
        [SerializeField, Range(0.25f, 3)]
        private float _minScale = 0.5f;
        [SerializeField, Range(0.1f, 6)]
        private float _maxSpeed = 4;
        [SerializeField, Range(0.1f, 6)]
        private float _minSpeed = 2;
        
        private float _speed;

        private void Awake() {
            _renderer = GetComponent<MeshRenderer>();
        }
        
        public void Spawn(float proportionalX, float proportionalSize) {
            gameObject.SetActive(true);
            ConfigureBall(proportionalX, proportionalSize);
        }
        
        private void ConfigureBall(float proportionalX, float proportionalSize) {
            proportionalSize = Mathf.Clamp01(proportionalSize);
            proportionalX = Mathf.Clamp01(proportionalX);
            
            float scale = _minScale + proportionalSize * (_maxScale - _minScale);
            
            SetTransform(proportionalX, scale);
            SetSpeed(proportionalSize);
            SetPointsForKill(proportionalSize);
            SetColor(proportionalSize);
        }

        private void SetTransform(float proportionalX, float scale) {
            float screenWidthInUnits = Camera.main.orthographicSize * 2 * Screen.width / Screen.height;
            float y = -Camera.main.orthographicSize - scale / 2;
            float x = scale / 2.0f + proportionalX * (screenWidthInUnits - scale) - screenWidthInUnits * 0.5f;
            
            transform.position = new Vector3(x, y, 0);
            transform.localScale = new Vector3(scale, scale, scale);
        }

        private void SetSpeed(float proportionalSize) {
            _speed = _minSpeed + (1 - proportionalSize) * (_maxSpeed - _minSpeed);
        }

        private void SetPointsForKill(float proportionalSize) {
            PointsForKill = Mathf.RoundToInt((1 - proportionalSize) * 10);
        }
        
        private void SetColor(float proportionalSize) {
            float red = (1 - proportionalSize) * 2;
            float green = proportionalSize * 2;
            float blue = 0;
            Color color = new Color(red, green, blue);
            _renderer.material.SetColor("_MainColor", color);
        }
        
        public void UpdateEntity(float deltaTime, float speedModifier) {
            MoveBall(deltaTime, speedModifier);
            CheckScreenConstraints();
        }
        
        private void MoveBall(float deltaTime, float speedModifier) {
            transform.Translate(0, _speed * speedModifier * deltaTime, 0);
        }

        private void CheckScreenConstraints() {
            if (transform.position.y > Camera.main.orthographicSize + transform.localScale.y) {
                BallGoOffscreen(this);
            }
        }

        private void OnMouseDown() {
            BallClicked(this);
        }

    }

}