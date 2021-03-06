﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformEx
{
   public static IEnumerator Move(this Transform t, Vector3 target, float duration)
    {
        Vector3 diffVector = (target - t.position);
        float diffLength = diffVector.magnitude;
        diffVector.Normalize();
        float counter = 0;
        while (counter < duration)
        {
            float moveamount = (Time.deltaTime)*diffLength/duration;

            t.position += diffVector * moveamount;
            counter += Time.deltaTime;
            yield return null; 
        }
        t.position = target;
    }

    public static IEnumerator Scale(this Transform t, Vector3 target, float duration)
    {
        Vector3 diffVector = (target - t.localScale);
        float diffLength = diffVector.magnitude;
        diffVector.Normalize();
        float counter = 0;
        while (counter < duration)
        {
            float moveamount = (Time.deltaTime) * diffLength / duration;

            t.localScale += diffVector * moveamount;
            counter += Time.deltaTime;
            yield return null;
        }
        t.localScale = target;
    }
}
