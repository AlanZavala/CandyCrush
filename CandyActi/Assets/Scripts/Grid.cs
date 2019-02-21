﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int x
    {
        get;
        private set; 
    }

    public int y
    {
        get;
        private set; 
    }

    [HideInInspector]
    public int id; 
    
    public void OnItemPositionChanged(int newX, int newY)
    {
        x = newX;
        y = newY;
        gameObject.name = string.Format("ball_[{0}][{1}]", x, y);
    }

    void OnMouseDown()
    {
        if(OnMouseOverItemEventHandler != null)
        {
            OnMouseOverItemEventHandler(this);
        }
    }

    public delegate void OnMouseOverItem(Grid item);
    public static event OnMouseOverItem OnMouseOverItemEventHandler;
}
