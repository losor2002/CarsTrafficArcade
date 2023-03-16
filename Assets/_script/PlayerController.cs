using System;
using UnityEngine;

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
        public float tilt2;
        public float tilt1;
        public float tilt;
        public Boundary boundary;
        public GameObject shot;
        public Transform shotSpawn;
        public float fireRate;
        public ParticleSystem fiammata;
        private Quaternion calibrationQuaternion;
        private int control;
        private GameController gamecontroller;
        private float nextFire;
        private SimpleTouchAreaButton toucharea;
        private touchpad touchpad;


        private void Start()
        {
            calibraton();
            control = PlayerPrefs.GetInt("control", 2);
            var touchpadObject = GameObject.FindGameObjectWithTag("Mzone");
            var gc = GameObject.FindGameObjectWithTag("GameController");
            touchpad = touchpadObject.GetComponent<touchpad>();
            toucharea = touchpadObject.GetComponent<SimpleTouchAreaButton>();
            gamecontroller = gc.GetComponent<GameController>();
        }

        private void Update()
        {
            if (gamecontroller.score >= 30 && gamecontroller.score < 50)
            {
                speed = speed1;
                tilt = tilt1;
            }

            if (gamecontroller.score >= 50)
            {
                speed = speed2;
                tilt = tilt2;
            }

            if (shotSpawn != null)
            {
                if (!gamecontroller.pause)
                {
                    if (toucharea.CanFire() && Time.time > nextFire)
                    {
                        nextFire = Time.time + fireRate;
                        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                        fiammata.Play();
                    }
                }
            }

            if (gamecontroller.gameOver)
            {
                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            if (control == 0)
            {
                var accelerationR = Input.acceleration;
                var acceleration = Fixacc(accelerationR);
                var movement = new Vector3(acceleration.x, 0.0f, acceleration.y);
                GetComponent<Rigidbody>().velocity = movement * speed * 1.5f;
            }

            if (control == 1)
            {
                var direction = touchpad.GetDirection();
                var movement = new Vector3(direction.x, 0.0f, direction.y);
                GetComponent<Rigidbody>().velocity = movement * speed;
            }

            if (control == 2)
            {
                var movement = new Vector3(gamecontroller.horizontalPlayerMovement, 0.0f,
                    gamecontroller.verticalPlayerMovement);
                GetComponent<Rigidbody>().velocity = movement * speed;
            }

            GetComponent<Rigidbody>().position = new Vector3
            (
                Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
            );

            GetComponent<Rigidbody>().rotation =
                Quaternion.Euler(0.0f, GetComponent<Rigidbody>().velocity.x * tilt, 0.0f);
        }

        private void calibraton()
        {
            var accsnapsh = Input.acceleration;
            var rotate = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), accsnapsh);
            calibrationQuaternion = Quaternion.Inverse(rotate);
        }

        private Vector3 Fixacc(Vector3 acceleration)
        {
            var fixedacc = calibrationQuaternion * acceleration;
            return fixedacc;
        }
    }
}