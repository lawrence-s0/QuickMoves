using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public BoardManager boardScript;
    public SpriteRenderer mySpriteRenderer;

    public Sprite playerUPLEFT;
    public Sprite playerUPRIGHT;
    public Sprite playerLEFT;
    public Sprite playerRIGHT;
    public Sprite playerHurtLEFT;
    public Sprite playerHurtRIGHT;

    public int playerDIR = 1;
    // 1 = up + left
    // 2 = up + right
    // 3 = left
    // 4 = right

    public Color myRED;

    public int[,] coordinates;
    public int[,] board2;
    public int playerX;
    public int playerY;
    public bool resetEnabled;
    public bool onCooldown = false;
    public bool hitWall = false;

    public float moveCooldown;
    public float wallCooldown;

    private float timer;
    private float bestTime;
    private int tilesLeft = 0;
    public Text timerText;
    public Text bestTimeText;
    public Text endTimeText;
    public Text bestEndTimeText;
    public Text resetText;
    public Text tilesLeftText;

    // Start is called before the first frame update
    public void playerInitialize()
    {
        //playerX = GameObject.Find("GameManager").GetComponent<BoardManager>().myX;
        //playerY = GameObject.Find("GameManager").GetComponent<BoardManager>().myY;
        //board2 = GameObject.Find("GameManager").GetComponent<BoardManager>().board;

        mySpriteRenderer = GetComponent<SpriteRenderer>();
        playerX = GameObject.Find("GameManager").GetComponent<BoardManager>().myX;
        playerY = GameObject.Find("GameManager").GetComponent<BoardManager>().myY;
        board2 = GameObject.Find("GameManager").GetComponent<BoardManager>().board;
        resetEnabled = true;
        tilesLeft = GameObject.Find("GameManager").GetComponent<BoardManager>().pathLength;
    }

    // Update is called once per frame
    void Update()
    {
        if (board2[playerX, playerY] != 6)
        {
            timer += Time.deltaTime;
        }

        if (onCooldown) return;

        //**RESET**
        if (Input.GetKeyDown("space") && resetEnabled)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().InitGame();
            KillBoard(GameObject.Find("GameManager").GetComponent<BoardManager>().board);
            Destroy(GameObject.Find("NewBoard"));
            resetEnabled = false;
            timer = -4f;
            transform.position = new Vector3 (0, 0, -0.1f);
            tilesLeft = 0;
            ResetAllText();
        }

        //To store move directions.
        int horizontal = 0;
        int vertical = 0;
        //To get move directions
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));
        //We can't go in both directions at the same time
        if (horizontal != 0)
            vertical = 0;

        //If there's a direction, we are trying to move.
        if (horizontal != 0 || vertical != 0)
        {
            UpdateSpriteDir(horizontal, vertical);
            if (CollisionCheck(horizontal, vertical))
            {
                StartCoroutine(actionCooldown(moveCooldown));
                Move(horizontal, vertical);
                tilesLeft--;
            }
            else
            {
                hitWall = true;
                StartCoroutine(actionCooldown(wallCooldown));
            }
        }
    }

    void Move(int xDir, int yDir)
    {
        if (yDir == 1)
        {
            Vector3 newPosition = transform.position + 2 * Vector3.up;
            transform.position = newPosition;
            playerY++;
            
        }
        else if(yDir == -1)
        {
            Vector3 newPosition = transform.position + 2 * Vector3.down;
            transform.position = newPosition;
            playerY--;
        }
        else if (xDir == -1)
        {
            Vector3 newPosition = transform.position + 2 * Vector3.left;
            transform.position = newPosition;
            playerX--;
        }
        else if (xDir == 1)
        {
            Vector3 newPosition = transform.position + 2 * Vector3.right;
            transform.position = newPosition;
            playerX++;
        }

        //SetTimerText();
    }

    void UpdateSpriteDir(int xDir, int yDir)
    {
        if (yDir == 1)
        {
            if (playerDIR == 3)
            {
                playerDIR = 1;
                mySpriteRenderer.sprite = playerUPLEFT;
            }
            else if (playerDIR == 4)
            {
                playerDIR = 2;
                mySpriteRenderer.sprite = playerUPRIGHT;
            }

        }
        else if (xDir == -1)
        {
            playerDIR = 3;
            mySpriteRenderer.sprite = playerLEFT;
        }
        else if (xDir == 1)
        {
            playerDIR = 4;
            mySpriteRenderer.sprite = playerRIGHT;
        }
    }

    bool CollisionCheck(int xDir, int yDir)
    {
        if (board2[playerX, playerY] == 1 && xDir == -1)
        {
            return true;
        }
        else if (board2[playerX, playerY] == 2 && yDir == 1)
        {
            return true;
        }
        else if (board2[playerX, playerY] == 3 && xDir == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void KillBoard(int[,] board)
    {
        for(int x = 0; x < 2 * GameObject.Find("GameManager").GetComponent<BoardManager>().pathLength + 1; x++)
        {
            for(int y = 0; y < GameObject.Find("GameManager").GetComponent<BoardManager>().pathLength; y++)
            {
                board[x, y] = 0;
            }
        }
    }

    public void SetTimerText()
    {
        if (tilesLeft > 0)
        {
            tilesLeftText.text = "TILES LEFT: " + tilesLeft.ToString();
        }
        else
        {
            tilesLeftText.text = "";
        }
        timerText.text = "TIME \n\t" + timer.ToString();
        if (timer <= 0)
        {
            timerText.text = "TIME";
        }
        if (bestTime > 0)
        {
            bestTimeText.text = "BEST TIME \n\t" + bestTime.ToString();
        }
        if (board2[playerX, playerY] == 6)
        {
            if (bestTime == 0 || timer < bestTime)
            {
                bestTime = timer;
            }
            timerText.text = "";
            endTimeText.text = "Your time was: \n" + timer.ToString();
            bestEndTimeText.text = "Your best time was: \n" + bestTime.ToString() + "\n Want to do better?";
            resetText.text = "Press SPACE to try again";
        }
    }

    public void ResetAllText()
    {
        timerText.text = "";
        endTimeText.text = "";
        resetText.text = "";
        bestTimeText.text = "";
        bestEndTimeText.text = "";
        tilesLeftText.text = "";
    }

    private IEnumerator actionCooldown(float cooldown)
    {
        onCooldown = true;
        if (hitWall)
        {
            if (playerDIR == 1 || playerDIR == 3)
            {
                mySpriteRenderer.sprite = playerHurtLEFT;
            }
            else if (playerDIR == 2 || playerDIR == 4)
            {
                mySpriteRenderer.sprite = playerHurtRIGHT;
            }
            mySpriteRenderer.color = myRED;
        }
        while (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
            yield return null;
        }

        if (hitWall)
        {
            hitWall = false;
            if (playerDIR == 1)
            {
                mySpriteRenderer.sprite = playerUPLEFT;
            }
            else if (playerDIR == 2)
            {
                mySpriteRenderer.sprite = playerUPRIGHT;
            }
            else if (playerDIR == 3)
            {
                mySpriteRenderer.sprite = playerLEFT;
            }
            else if (playerDIR == 4)
            {
                mySpriteRenderer.sprite = playerRIGHT;
            }
            mySpriteRenderer.color = Color.white;
        }
        onCooldown = false;
    }
}