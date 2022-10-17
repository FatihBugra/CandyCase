using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Candy : MonoBehaviour
{
    [Header("Board Positions")]
    public int row;
    public int column;

    public bool isMatched;

    private int prevRow;
    private int prevCol;
    private int targetX;
    private int targetY;

    private Vector2 tempPos;
    private Vector2 firstTouchPos;
    private Vector2 finalTouchPos;

    private float swipeAngle;
    private float swipeResist;
    
    private WaitForSeconds delay = new WaitForSeconds(.1f);
    private Board boardScript;
    private FýndMatches fýndMatchesScript;
    private Camera cam;
    private GameObject otherCandy;

    void Start()
    {
        cam = Camera.main;
        boardScript = FindObjectOfType<Board>();
        fýndMatchesScript = FindObjectOfType<FýndMatches>();

    }
        
    void Update()
    {
        if(isMatched)
        {
            boardScript.DestroyMatches();
           
        }
        targetX = column;
        targetY = row;
        MoveTheCandys();      
      
    }
 
    void MoveTheCandys()
    {
        MoveHorizontally();
        MoveVertically(); 
        fýndMatchesScript.FýndAllMatches();
    }
    
 

    private void OnMouseDown() {

        firstTouchPos = cam.ScreenToWorldPoint(Input.mousePosition);    
    }
    private void OnMouseUp()
    {
        finalTouchPos = cam.ScreenToWorldPoint(Input.mousePosition);       
        CalculateAngel();
    }
    void CalculateAngel()
    {
        if(Mathf.Abs(finalTouchPos.y - firstTouchPos.y) > swipeResist 
        || Mathf.Abs(finalTouchPos.x - firstTouchPos.x) > swipeResist)
        { 
            float yDiff = (finalTouchPos.y - firstTouchPos.y);
            float xDiff = (finalTouchPos.x - firstTouchPos.x);
            swipeAngle = Mathf.Atan2(yDiff, xDiff) * 180 / Mathf.PI;
            SwipeThePice();
        }
    }

    void SwipeThePice() 
    {
        if(swipeAngle > -45 && swipeAngle <= 45  && column < boardScript.width -1)
        {
            // swipe right
            otherCandy = boardScript.allCandys[column + 1 , row];
            prevCol = column;
            prevRow = row;
            otherCandy.GetComponent<Candy>().column -= 1;
            column += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0) // 135 den büyük ve -135 kücük sayý yok o yüzden or kullandým
        {
            // swipe left
            otherCandy = boardScript.allCandys[column - 1, row];
            prevCol = column;
            prevRow = row;

            otherCandy.GetComponent<Candy>().column += 1;
            column -= 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < boardScript.height -1)
        {
            // swipe Up
            otherCandy = boardScript.allCandys[column, row + 1];
            prevCol = column;
            prevRow = row;

            otherCandy.GetComponent<Candy>().row -= 1;
            row += 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0 )
        {
            // swipe Down
            otherCandy = boardScript.allCandys[column , row -1];
            prevCol = column;
            prevRow = row;

            otherCandy.GetComponent<Candy>().row += 1;
            row -= 1;

        }
        StartCoroutine(ChechMatchedCorotine());

    }

    public IEnumerator ChechMatchedCorotine()
    {
        yield return delay;

        if (otherCandy != null)
        {
            if (!isMatched && !otherCandy.GetComponent<Candy>().isMatched)
            {
                Candy targetCandy = otherCandy.GetComponent<Candy>();
                targetCandy.row = row;
                targetCandy.column = column;
                row = prevRow;
                column = prevCol;
            }
            
            otherCandy = null;
        }   
        

    }
    void MoveHorizontally()
    {
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //move Right Or left
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPos, .1f);
            if (boardScript.allCandys[column, row] != this.gameObject)
            {
                boardScript.allCandys[column, row] = this.gameObject;

            }
        }
        else
        {
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = tempPos;
            
        }
    }
    void MoveVertically()
    {
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //move Right Or left
            tempPos = new Vector2(transform.position.x,targetY);
            transform.position = Vector2.Lerp(transform.position, tempPos, .1f);
            if (boardScript.allCandys[column, row] != this.gameObject)
            {
                boardScript.allCandys[column, row] = this.gameObject;

            }
        }
        else
        {
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = tempPos;
        }
    }
    

}
