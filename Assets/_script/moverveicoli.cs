using UnityEngine;

public class moverveicoli : MonoBehaviour
{
    public float speed;
    public float speed1;
    public float speed2;

    private gamecontroller gc;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        GameObject GC = GameObject.FindGameObjectWithTag("GameController");
        gc = GC.GetComponent<gamecontroller>();
    }

    private void Update()
    {
        if(gc.score >= 30 && gc.score < 50)
        {
            GetComponent<Rigidbody>().velocity = transform.forward * speed1;
        }
        if (gc.score >= 50)
        {
            GetComponent<Rigidbody>().velocity = transform.forward * speed2;
        }

        if(gc.gameover)
        {
            int zombie = PlayerPrefs.GetInt("zombie");
            if(zombie == 1)
            {
                if(this.CompareTag("pista") || this.CompareTag("zombieCade"))
                {
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
                else
                {
                    GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, -1f);
                }
            }
            else
            {
                if (this.CompareTag("pista"))
                {
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
                else
                {
                    GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 10f);
                }
            }
        }
    }
}
