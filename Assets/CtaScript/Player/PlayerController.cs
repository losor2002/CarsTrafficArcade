using System;
using CtaScript.GameController;
using CtaScript.UserInput;
using UnityEngine;
using UnityEngine.Serialization;

namespace CtaScript.Player
{
    [Serializable]
    public class Boundary
    {
        public float xMin, xMax, zMin, zMax;
    }

    public class PlayerController : MonoBehaviour
    {
        public float speed;
        public float speed1;
        public float speed2;
        public float tilt;
        public float tilt1;
        public float tilt2;
        public Boundary boundary;
        public GameObject shot;
        public Transform shotSpawn;
        public float fireRate;
        public ParticleSystem muzzleFlash;

        private Quaternion _accelerometerCalibrationQuaternion;
        private int _controlSystem;
        private Vector3 _endPosition;
        private TouchAreaButton _fireButton;
        private GameControllerPlayScene _gameControllerPlayScene;
        private bool _isGunActive;
        private bool _isMoving;
        private float _nextFireTime;
        private Touchpad _touchpad;
        private bool _zombieMode;

        private void Start()
        {
            _zombieMode = PlayerPrefs.GetInt("zombie") != 0;
            _controlSystem = PlayerPrefs.GetInt("control", 2);
            if (_controlSystem == 0)
            {
                AccelerometerCalibration();
            }
            
            _touchpad = FindAnyObjectByType<Touchpad>();
            _fireButton = FindAnyObjectByType<TouchAreaButton>();
            _gameControllerPlayScene = FindAnyObjectByType<GameControllerPlayScene>();

            _isGunActive = shotSpawn != null;
        }

        private void Update()
        {
            if (_gameControllerPlayScene.pause)
            {
                return;
            }
            
            if (_isGunActive && _fireButton.IsPressed() && Time.time > _nextFireTime)
            {
                _nextFireTime = Time.time + fireRate;
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                muzzleFlash.Play();
            }

            switch (_gameControllerPlayScene.score)
            {
                case >= 30 and < 50:
                    speed = speed1;
                    tilt = tilt1;
                    break;
                case >= 50:
                    speed = speed2;
                    tilt = tilt2;
                    break;
            }

            if (!_zombieMode)
            {
                if (!_isMoving)
                {
                    var horizontalDiscreteMovement = _touchpad.GetHorizontalDiscreteMovement();
                    if (horizontalDiscreteMovement != 0)
                    {
                        _endPosition = transform.position +
                                       new Vector3(2.59f * horizontalDiscreteMovement, 0.0f, 0.0f);
                        if (_endPosition.x <= boundary.xMax && _endPosition.x >= boundary.xMin)
                        {
                            _isMoving = true;
                            transform.position = Vector3.MoveTowards(transform.position, _endPosition,
                                speed * Time.deltaTime);
                            transform.rotation =
                                Quaternion.Euler(0.0f, horizontalDiscreteMovement * speed * tilt, 0.0f);
                        }
                    }
                    else
                    {
                        var direction = _touchpad.GetDirection();
                        var movement = new Vector3(0.0f, 0.0f, direction.y);
                        transform.position += movement * (speed * Time.deltaTime);
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, _endPosition,
                        speed * Time.deltaTime);
                    if (transform.position == _endPosition)
                    {
                        _isMoving = false;
                        transform.rotation = Quaternion.identity;
                    }
                }
            }
            else
            {
                var movement = Vector3.zero;
                switch (_controlSystem)
                {
                    case 0:
                    {
                        var acceleration = FixedAcceleration(Input.acceleration);
                        movement = new Vector3(acceleration.x, 0.0f, acceleration.y) * 1.5f;
                        break;
                    }
                    case 1:
                    {
                        var direction = _touchpad.GetDirection();
                        movement = new Vector3(direction.x, 0.0f, direction.y);
                        break;
                    }
                    case 2:
                    {
                        movement = new Vector3(_gameControllerPlayScene.horizontalPlayerMovement, 0.0f,
                            _gameControllerPlayScene.verticalPlayerMovement);
                        break;
                    }
                }

                var velocity = movement * speed;
                transform.position += velocity * Time.deltaTime;
                transform.rotation = Quaternion.Euler(0.0f, velocity.x * tilt, 0.0f);
            }

            var position = transform.position;
            transform.position = new Vector3
            (
                Mathf.Clamp(position.x, boundary.xMin, boundary.xMax),
                position.y,
                Mathf.Clamp(position.z, boundary.zMin, boundary.zMax)
            );
        }

        private void AccelerometerCalibration()
        {
            var rotate = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), Input.acceleration);
            _accelerometerCalibrationQuaternion = Quaternion.Inverse(rotate);
        }

        private Vector3 FixedAcceleration(Vector3 acceleration)
        {
            return _accelerometerCalibrationQuaternion * acceleration;
        }
    }
}