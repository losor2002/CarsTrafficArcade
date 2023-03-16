using UnityEngine;

namespace _script
{
    public class nuova_partita : MonoBehaviour
    {
        public int NewGame;

        private void Awake()
        {
            var objs = GameObject.FindGameObjectsWithTag("NuovaPartita");

            if (objs.Length > 1)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}