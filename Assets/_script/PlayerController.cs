using UnityEngine;

namespace _script
{
    [System.Serializable]
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
        private Quaternion calibrationQuaternion;
        private touchpad touchpad;
        private SimpleTouchAreaButton toucharea;
        private int control;
        private float nextFire;
        public GameObject shot;
        public Transform shotSpawn;
        public float fireRate;
        private GameController gamecontroller;
        public ParticleSystem fiammata;


        private void Start()
        {
            calibraton();
            control = PlayerPrefs.GetInt("control", 2);
            GameObject touchpadObject = GameObject.FindGameObjectWithTag("Mzone");
            GameObject gc = GameObject.FindGameObjectWithTag("GameController");
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

            if (gamecontroller.gameover)
            {
                Destroy(gameObject);
            }
        }

        void FixedUpdate()
        {
            if (control == 0)
            {
                Vector3 accelerationR = Input.acceleration;
                Vector3 acceleration = Fixacc(accelerationR);
                Vector3 movement = new Vector3(acceleration.x, 0.0f, acceleration.y);
                GetComponent<Rigidbody>().velocity = movement * speed * 1.5f;
            }

            if (control == 1)
            {
                Vector2 direction = touchpad.GetDirection();
                Vector3 movement = new Vector3(direction.x, 0.0f, direction.y);
                GetComponent<Rigidbody>().velocity = movement * speed;
            }

            if (control == 2)
            {
                Vector3 movement = new Vector3(gamecontroller.hor, 0.0f, gamecontroller.vert);
                GetComponent<Rigidbody>().velocity = movement * speed;
            }

            GetComponent<Rigidbody>().position = new Vector3
            (
                Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
            );

            GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, GetComponent<Rigidbody>().velocity.x * tilt, 0.0f);
        }

        void calibraton()
        {
            Vector3 accsnapsh = Input.acceleration;
            Quaternion rotate = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), accsnapsh);
            calibrationQuaternion = Quaternion.Inverse(rotate);
        }

        Vector3 Fixacc (Vector3 acceleration)
        {
            Vector3 fixedacc = calibrationQuaternion * acceleration;
            return fixedacc;
        }
    }
}