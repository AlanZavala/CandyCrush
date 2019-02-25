using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour
{
    public List<Grid> match;
    public int matchstartingX;
    public int matchendX;
    public int matchstartingY;
    public int matchendY;   

    public bool ValidMatch
    {
        get
        {
            return match != null;
        }
    }
}
