using UnityEngine;
using System.Collections;

public class Row_Scanner : MonoBehaviour {

    //public int[] blockCount;
    //public GameObject[] completeRow;
    //public GameObject[] blocks;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}


    /* Leftovers of an epic failed attempt. Taking a break because my brainhole is confused about the
     * best way to accomplish this.
     * 
     * What needs to happen: Called by Block_Control once the falling block has landed, scan each 
     * row for objects tagged "Block", if there are 10 objects in that row (at same Y position), destroy them
     * all. Then Find all objects tagged "Block" again, find out which of those were above (higher Y position)
     * and shift them all down the Y axis by 1.
    
    public void CheckRows()
    {
        blocks = GameObject.FindGameObjectsWithTag("Block");
        if (blocks.Length > 0)
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                if (blocks[i].transform.position.y == i)
                {
                    blockCount = new int[i];
                    blockCount[i] += 1;
                    blocks[i].tag = "Finish";
                    //GameObject[] completeRow = new GameObject[i];
                    //completeRow[i] = blocks[i];
                }
                if (blockCount.Length > 0)
                {
                    if (blockCount[i] >= 16)
                    {
                        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Finish");
                        foreach (GameObject cube in cubes)
                        {
                            Destroy(cube);
                        }
                    }
                }                
            }
        }
     
     */
}
