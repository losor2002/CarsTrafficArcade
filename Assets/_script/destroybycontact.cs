using UnityEngine;

namespace _script
{
    public class destroybycontact : MonoBehaviour
    {
        public GameObject explosion;
        public GameObject zombiecade;
        private GameController gameController;
        private int zombie;

        private void Start()
        {
            var gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
            gameController = gameControllerObject.GetComponent<GameController>();
            zombie = PlayerPrefs.GetInt("zombie");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                if (zombie == 0)
                {
                    var pos1 = other.transform.position + new Vector3(0f, 0.1f, 0f);
                    Instantiate(explosion, pos1, other.transform.rotation);
                    Destroy(other.gameObject);
                }
                else
                {
                    Instantiate(zombiecade, other.transform.position, other.transform.rotation);
                    Destroy(other.gameObject);
                }

                var pos = transform.position + new Vector3(0f, 0.1f, 0f);
                Instantiate(explosion, pos, transform.rotation);
                gameController.GameOver();
                Destroy(gameObject);
            }
        }
    }
}