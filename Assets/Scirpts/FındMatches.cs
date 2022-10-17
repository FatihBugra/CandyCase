using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FındMatches : MonoBehaviour
{
    private Board boardScript;
   
    public List<GameObject> currentMaches = new List<GameObject>();
    
    void Start()
    {
        boardScript = FindObjectOfType<Board>();          
        
    }
    
    public void FındAllMatches()
    {
        StartCoroutine(FındMatchesCoroutine());
    }
    private IEnumerator FındMatchesCoroutine()
    {
        yield return new WaitForSeconds(.1f);
        for (int i = 0; i < boardScript.width; i++)
        {
            for (int j = 0; j < boardScript.height; j++)
            {
                GameObject currentCandy = boardScript.allCandys[i, j];
                if(currentCandy != null)
                {
                    if(i > 0 && i < boardScript.width -1)
                    {
                        
                        GameObject leftCandy = boardScript.allCandys[i - 1, j];
                        GameObject rightCandy = boardScript.allCandys[i + 1, j];
                        if (leftCandy != null && rightCandy != null)
                        {
                            if(leftCandy.tag == currentCandy.tag && rightCandy.tag == currentCandy.tag)
                            {
                                leftCandy.GetComponent<Candy>().isMatched = true;
                                rightCandy.GetComponent<Candy>().isMatched = true;
                                currentCandy.GetComponent<Candy>().isMatched = true;
                            }
                        }
                    }
                    if (j > 0 && j < boardScript.height -1)
                    {

                        GameObject downCandy = boardScript.allCandys[i , j - 1];
                        GameObject uperCandy = boardScript.allCandys[i , j + 1];
                        if (downCandy != null && uperCandy != null)
                        {
                            if (downCandy.tag == currentCandy.tag && uperCandy.tag == currentCandy.tag)
                            {
                                downCandy.GetComponent<Candy>().isMatched = true;
                                uperCandy.GetComponent<Candy>().isMatched = true;
                                currentCandy.GetComponent<Candy>().isMatched = true;
                            }
                        }
                    }
                }
            }
        }
        
    }
    
}
