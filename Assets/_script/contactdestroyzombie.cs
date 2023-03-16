using UnityEngine;

namespace _script
{
    public class contactdestroyzombie : MonoBehaviour
    {
        public GameObject zombiecade;
        private bool colpito;

        private void Start()
        {
            colpito = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("proiettile"))
            {
                if (colpito)
                {
                    Instantiate(zombiecade, transform.position, transform.rotation);
                    var k = PlayerPrefs.GetInt("kills", 0);
                    PlayerPrefs.SetInt("kills", k + 1);
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                }

                if (!colpito)
                {
                    colpito = true;
                    Destroy(other.gameObject);
                }
            }
        }
    }
}