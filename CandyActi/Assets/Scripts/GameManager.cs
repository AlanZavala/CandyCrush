using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private int ballsDestroyed = 0;

    private player[][] matriz;

    public player ball1;
    public player ball2;


    // Start is called before the first frame update
    void Start()
    {
        matriz = new player[7][];
        matriz[0] = new player[7];


        for(int i=0; i<7; i++)
        {
            for(int j=0; j<7; j++)
            {
                //matriz[i][j] = new player();
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
