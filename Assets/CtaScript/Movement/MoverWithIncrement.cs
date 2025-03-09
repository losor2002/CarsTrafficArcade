using CtaScript.GameController;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CtaScript.Movement
{
    public class MoverWithIncrement : MonoBehaviour
    {
        private const float TrackZombieModeSpeed = -11f;
        private const float Increment = -1f;
        
        public float speed;

        private GameControllerPlayScene _gameControllerPlayScene;
        private float _trackDeltaSpeed;

        private void Awake()
        {
            if (CompareTag("Enemy"))
            {
                speed += Random.Range(-1f, 0f);
            }
            else if (PlayerPrefs.GetInt("zombie") != 0 && CompareTag("pista"))
            {
                speed = TrackZombieModeSpeed;
            }
        }

        private void Start()
        {
            _gameControllerPlayScene = FindAnyObjectByType<GameControllerPlayScene>();
            
            float trackSpeed = GameObject.FindWithTag("pista").GetComponent<MoverWithIncrement>().speed;
            _trackDeltaSpeed = speed - trackSpeed;
        }

        private void Update()
        {
            if (!_gameControllerPlayScene.gameOver)
            {
                float currentSpeed = _gameControllerPlayScene.score switch
                {
                    >= 30 and < 50 => speed + Increment,
                    >= 50 => speed + Increment * 2,
                    _ => speed
                };
                
                transform.position += Vector3.forward * (currentSpeed * Time.deltaTime);
            }
            else
            {
                transform.position += Vector3.forward * (_trackDeltaSpeed * Time.deltaTime);
            }
        }
    }
}