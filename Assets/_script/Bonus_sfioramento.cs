using UnityEngine;

public class Bonus_sfioramento : MonoBehaviour
{
    private gamecontroller gameController;
    private bool a;

    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        gameController = gameControllerObject.GetComponent<gamecontroller>();
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
