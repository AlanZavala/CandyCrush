using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private int ballsDestroyed = 0;

    private GameObject[,] matriz;

    public player fball;
    public player sball;
    private int k = 1;

    // Start is called before the first frame update
    void Start()
    {
        matriz = new GameObject[7,7];



        for(int i=0; i<7; i++)
        {
            for(int j=0; j<7; j++)
            {

                matriz[i,j]=GameObject.Find("Ball" + k);

                Debug.Log(matriz[i,j].gameObject.GetComponent<player>().color);
                k++;
                Debug.Log(k);

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
        if(fball == null)
        {
           fball = ball;
        }
        else
        {
            sball = ball;
            Change();
        }
    }

    public void Change()
    {

    }
}
