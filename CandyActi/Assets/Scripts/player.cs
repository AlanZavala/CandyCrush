using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public int color;

    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
       

        if(color == 1 )
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        Debug.Log("hola");
        gameObject.GetComponent<Renderer>().material.color = Color.grey;
//        gm.SelectedBall(this);
    }
}
