using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private GameObject[,] matriz;
    public Ball fball;
    public Ball sball;
    public Ball aux; 

    private int k = 1;
    private Vector2 velocity;
    public float smoothTime;

    private int ballsDestroyed = 0;
    private bool canbeSwaped; //to verify if the ball can swap (they have to together)
    public Text brokenText;
             

    // Start is called before the first frame update
    void Start()
    {
        matriz = new GameObject[7,7];
        brokenText.text = "Piezas eliminadas: " + ballsDestroyed;


        for (int i=0; i<7; i++)
        {
            for(int j=0; j<7; j++)
            {
                string look = "Ball" + k;
                matriz[i,j]=GameObject.Find(look);
                GameObject.Find(look).gameObject.GetComponent<Ball>().x = i;
                GameObject.Find(look).gameObject.GetComponent<Ball>().y = j;
                //Debug.Log(matriz[i,j].gameObject.GetComponent<Ball>().color);
                k++;
               // Debug.Log(k);

            }
        }
    }

    public void UpdateDestroyed()
    {
        ballsDestroyed += 3;
        brokenText.text = "Piezas eliminadas: " + ballsDestroyed;
    }

    public void SelectedBall(Ball ball)
    {
        if(fball == null)
        {
           fball = ball;
        }
        else
        {
            sball = ball;
            Verify();
            if (canbeSwaped)
            {
                Swap();
                Move();
                canbeSwaped = false;
            }
            else
            {
                fball = null;
                sball = null;
            }
        }
    }

    public void Verify()
    {
        //First check from right to left 
        if (  ((fball.x +1) == sball.x )  || ((fball.x - 1) == sball.x)    )
        {            
            //The check from up and down
            if(fball.y == sball.y)
            {
                Debug.Log("Can be swaped");
                canbeSwaped = true;
            }                            
        }

        //First check from up to dowm 
        if (   ((fball.y + 1) == sball.y) || ((fball.y - 1) == sball.y))
        {
            //The check from right and left
            if (fball.x == sball.x)
            {
                Debug.Log("Can be swaped");
                canbeSwaped = true;
            }
        }
        fball.ColorBall();
        sball.ColorBall();
    }

    public void Swap()
    {
        //THIS METHOD CHANGE THE BALL IN THE MATRIX
        int xaux = fball.x;
        int yaux = fball.y;
        string nameaux = fball.name;

        fball.x = sball.x;
        fball.y = sball.y;
        fball.name = sball.name;

        sball.x = xaux;
        sball.y = yaux;
        sball.name = nameaux;
    }

    public void Move()
    {
        float xaux = fball.transform.position.x;
        float yaux = fball.transform.position.y;
        float zaux = fball.transform.position.z;

        //aux.transform.position = fball.transform.position;

        fball.transform.position = sball.transform.position;
        sball.transform.position = new Vector3(xaux, yaux, zaux);


        fball = null;
        sball = null;
    }

    public void Check()
    {

    }

}
