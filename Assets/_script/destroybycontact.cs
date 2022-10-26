using UnityEngine;

public class destroybycontact : MonoBehaviour
{
    private gamecontroller gameController;
    public GameObject explosion;
    private int zombie;
    public GameObject zombiecade;

    void Start ()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        gameController = gameControllerObject.GetComponent<gamecontroller>();
        zombie = PlayerPrefs.GetInt("zombie");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (zombie == 0)
            {
                Vector3 pos1 = other.transform.position + new Vector3(0f, 0.1f, 0f);
                Instantiate(explosion, pos1, other.transform.rotation);
                Destroy(other.gameObject);
            }
            else
            {
                Instantiate(zombiecade, other.transform.position, other.transform.rotation);
                Destroy(other.gameObject);
            }
            Vector3 pos = transform.position + new Vector3(0f, 0.1f, 0f);
            Instantiate(explosion, pos, transform.rotation);
            gameController.GameOver();
            Destroy(gameObject);
        }
    }
}
