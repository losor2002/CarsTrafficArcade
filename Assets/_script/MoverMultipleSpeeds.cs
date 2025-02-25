using UnityEngine;

namespace _script
{
    public class MoverMultipleSpeeds : MonoBehaviour
    {
        public float speed;
        public float speed1;
        public float speed2;

        private GameControllerPlayScene _gameControllerPlayScene;
        private bool _zombieMode;
        private float _randomIncrement;

        private void Start()
        {
            _gameControllerPlayScene = FindAnyObjectByType<GameControllerPlayScene>();
            _zombieMode = PlayerPrefs.GetInt("zombie") != 0;
            _randomIncrement = Random.Range(-1.5f, 0.0f);
        }

        private void Update()
        {
            var currentSpeed = _gameControllerPlayScene.score switch
            {
                >= 30 and < 50 => speed1,
                >= 50 => speed2,
                _ => speed
            };
            
            if (!_gameControllerPlayScene.gameOver)
            {
                if (CompareTag("Enemy"))
                {
                    currentSpeed += _randomIncrement;
                }
                
                transform.position += Vector3.forward * (currentSpeed * Time.deltaTime);
            }
            else
            {
                Vector3 movement;
                if (_zombieMode)
                {
                    movement = CompareTag("pista") || CompareTag("zombieCade") ? Vector3.zero
                        : Vector3.back + (Vector3.forward * _randomIncrement / 10);
                }
                else
                {
                    movement = CompareTag("pista") ? Vector3.zero : Vector3.back * (currentSpeed + _randomIncrement);
                }

                transform.position += movement * Time.deltaTime;
            }
        }
    }
}