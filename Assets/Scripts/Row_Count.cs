using UnityEngine;
using System.Collections;

public class Row_Count : MonoBehaviour {

	[HideInInspector]
    public int yPos;
	[HideInInspector]
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
			Debug.Log ("Y pos: "+yPos);
            yPos = Mathf.RoundToInt(transform.position.y);
			Debug.Log ("Y pos adfer rounding: "+yPos);

			if (yPos <= 18) { //the highest level

				transform.parent = GameObject.Find (yPos.ToString ()).transform;
				transform.parent.GetComponent<Row_Scanner> ().NewTotal ();

			} else { // if go out the highest level, then game over

				Debug.Log ("Game Over");
				Time.timeScale = 0f;
			}
        }
	}
}
