using UnityEngine;
using System.Collections;

public class Block_Control : MonoBehaviour {

    public bool playerControlled;
    public bool safeMove;
    public bool isFalling;
    public bool isInitialized;
       
    public Vector3 tempMove;

    public Block_Spawner blockSpawner;

    public Transform[] childBlocks;

    public float blockTick;
    public float blockTime;
    public float moveTick;
    public float moveTime;
    
    // Use this for initialization
	void Start () {
        playerControlled = true;
        blockTime = 0.5f;
        moveTime = 0.1f;
        blockTick = blockTime;
        isFalling = true;
        blockSpawner = GameObject.Find("GameManager").GetComponent<Block_Spawner>();        
	}
	
	// Update is called once per frame
	void Update () {
        
        if (playerControlled)
        {
            InitializeObjects();
            BlockDrop();
            SideMovement();
            //RotateMovement();
        }
	}

    void InitializeObjects()
    {
        if (!isInitialized)
        {
            childBlocks = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                childBlocks[i] = transform.GetChild(i);
                childBlocks[i].tag = "Player";
            }
            isInitialized = true;
        }        
    }

    //Gravity effect and disables block control when collision occurs
    void BlockDrop()
    {
        if (isFalling)
        {
            Vector3 pos = transform.position;
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                tempMove = new Vector3(pos.x, pos.y - 0.5f, pos.z);
                CollisionCheck(Vector3.down);
                if (safeMove)
                {
                    transform.position = tempMove;
                }
                else
                {
                    isFalling = false;
                    playerControlled = false;
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        childBlocks[i].tag = "Obstacle";
                    }
                    CallSpawner();
                }
            }
            else if (blockTick > 0.0f)
            {
                blockTick -= Time.deltaTime;
            }
            else
            {
                CollisionCheck(-Vector3.up);
                if (safeMove)
                {
                    transform.position = new Vector3(pos.x, pos.y - 0.5f, pos.z);
                    blockTick = blockTime;
                }
                else
                {
                    isFalling = false;
                    playerControlled = false;
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        childBlocks[i].tag = "Obstacle";
                    }
                    CallSpawner();
                }    
            }
        }        
    }

    //Player control over falling block (Left/Right for side movement and Down for quick drop)
    void SideMovement()
    {
        Vector3 pos = transform.position;
        if (Input.anyKeyDown)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                tempMove = new Vector3(pos.x + 0.5f, pos.y, pos.z);
                CollisionCheck(Vector3.right);
                if (safeMove)
                {
                    transform.position = tempMove;
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                tempMove = new Vector3(pos.x - 0.5f, pos.y, pos.z);
                CollisionCheck(Vector3.left);
                if (safeMove)
                {
                    transform.position = tempMove;
                }
            }
            moveTick = moveTime;
        }
        else if (moveTick > 0.0f)
        {
            moveTick -= Time.deltaTime;
        }
        else
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                tempMove = new Vector3(pos.x + 0.5f, pos.y, pos.z);
                CollisionCheck(Vector3.right);
                if (safeMove)
                {
                    transform.position = tempMove;
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                tempMove = new Vector3(pos.x - 0.5f, pos.y, pos.z);
                CollisionCheck(Vector3.left);
                if (safeMove)
                {
                    transform.position = tempMove;
                }
            }
            moveTick = moveTime;
        }
        
    }

    /* Rotation disabled until I complete and figure out the raycast details
    void RotateMovement(){
        if (Input.anyKeyDown)
        {
            if (Input.GetAxisRaw("Jump") > 0)
            {
                transform.Rotate(0, 0, 90);
            }
        }        
    }
    */ 


    /* Raycast to detect clearance of side and falling movement
     * (needs bulked up for multiple raycasts from each exposed block
     * so that pieces can "fit" together like puzzle pieces)
    */
    bool CollisionCheck(Vector3 targetPosition)
    {
        foreach (Transform childBlock in childBlocks)
        {
            RaycastHit objectHit;
            Debug.DrawRay(childBlock.position, targetPosition * 0.5f, Color.green);
            if (Physics.Raycast(childBlock.position, targetPosition, out objectHit, 0.5f))
            {
                if (!objectHit.transform.CompareTag("Player"))
                {
                    safeMove = false;
                    Debug.Log("Cannot move there");
                    return false;
                }                
            }
            else
            {
                safeMove = true;
            }
        }
        return true;
    }

    /*void CollisionCheck(Vector3 targetPosition)
    {
        RaycastHit objectHit;
        Debug.DrawRay(transform.position, targetPosition * 0.5f, Color.green);
        if (Physics.Raycast(transform.position, targetPosition, out objectHit, 0.5f))
        {
            safeMove = false;
            Debug.Log("Cannot move there");
        }
        else
        {
            safeMove = true;
        }
    }*/






    void CallSpawner()
    {
        if (blockSpawner != null)
        {
            blockSpawner.CreateBlock();
        }        
    }
}
