using UnityEngine;

namespace _script
{
    public class Bonus_sfioramento : MonoBehaviour
    {
        private bool a;
        private GameController gameController;

        private void Start()
        {
            var gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
            gameController = gameControllerObject.GetComponent<GameController>();
            a = false;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (!a)
                {
                    gameController.Touched();
                    a = true;
                }
            }
        }
    }
}