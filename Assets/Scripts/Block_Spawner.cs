using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Block_Spawner : MonoBehaviour {

    public GameObject[] block;
    public Sprite[] icon;
    public GameObject nextBlock;
    public Image nextImage;
    public Text scoreText;
    public int score;

	// Use this for initialization
	void Start () {
        nextImage = GameObject.Find("Canvas/Image").GetComponent<Image>();
        scoreText = GameObject.Find("Canvas/ActualScore").GetComponent<Text>();
        NextBlock();
        CreateBlock();
	}

    // Determines which type of block will drop next
    public void NextBlock()
    {
        int x = Random.Range(0, 7);
        nextBlock = block[x];
        nextImage.sprite = icon[x];
    }

    //Creates the new block
    public void CreateBlock()
    {
        if (nextBlock != null)
        {
            Instantiate(nextBlock, new Vector3(5, 9, 0), Quaternion.identity);
            NextBlock();
        }
        else
        {
            NextBlock();
            Instantiate(nextBlock, new Vector3(5, 9, 0), Quaternion.identity);
        }
        score += 1;
        scoreText.text = score.ToString();
    }

    
}
