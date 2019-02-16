using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private int ballsDestroyed = 0;

    private GameObject[,] matriz;

    public player ball1;
    public player ball2;
    private int k = 1;

    // Start is called before the first frame update
    void Start()
    {
        matriz = new GameObject[7,7];



        for(int i=0; i<7; i++)
        {
            for(int j=0; j<7; j++)
            {
<<<<<<< HEAD
                matriz[i,j]=GameObject.Find("Ball" + k);

                Debug.Log(matriz[i,j].gameObject.GetComponent<player>().color);
                k++;
                Debug.Log(k);
=======
                //matriz[i][j] = new player();
>>>>>>> e3a52b2dafce1683dcf555361f3f8a68a547fbb0
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdateDestroyed()
    {
        ballsDestroyed += 3;
    }

    public void SelectedBall(player ball)
    {
        if(ball1 == null)
        {
            ball1 = ball;
        }
        else
        {
            ball2 = ball;
            Search();
        }
    }

    public void Search()
    {

    }
}
