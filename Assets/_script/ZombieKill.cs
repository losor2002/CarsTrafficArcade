using UnityEngine;
using UnityEngine.Serialization;

namespace _script
{
    public class ZombieKill : MonoBehaviour
    {
        [FormerlySerializedAs("zombiecade")] public GameObject fallingZombie;

        private bool _hit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("proiettile"))
            {
                if (_hit)
                {
                    Instantiate(fallingZombie, transform.position, transform.rotation);
                    PlayerPrefs.SetInt("kills", PlayerPrefs.GetInt("kills", 0) + 1);
                    Destroy(gameObject);
                }
                else
                {
                    _hit = true;
                }

                Destroy(other.gameObject);
            }
        }
    }
}