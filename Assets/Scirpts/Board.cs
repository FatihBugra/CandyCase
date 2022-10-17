using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("Positions")]
    public int height;
    public int width;
    public int offSet;

    public GameObject backgroundTile;

    [Header("Candy")]
    public GameObject[] candys;
    public  GameObject[,] allCandys;

    public GameObject explosion;

    void Start()
    {
        
       
        allCandys = new GameObject[width,height];
     
        SetUpTheCandys();

    }

   
   void SetUpTheCandys()
   {

     for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                
                Vector2 position = new Vector2(i,j + offSet);               
                int chosenCandy = Random.Range(0, candys.Length);
               
                while (AwoidMatch(i, j, candys[chosenCandy]))
                {
                    chosenCandy = Random.Range(0, candys.Length);
                  
                }
                
                GameObject candy = Instantiate(candys[chosenCandy], position, Quaternion.identity, transform) as GameObject;
                Candy candyComponet = candy.GetComponent<Candy>();

                candyComponet.row = j;
                candyComponet.column = i;


                candy.name = "(" + i + "," + j + ")";
                allCandys[i,j] = candy;
            }
        }
   }
    private bool AwoidMatch(int column, int row, GameObject piece)
    {
        if(column > 1 && row > 1)
        {
            if (allCandys[column - 1, row].tag == piece.tag && allCandys[column - 2, row].tag == piece.tag)
            {
                return true;
            }
            if (allCandys[column, row - 1].tag == piece.tag && allCandys[column, row - 2].tag == piece.tag)
            {
                return true;
            }
            
        }
        else if (column <= 1 || row <= 1)
            {
                if (row > 1)
                {
                    if (allCandys[column, row - 1].tag == piece.tag && allCandys[column, row - 2].tag == piece.tag)
                    {
                        return true;
                    }
                }
                if (column > 1)
                {
                    if (allCandys[column - 1, row].tag == piece.tag && allCandys[column - 2, row].tag == piece.tag)
                    {
                        return true;
                    }
                }

            }

        return false;
    }
    private void DestroyMatchesAt(int coulmn , int row)
    {
        if(allCandys[coulmn,row].GetComponent<Candy>().isMatched)
        {
            Instantiate(explosion, allCandys[coulmn, row].transform.position, Quaternion.identity);
            Destroy(allCandys[coulmn, row]);
            allCandys[coulmn, row] = null;
            
        }
       
    }

    public void DestroyMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(allCandys[i,j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
       StartCoroutine(DecreaseRowCoroutine());
    }
    
    private IEnumerator DecreaseRowCoroutine()
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allCandys[i, j] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    allCandys[i, j].GetComponent<Candy>().row -= nullCount;
                    allCandys[i, j] = null;
                }
               
            }
            nullCount = 0;
            yield return new WaitForSeconds(.01f);
        }
        StartCoroutine(FillTheBoardCoroutine());
    }

    void RefillTheBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(allCandys[i,j] == null)
                {
                    Vector2 position = new Vector2(i, j + offSet);

                    int neWChoosenCandy = Random.Range(0, candys.Length);

                    GameObject newCandy = Instantiate(candys[neWChoosenCandy], position, Quaternion.identity, transform) as GameObject;                   
                    allCandys[i, j] = newCandy;

                    Candy candyComponet = newCandy.GetComponent<Candy>();
                    candyComponet.row = j;                    
                    candyComponet.column = i;      
                }
            }
        }
    }
    bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(allCandys[i,j] != null)
                {
                    if(allCandys[i,j].GetComponent<Candy>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private IEnumerator FillTheBoardCoroutine()
    {
       
        yield return new WaitForSeconds(.5f);
        RefillTheBoard();
       

        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(.1f);
            DestroyMatches();
        }
    }
}
