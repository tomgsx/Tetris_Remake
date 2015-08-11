using UnityEngine;
using System.Collections;

public class Block_Control : MonoBehaviour {

    public Vector3 tempMove;

    public Block_Spawner blockSpawner;

    public Transform[] childBlocks;

    public bool playerControlled;
    public bool safeMove;
    public bool isFalling;
    public bool isInitialized;

    public float blockTick;
    public float moveTick;

    private float blockTime;
    private float moveTime;
    
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
            RotateMovement();
            BlockDrop();
            SideMovement();            
        }
	}

    // Find all child objects, disable colliders (to ignore self with raycasts)
    void InitializeObjects()
    {
        if (!isInitialized)
        {
            childBlocks = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                childBlocks[i] = transform.GetChild(i);
                childBlocks[i].tag = "Player";
                childBlocks[i].gameObject.GetComponent<Collider>().enabled = false;
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
            tempMove = new Vector3(pos.x, pos.y - 1, pos.z);
            // Continuous fast movement on vertical input key hold down
            if (Input.GetAxisRaw("Vertical") < 0)
            {                
                if (CollisionCheck(Vector3.down))
                {
                    transform.position = tempMove;
                }
                else
                {
                    DisableMovement();
                    CallSpawner();
                }
            }
            else if (blockTick > 0.0f)
            {
                blockTick -= Time.deltaTime;
            }
            else
            {
                if (CollisionCheck(Vector3.down))
                {
                    // Default gravity movement with no player input
                    transform.position = tempMove;
                    blockTick = blockTime;
                }
                else
                {                    
                    DisableMovement();
                    CallSpawner();
                }    
            }
        }        
    }

    // Player control over falling block (Left/Right for side movement and Down for quick drop)
    void SideMovement()
    {
        Vector3 pos = transform.position;
        // Instant movement on first horizontal input key press
        if (Input.anyKeyDown)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                tempMove = new Vector3(pos.x + 1, pos.y, pos.z);
                if (CollisionCheck(Vector3.right))
                {
                    transform.position = tempMove;
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                tempMove = new Vector3(pos.x - 1, pos.y, pos.z);
                if (CollisionCheck(Vector3.left))
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
            // Continuous movement on horizontal input key hold down
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                tempMove = new Vector3(pos.x + 1, pos.y, pos.z);
                if (CollisionCheck(Vector3.right))
                {
                    transform.position = tempMove;
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                tempMove = new Vector3(pos.x - 1, pos.y, pos.z);
                if (CollisionCheck(Vector3.left))
                {
                    transform.position = tempMove;
                }
            }
            moveTick = moveTime;
        }
        
    }

    // Rotate active block on Jump input key if no obstacles are in the the way
    void RotateMovement(){
        if (Input.anyKeyDown)
        {
            if (Input.GetAxisRaw("Jump") > 0)
            {
                if (gameObject.name != "O_block(Clone)")
                {
                    transform.Rotate(0, 0, 90);
                    foreach (Transform childBlock in childBlocks)
                    {
                        if (Physics.CheckSphere(childBlock.position, 0.49f))
                        {
                            transform.Rotate(0, 0, -90);
                            Debug.Log("Cannot rotate");
                            break;
                        }
                    }
                }
            }
        }
    }

    // Disable player control on a block (upon landing), reset tag and colliders
    void DisableMovement()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            childBlocks[i].tag = "Block";
            childBlocks[i].gameObject.GetComponent<Collider>().enabled = true;
        }
        isFalling = false;
        playerControlled = false;
    }

    // Check adjacent space to see if there is room for current cubes
    bool CollisionCheck(Vector3 targetPosition)
    {
        foreach (Transform childBlock in childBlocks)
        {
            RaycastHit objectHit;
            Debug.DrawRay(childBlock.position, targetPosition * 1, Color.green);
            if (Physics.Raycast(childBlock.position, targetPosition, out objectHit, 1.0f))
            {
                safeMove = false;
                Debug.Log("Cannot move there");
                return false;             
            }
            else
            {
                safeMove = true;
            }
        }
        return safeMove;
    }

    // Create new player controlled block
    void CallSpawner()
    {
        if (blockSpawner != null)
        {
            blockSpawner.CreateBlock();
        }        
    }
}
