using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public BoardManager boardScript;
    public GameObject[] countdown;

    // Start is called before the first frame update
    void Awake()
    {
        //boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    public void InitGame()
    {
        //boardScript = GetComponent<BoardManager>();
        StartCoroutine(Countdown(4));
        //boardScript.boardSetup();
    }

    public IEnumerator Countdown(int seconds)
    {
        int count = seconds;

        while (count > 0)
        {

            GameObject toInstantiate = countdown[count - 1];
            GameObject instance = Instantiate(toInstantiate, new Vector3(0, 0, -0.11f), Quaternion.identity) as GameObject;
            yield return new WaitForSeconds(1);
            Destroy(instance);
            count--;
        }

        // count down is finished...
        boardScript = GetComponent<BoardManager>();
        boardScript.boardSetup();
        GameObject.Find("player").GetComponent<PlayerController>().playerInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
