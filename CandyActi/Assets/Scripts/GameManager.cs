using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private int [,] matriz;
    private GameObject[,] auxMatrix;
    public Ball fball;
    public Ball sball;
    LinkedList<GameObject> rows = new LinkedList<GameObject>(); //This list will store the ball that will be destroyed. 

    private int k = 1;
    private Vector2 velocity;
    public float smoothTime;

    private int ballsDestroyed = 0;
    private bool canbeSwaped; //to verify if the ball can swap (they have to together)
    public Text brokenText;
             

    // Start is called before the first frame update
    void Start()
    {
        matriz = new int [7,7];
        auxMatrix = new GameObject[7, 7];
        brokenText.text = "Piezas eliminadas: " + ballsDestroyed;


        for (int i=0; i<7; i++)
        {
            for(int j=0; j<7; j++)
            {
                string look = "Ball" + k;
                auxMatrix[i, j] = GameObject.Find(look);
                matriz[i, j] = GameObject.Find(look).gameObject.GetComponent<Ball>().color;
                GameObject.Find(look).gameObject.GetComponent<Ball>().x = i;
                GameObject.Find(look).gameObject.GetComponent<Ball>().y = j;
                k++;              
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

    public void Swap() //THIS METHOD CHANGE THE VALUES OF THE MATRIX  -> 'CHANGE THE COLOR OF THE BALL'
    {
        //keep an aux to store the value of the first ball.
        int auxColor = fball.gameObject.GetComponent<Ball>().color;

        int auxX1 = fball.gameObject.GetComponent<Ball>().x;
        int auxY1 = fball.gameObject.GetComponent<Ball>().y;

        int auxX2 = sball.gameObject.GetComponent<Ball>().x;
        int auxY2 = sball.gameObject.GetComponent<Ball>().y;

        matriz[auxX1, auxY1] = matriz[auxX2, auxY2];
        matriz[auxX2, auxY2] = auxColor;

        fball.gameObject.GetComponent<Ball>().x = auxX2;
        fball.gameObject.GetComponent<Ball>().y = auxY2;

        sball.gameObject.GetComponent<Ball>().x = auxX1;
        sball.gameObject.GetComponent<Ball>().y = auxY1;


        /*GameObject aux = fball.gameObject;
        int xaux = fball.x;
        int yaux = fball.y;
        string nameaux = fball.name;

        matriz[xaux, yaux] = matriz[sball.x, sball.y];
        matriz[sball.x, sball.y] = aux;
              
        fball.x = sball.x;
        fball.y = sball.y;
        fball.name = sball.name;

        sball.x = xaux;
        sball.y = yaux;
        sball.name = nameaux;*/

    }

    public void Move()
    {
        float xaux = fball.transform.position.x;
        float yaux = fball.transform.position.y;
        float zaux = fball.transform.position.z;

        //aux.transform.position = fball.transform.position;

        fball.transform.position = sball.transform.position;
        sball.transform.position = new Vector3(xaux, yaux, zaux);

        bool check = Check(fball);
        if (check)
        {
            rows.AddFirst(FindTheBall(sball.gameObject.GetComponent<Ball>().x, sball.gameObject.GetComponent<Ball>().y));
            DestroyBalls();
            Debug.Log("la bola 1 sí tiene");
        }
        else
        {            
            Debug.Log("la bola 1 no tiene");
            check = Check(sball);
            if (check)
            {
                rows.AddFirst(FindTheBall(fball.gameObject.GetComponent<Ball>().x, fball.gameObject.GetComponent<Ball>().y));
                DestroyBalls();
                Debug.Log("la bola 2 sí tiene");
            }
            else
            {
                Debug.Log("la bola 2 no tiene");
            }
        }

        
        fball = null;
        sball = null;
    }

    public bool Check(Ball checkBall) //This method check if there are three blocks together
    {
        int x = checkBall.x;
        int y = checkBall.y;
        int pivote = 1;
        int together = 0;
        int color = checkBall.gameObject.GetComponent<Ball>().color;

        while (x+pivote<=6 && together<2)
        {
            if(matriz[x+pivote,y] != color)
            {                
                break;
            }
            else
            {
                rows.AddLast(FindTheBall(x + pivote, y));
                together++;
                pivote++;                
            }            
        }
        pivote = 1;
        while (x - pivote >= 0 && together < 2)
        {            
            if (matriz[x-pivote, y] != color)
            {
                break;
            }
            else
            {
                rows.AddLast(FindTheBall(x - pivote, y));
                together++;
                pivote--;                
            }
        }

        if (together != 2)
        {
            rows.Clear();
            together = 0;
            pivote = 1;
            while (y + pivote <= 6 && together < 2)
            {                
                if (matriz[x, y + pivote] != color)
                {
                    break;
                }
                else
                {
                    rows.AddLast(FindTheBall(x, y+pivote));
                    together++;
                    pivote++;
                }

            }
            pivote = 1;
            while (y - pivote >= 0 && together < 2)
            {
                if (matriz[x, y - pivote] != color)
                {
                    break;
                }
                else
                {
                    rows.AddLast(FindTheBall(x, y - pivote));
                    together++;
                    pivote--;
                }
            }
        }       

        if (together == 2)
        {
            return true;
        }
        else
        {
            rows.Clear();
            return false;
        }                   
    }

    public void DestroyBalls()
    {
        UpdateDestroyed();
        for(int d=0; d<3; d++)
        {
            Destroy(rows.Last.Value);
            rows.RemoveLast();
        }
    }

    public GameObject FindTheBall(int x, int y)
    {
        return auxMatrix[x, y];
    }

}
