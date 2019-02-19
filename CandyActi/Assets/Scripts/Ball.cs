using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public int color;
    public int x;
    public int y;

    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        ColorBall();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCoordinates(int newx, int newy)
    {
        x = newx;
        y = newy;
    }

    public void OnMouseDown()
    {
        //Debug.Log("hola");
        gameObject.GetComponent<Renderer>().material.color = Color.grey;
        gm.SelectedBall(this);
    }

    public void ColorBall()
    {
        if (color == 1)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if (color == 2)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
