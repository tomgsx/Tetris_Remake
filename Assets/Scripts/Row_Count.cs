using UnityEngine;
using System.Collections;

public class Row_Count : MonoBehaviour {

    public int yPos;
    public Block_Control blockControl;

	// Use this for initialization
	void Start () {
        blockControl = GetComponentInParent<Block_Control>();
	}
	
	// Update is called once per frame
	void Update () {
        // Once the block has landed, change individual cubes' parent to the row they are on (Y position)
        if (!blockControl.playerControlled)
        {
            yPos = Mathf.RoundToInt(transform.position.y);
            transform.parent = GameObject.Find(yPos.ToString()).transform;
            transform.parent.GetComponent<Row_Scanner>().NewTotal();
        }
	}
}
