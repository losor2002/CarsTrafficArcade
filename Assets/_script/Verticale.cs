using UnityEngine;
using UnityEngine.EventSystems;

public class Verticale : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool touched;
    private int pointerID;
    private bool a;
    private gamecontroller gamecontroller;

    void Start()
    {
        GameObject gc = GameObject.FindGameObjectWithTag("GameController");
        gamecontroller = gc.GetComponent<gamecontroller>();
        touched = false;
        if(this.CompareTag("su"))
        {
            a = true;
        }
        else
        {
            a = false;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (!touched)
        {
            touched = true;
            pointerID = data.pointerId;
            if(a)
            {
                gamecontroller.vert = 1;
            }
            else
            {
                gamecontroller.vert = -1;
            }
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (data.pointerId == pointerID)
        {
            gamecontroller.vert = 0;
            touched = false;
        }
    }
}
