using UnityEngine;
using System.Collections;

public class Row_Scanner : MonoBehaviour
{
    private Score_Tracker score;
    
    void Start()
    {
        score = GameObject.Find("GameManager").GetComponent<Score_Tracker>();
    }

    // If the row contains 16 cubes horizontally, destroy the row and shift all higher cubes down by 1.
    public void NewTotal()
    {
        if (transform.childCount == 16)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            foreach (GameObject row in GameObject.FindGameObjectsWithTag("Finish"))
            {
                foreach (Transform child in row.transform)
                {
                    if (child.position.y > transform.position.y)
                    {
                        child.position = new Vector3(child.position.x, child.position.y - 1, child.position.z);
                    }
                }
            }
            score.NewRow();
        }
    }
}