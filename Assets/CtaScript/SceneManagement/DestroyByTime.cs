using UnityEngine;

namespace CtaScript.SceneManagement
{
    public class DestroyByTime : MonoBehaviour
    {
        public float lifetime;

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }
    }
}