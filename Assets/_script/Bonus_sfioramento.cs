using UnityEngine;

namespace _script
{
    public class Bonus_sfioramento : MonoBehaviour
    {
        private GameController gameController;
        private bool a;

        private void Start()
        {
            GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
            gameController = gameControllerObject.GetComponent<GameController>();
            a = false;
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                if (!a)
                {
                    gameController.Sfioramento();
                    a = true;
                }
            }
        }
    }
}
