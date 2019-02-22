using UnityEngine;
using UnityEngine.UI;

public class CounterText : MonoBehaviour
{
    public Text textCounter;
    public int balls = 0;

    // Start is called before the first frame update
    void Start()
    {
        textCounter.text = "Balas destruidas: " + balls;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateBallsDestroyed(int ballsDestroyed)
    {
        balls += ballsDestroyed;
        textCounter.text = "Balas destruidas: " + balls;
    }


}
