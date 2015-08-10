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

    void InitializeObjects()
    {
        if (!isInitialized)
        {
            childBlocks = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                childBlocks[i] = transform.GetChild(i);
                //childBlocks[i].tag = "Player";
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
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                tempMove = new Vector3(pos.x, pos.y - 1, pos.z);
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
                        //childBlocks[i].tag = "Block";
                        childBlocks[i].gameObject.GetComponent<Collider>().enabled = true;
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
                    transform.position = new Vector3(pos.x, pos.y - 1, pos.z);
                    blockTick = blockTime;
                }
                else
                {
                    isFalling = false;
                    playerControlled = false;
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        //childBlocks[i].tag = "Block";
                        childBlocks[i].gameObject.GetComponent<Collider>().enabled = true;
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
                tempMove = new Vector3(pos.x + 1, pos.y, pos.z);
                CollisionCheck(Vector3.right);
                if (safeMove)
                {
                    transform.position = tempMove;
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                tempMove = new Vector3(pos.x - 1, pos.y, pos.z);
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
                tempMove = new Vector3(pos.x + 1, pos.y, pos.z);
                CollisionCheck(Vector3.right);
                if (safeMove)
                {
                    transform.position = tempMove;
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                tempMove = new Vector3(pos.x - 1, pos.y, pos.z);
                CollisionCheck(Vector3.left);
                if (safeMove)
                {
                    transform.position = tempMove;
                }
            }
            moveTick = moveTime;
        }
        
    }

    // Jump input to rotate active block if no obstacles in the way
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

    bool CollisionCheck(Vector3 targetPosition)
    {
        foreach (Transform childBlock in childBlocks)
        {
            RaycastHit objectHit;
            Debug.DrawRay(childBlock.position, targetPosition * 1, Color.green);
            if (Physics.Raycast(childBlock.position, targetPosition, out objectHit,1))
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
        return true;
    }

    void CallSpawner()
    {
        if (blockSpawner != null)
        {
            blockSpawner.CreateBlock();
        }        
    }
}
