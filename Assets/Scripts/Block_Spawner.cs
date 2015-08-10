using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Block_Spawner : MonoBehaviour
{

    public GameObject[] block;
    public Sprite[] icon;
    private GameObject nextBlock;
    private Image nextImage;
    private Score_Tracker score;
    private Row_Scanner row;

    // Use this for initialization
    void Start()
    {
        nextImage = GameObject.Find("Canvas/Image").GetComponent<Image>();
        score = GetComponent<Score_Tracker>();
        /* Still need to figure out how to best clear completed rows
        row = GetComponent<Row_Scanner>();
        */
        NextBlock();
        CreateBlock();
    }

    // Determines which type of block will drop next and displays the appropriate sprite
    public void NextBlock()
    {
        int x = Random.Range(0, 7);
        nextBlock = block[x];
        nextImage.sprite = icon[x];
    }

    // Creates the new block
    public void CreateBlock()
    {
        /* Waiting on completion of the CheckRows function in Row_Scanner
        row.CheckRows();
        */
        if (nextBlock != null)
        {
            Instantiate(nextBlock, new Vector3(7, 20, 0), Quaternion.identity);
            NextBlock();
        }
        else
        {
            NextBlock();
            Instantiate(nextBlock, new Vector3(7, 20, 0), Quaternion.identity);
        }
        score.NewBlock();
    }
}
