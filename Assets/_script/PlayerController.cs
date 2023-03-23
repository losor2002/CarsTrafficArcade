using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _script
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
        [FormerlySerializedAs("fiammata")] public ParticleSystem muzzleFlash;

        private Quaternion _accelerometerCalibrationQuaternion;
        private int _controlSystem;
        private Vector3 _endPosition;
        private TouchAreaButton _fireButton;
        private GameControllerPlayScene _gameControllerPlayScene;
        private bool _isGunActive;
        private bool _isMoving;
        private float _nextFireTime;
        private Rigidbody _rigidbody;
        private Touchpad _touchpad;
        private int _zombieMode;

        private void Start()
        {
            _zombieMode = PlayerPrefs.GetInt("zombie");
            _controlSystem = PlayerPrefs.GetInt("control", 2);
            if (_controlSystem == 0)
            {
                AccelerometerCalibration();
            }

            var touchpadObject = GameObject.FindGameObjectWithTag("Mzone");
            _touchpad = touchpadObject.GetComponent<Touchpad>();
            _fireButton = touchpadObject.GetComponent<TouchAreaButton>();
            _gameControllerPlayScene = GameObject.FindGameObjectWithTag("GameController")
                .GetComponent<GameControllerPlayScene>();

            _isGunActive = shotSpawn != null;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_isGunActive && !_gameControllerPlayScene.pause && _fireButton.IsPressed() && Time.time > _nextFireTime)
            {
                _nextFireTime = Time.time + fireRate;
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                muzzleFlash.Play();
            }
        }

        private void FixedUpdate()
        {
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

            if (_zombieMode == 0)
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
                            _rigidbody.velocity = new Vector3();
                            transform.position = Vector3.MoveTowards(transform.position, _endPosition,
                                speed * Time.fixedDeltaTime);
                            _rigidbody.rotation =
                                Quaternion.Euler(0.0f, horizontalDiscreteMovement * speed * tilt, 0.0f);
                        }
                    }
                    else
                    {
                        var direction = _touchpad.GetDirection();
                        var movement = new Vector3(0.0f, 0.0f, direction.y);
                        _rigidbody.velocity = movement * speed;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, _endPosition,
                        speed * Time.fixedDeltaTime);
                    if (transform.position == _endPosition)
                    {
                        _isMoving = false;
                        _rigidbody.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    }
                }
            }
            else
            {
                switch (_controlSystem)
                {
                    case 0:
                    {
                        var acceleration = FixedAcceleration(Input.acceleration);
                        var movement = new Vector3(acceleration.x, 0.0f, acceleration.y);
                        _rigidbody.velocity = movement * (speed * 1.5f);
                        break;
                    }
                    case 1:
                    {
                        var direction = _touchpad.GetDirection();
                        var movement = new Vector3(direction.x, 0.0f, direction.y);
                        _rigidbody.velocity = movement * speed;
                        break;
                    }
                    case 2:
                    {
                        var movement = new Vector3(_gameControllerPlayScene.horizontalPlayerMovement, 0.0f,
                            _gameControllerPlayScene.verticalPlayerMovement);
                        _rigidbody.velocity = movement * speed;
                        break;
                    }
                }

                _rigidbody.rotation = Quaternion.Euler(0.0f, _rigidbody.velocity.x * tilt, 0.0f);
            }

            _rigidbody.position = new Vector3
            (
                Mathf.Clamp(_rigidbody.position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(_rigidbody.position.z, boundary.zMin, boundary.zMax)
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