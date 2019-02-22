using System.Collections.Generic;
using UnityEngine;

public class FillGrid : MonoBehaviour
{
    GameObject[] candies;
    public float candies_width = 1f;
    private Grid[,] items;
    public int Xsize;
    public int Ysize;

    private Grid SelectedItems;
    public static int minitemformatch = 3;
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
        for(int i=0; i<candies.Length; i++)
        {
            candies[i].GetComponent<Grid>().id = i;
        }
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
            if(xDiff + yDiff == 1)
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

    IEnumerator TryMatch(Grid a, Grid b)
    {
        yield return StartCoroutine(Swap(a, b));
        Debug.Log("Swap candies");
    }

    List<Grid> SearchHorizontal(Grid item)
    {
        List<Grid> H_items = new List<Grid>{item};
        int left = item.x - 1;
        int right = item.x + 1;

        while(left>=0 && items[left, item.y]!=null && items[left, item.y].id == item.id)
        {
            H_items.Add(items[left, item.y]);
            left--;
        }
        while (right<Xsize && items[right, item.y] != null && items[right, item.y].id == item.id)
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

        while (lower >= 0 && items[item.x, lower] != null && items[item.x, lower].id == item.id)
        {
            V_items.Add(items[item.x, lower]);
            lower--;
        }
        while (upper < Ysize && items[item.x, upper ] != null && items[item.x, upper].id == item.id)
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
            h.matchstartingY = h.matchendY = hmatch[0].x;
            h.match = hmatch;
        }
        else if (vmatch.Count>=minitemformatch)
        {
            h.matchstartingY = GetMinimumY(vmatch);
            h.matchendY = GetMaximumY(vmatch);
            h.matchstartingX = h.matchendX = vmatch[0].y;
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
}
