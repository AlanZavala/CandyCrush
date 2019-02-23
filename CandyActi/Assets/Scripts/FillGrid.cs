using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class FillGrid : MonoBehaviour
{
    GameObject[] candies;
    public float candies_width = 1f;
    private Grid[,] items;
    public int Xsize;
    public int Ysize;
    private bool targetPower = false;
    private List<Grid> targetList = new List<Grid>();  
    private Grid SelectedItems;
    public static int minitemformatch = 3;
    public float delaybetween = 0.2f;
    public CounterText ct;
    // Start is called before the first frame update
    void Start()
    {
        GetCandies();
        FillGrid1();
        Grid.OnMouseOverItemEventHandler += OnMouseOverItem; //select candies by using this event
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetCandies()
    {
        candies = Resources.LoadAll<GameObject>("Prefabs");
        //for(int i=0; i<candies.Length; i++)
        //{
        //    candies[i].GetComponent<Grid>().id = i;
        //}
    }

    void FillGrid1()
    {
        items = new Grid[Xsize, Ysize];
        for(int i=0; i<Xsize; i++)
        {
            for(int j=0; j<Ysize; j++)
            {
                items[i, j] = instantiate_candies(i, j);
            }
        }
    }

    Grid instantiate_candies(int x, int y)
    {        
        GameObject random_candies = candies[Random.Range(0, candies.Length)];
        Grid newcandy = ((GameObject) Instantiate(random_candies, new Vector2(x * candies_width, y), Quaternion.identity)).GetComponent<Grid> ();
        newcandy.OnItemPositionChanged(x, y);
        return newcandy;
    }

    void OnMouseOverItem(Grid item)
    {
        if (!targetPower)
        {
            if (!item.CompareTag("Stone"))
            {


                if (SelectedItems == null)
                {
                    Debug.Log("Start point");
                    SelectedItems = item;
                }
                else
                {
                    Debug.Log("end point");
                    float xDiff = Mathf.Abs(item.x - SelectedItems.x);
                    float yDiff = Mathf.Abs(item.y - SelectedItems.y);
                    if (xDiff + yDiff == 1)
                    {
                        Debug.Log("try match function");
                        StartCoroutine(TryMatch(SelectedItems, item));
                    }
                    else
                    {
                        Debug.Log("error");
                    }
                    SelectedItems = null;
                }
            }
        }
        else
        {
            targetList.Add(item);
            Debug.Log(targetList.Count);
        }

    }

    IEnumerator TryMatch(Grid a, Grid b)
    {
        yield return StartCoroutine(Swap(a, b));
        Debug.Log("Swap candies");
        Match matchA = GetMatch(a);
        Match matchB = GetMatch(b);

        if(!matchA.ValidMatch && !matchB.ValidMatch)
        {
            yield return StartCoroutine(Swap(a, b));
            yield break;
        }
        if (matchA.ValidMatch && !a.CompareTag("Stone"))
        {
            Debug.Log("Match");
            yield return StartCoroutine(DestroyBall(matchA.match));
            yield return new WaitForSeconds(delaybetween);
            yield return StartCoroutine(UpdateGrid(matchA));
        }else if (matchB.ValidMatch && !a.CompareTag("Stone"))
        {
            Debug.Log("Not Match");
            yield return StartCoroutine(DestroyBall(matchB.match));
            yield return new WaitForSeconds(delaybetween);
            yield return StartCoroutine(UpdateGrid(matchB));

        }
    }

    IEnumerator DestroyBall(List<Grid> items)
    {
        int ballsDestroyed = 0;
        int countMash = 0;
        foreach (Grid i in items)
        {
            if (i != null) {
                if (i.gameObject.CompareTag("Mushroom"))
                {
                    countMash++;

                }
            }
        }

            foreach (Grid i in items)
        {
            if (i != null)
            {
                if (i.gameObject.CompareTag("Stone")) { 
                    Debug.Log("ruben");

                }
                else{
                    yield return StartCoroutine(i.transform.Scale(Vector3.zero, 0.045f));
                    if (countMash>0) {

                        //Destroy(i.gameObject);
                        //ballsDestroyed++;
                        ChangeColors();
                        FillGrid1();
                        //Revaluate();

                    }
                    else if (i.gameObject.CompareTag("Target"))
                    {
                        targetPower = true;
                        Destroy(i.gameObject);
                        ballsDestroyed++;
                        StartCoroutine("DelayTarget");
                        Debug.Log("Target Power");

                    }
                    else
                    {
                        Destroy(i.gameObject);
                        ballsDestroyed++;
                    }
                }

            }
        }
        ct.UpdateBallsDestroyed(ballsDestroyed);
    }
    IEnumerator DelayTarget()
    {
        yield return new WaitForSeconds(10);
        targetPower = false;
        Debug.Log("La lista contiene : " + targetList.Count);
        DestroyBall(targetList);
        targetList.Clear();
        Debug.Log("Finish");
    }


    /*void DestroySelected(List<Grid>targetBalls)
    {
        DestroyBall(targetBalls);
    }*/

    List<Grid> SearchHorizontal(Grid item)
    {
        List<Grid> H_items = new List<Grid>{item};
        int left = item.x - 1;
        int right = item.x + 1;

        while(left>=0 && items[left, item.y]!=null && items[left, item.y].id == item.id && items[left, item.y].tag!="Stone")
        {
            H_items.Add(items[left, item.y]);
            left--;
        }
        while (right<Xsize && items[right, item.y] != null && items[right, item.y].id == item.id && items[right, item.y].tag!="Stone")
        {
            H_items.Add(items[right, item.y]);
            right++;
        }
        return H_items;

    }

    List<Grid> SearchVertical(Grid item)
    {
        List<Grid> V_items = new List<Grid> {item};
        int lower = item.y - 1;
        int upper = item.y + 1;

        while (lower >= 0 && items[item.x, lower] != null && items[item.x, lower].id == item.id && items[item.x, lower].tag!="Stone")
        {
            V_items.Add(items[item.x, lower]);
            lower--;
        }
        while (upper < Ysize && items[item.x, upper ] != null && items[item.x, upper].id == item.id  && items[item.x, upper].tag!="Stone")
        {
            V_items.Add(items[item.x, upper]);
            upper++;
        }
        return V_items;

    }

    int GetMinimumX(List<Grid> items)
    {
        float[] indices = new float[items.Count];
        for(int i=0; i<indices.Length; i++)
        {
            indices[i] = items[i].x;
        }
        return (int)Mathf.Min(indices);
    }


    int GetMaximumX(List<Grid> items)
    {
        float[] indices = new float[items.Count];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = items[i].x;
        }
        return (int)Mathf.Max(indices);
    }

    int GetMinimumY(List<Grid> items)
    {
        float[] indices = new float[items.Count];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = items[i].y;
        }
        return (int)Mathf.Min(indices);
    }

    int GetMaximumY(List<Grid> items)
    {
        float[] indices = new float[items.Count];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = items[i].y;
        }
        return (int)Mathf.Max(indices);
    }

    Match GetMatch(Grid item)
    {
        Match h = new Match();
        h.match = null;
        List<Grid> hmatch = SearchHorizontal(item);
        List<Grid> vmatch = SearchVertical(item);
        if (hmatch.Count >= minitemformatch && hmatch.Count > vmatch.Count) 
        {
            h.matchstartingX = GetMinimumX(hmatch);
            h.matchendX = GetMaximumX(hmatch);
            h.matchstartingY = h.matchendY = hmatch[0].y;
            h.match = hmatch;
        }
        else if (vmatch.Count>=minitemformatch)
        {
            h.matchstartingY = GetMinimumY(vmatch);
            h.matchendY = GetMaximumY(vmatch);
            h.matchstartingX = h.matchendX = vmatch[0].x;
            h.match = vmatch;
        }

        return h;
    }

    IEnumerator Swap (Grid a, Grid b)
    {
        Changerigidbodystatus(false);
        float moveduration = 0.1f;
        Vector3 apos = a.transform.position;
        Vector3 bpos = b.transform.position;
        StartCoroutine(a.transform.Move(bpos, moveduration));
        StartCoroutine(b.transform.Move(apos, moveduration));
        yield return new WaitForSeconds(moveduration);
        SwapIndices(a, b);
        Changerigidbodystatus(true);    
    }

    void SwapIndices(Grid a, Grid b)
    {
        Grid aux = items[a.x, a.y];
        items[a.x, a.y] = b;
        items[b.x, b.y] = aux;
        int boldx = b.x;
        int boldy = b.y;

        b.OnItemPositionChanged(a.x, a.y);
        a.OnItemPositionChanged(boldx, boldy);
    }

    void Changerigidbodystatus(bool status)
    {
        foreach (Grid g in items)
        {
            if(g != null)
            {
                g.GetComponent<Rigidbody2D>().isKinematic = !status;
            }
        }
    }


    //Instantiate new candies 
    IEnumerator UpdateGrid(Match match)
    {
        if(match.matchstartingY == match.matchendY)
        {
            //horizontal match
            for(int x=match.matchstartingX; x<=match.matchendX; x++)
            {
                for(int y=match.matchstartingY; y<Ysize-1; y++)
                {
                    Grid upperindes = items[x, y + 1];
                    Grid current = items[x, y];
                    items[x, y] = upperindes;
                    items[x, y + 1] = current;
                    items[x, y].OnItemPositionChanged(items[x, y].x, items[x, y].y - 1);
                }
                items[x, Ysize - 1] = instantiate_candies(x, Ysize-1);
            }
        }
        else if(match.matchendX == match.matchstartingX)
        {
            //vertical match
            int matchHeight = 1 + (match.matchendY- match.matchstartingY);
            for (int y= match.matchstartingY+ matchHeight; y<=Ysize-1; y++)
            {
                Grid Lowerindex = items[match.matchstartingX, y - matchHeight];
                Grid current = items[match.matchstartingX, y];
                items[match.matchstartingX, y-matchHeight] = current;
                items[match.matchstartingX, y] = Lowerindex;
            }
            for(int y=0; y<Ysize-matchHeight; y++)
            {
                items[match.matchstartingX, y].OnItemPositionChanged(match.matchstartingX, y);
            }
            for(int i=0; i<match.match.Count; i++)
            {
                items[match.matchstartingX, (Ysize - 1) - i] = instantiate_candies(match.matchstartingX, (Ysize-1)-i);

            }
        }

        for(int x=0; x<Xsize; x++)
        {
            for(int y=0; y<Ysize; y++)
            {
                Match matchinfo = GetMatch(items[x, y]);
                if (matchinfo.ValidMatch)
                {
                    yield return new WaitForSeconds(delaybetween);
                    yield return StartCoroutine(DestroyBall(matchinfo.match));
                    yield return new WaitForSeconds(delaybetween);
                    yield return StartCoroutine(UpdateGrid(matchinfo));
                    yield return new WaitForSeconds(delaybetween);
                }
            }
        }
    }
    void ChangeColors() {
        int counter = 0;
        foreach(Grid i in items)
        {
            Destroy(i.gameObject);
            //counter++;
            //if (counter==49) {
            //    break;
            //}
            //Debug.Log("counter: "+counter);
        }
    }
    IEnumerator Revaluate() {
        for (int x = 0; x < Xsize; x++)
        {
            for (int y = 0; y < Ysize; y++)
            {
                Match matchinfo = GetMatch(items[x, y]);
                if (matchinfo.ValidMatch)
                {
                    yield return new WaitForSeconds(delaybetween);
                    yield return StartCoroutine(DestroyBall(matchinfo.match));
                    yield return new WaitForSeconds(delaybetween);
                    yield return StartCoroutine(UpdateGrid(matchinfo));
                    yield return new WaitForSeconds(delaybetween);
                }
            }
        }
    }
}
