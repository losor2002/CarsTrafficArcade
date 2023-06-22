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

        private void Start()
        {
            _gameControllerPlayScene = GameObject.FindGameObjectWithTag("GameController")
                .GetComponent<GameControllerPlayScene>();
            _zombieMode = PlayerPrefs.GetInt("zombie") != 0;
        }

        private void Update()
        {
            if (!_gameControllerPlayScene.gameOver)
            {
                var currentSpeed = _gameControllerPlayScene.score switch
                {
                    >= 30 and < 50 => speed1,
                    >= 50 => speed2,
                    _ => speed
                };
                transform.position += Vector3.forward * (currentSpeed * Time.deltaTime);
            }
            else
            {
                Vector3 movement;
                if (_zombieMode)
                {
                    movement = CompareTag("pista") || CompareTag("zombieCade") ? Vector3.zero : Vector3.back;
                }
                else
                {
                    movement = CompareTag("pista") ? Vector3.zero : Vector3.forward * 10;
                }

                transform.position += movement * Time.deltaTime;
            }
        }
    }
}