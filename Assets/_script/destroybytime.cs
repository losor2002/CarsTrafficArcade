using UnityEngine;

namespace _script
{
    public class destroybytime : MonoBehaviour
    {
        public float lifetime;

        void Start()
        {
            Destroy (gameObject, lifetime);
        }
    }
}
