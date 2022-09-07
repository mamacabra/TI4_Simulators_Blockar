using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigureJoint : MonoBehaviour
{
    [Space(30)]
    [Header("Only rayDistance, breakForce and breakTorque must be sets by hand.")]
    [Tooltip("The MainBlock, Gets on start by the GameManager.cs")]
    [SerializeField] private GameObject mainBlock;

    [Header("Lists")]
    [Space(10)]
    [Tooltip("List of all directions that not have a block")]
    [SerializeField] private List<int> nonCollidingDirs;
    [Tooltip("List of all blocks jointed to this block")]
    [SerializeField] private List<GameObject> linkedBlocks;
    [Tooltip("Ray Distance to capture other block around this block")]

    [Header("Floats")]
    [Space(10)]
    [SerializeField] private float rayDistance = 0.75f;
    [Tooltip("Force to break the block ps.. In the main block it doesn't matter")]
    [SerializeField] private float breakForce = float.PositiveInfinity;
    [Tooltip("Torque to break the block ps.. In the main block it doesn't matter")]
    [SerializeField] private float breakTorque = float.PositiveInfinity;

    [Header("Materials")]
    [Space(10)]
    [Tooltip("The Destroy Material, Gets on start by the GameManager.cs")]
    [SerializeField] private Material destroyMaterial;
    [Tooltip("The Default Material, Gets on start by the block")]
    [SerializeField] private Material defaultMaterial;

    private Rigidbody myRigidBody;
    private readonly RaycastHit[] _hit = new RaycastHit[6];
    private readonly Vector3[] _directions = {
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right,
        Vector3.up,
        Vector3.down
    };

    void Start()
    {
        mainBlock = GameManager.instance.GetMainBlock();
        myRigidBody = this.gameObject.GetComponent<Rigidbody>();
        defaultMaterial = GetComponentInChildren<Renderer>().material;
        destroyMaterial = GameManager.instance.GetDestroyMaterial();
        Configure();
    }
    void Update()
    {
        TestDraw();
        ResetMaterial();
    }

    /// <summary>
    ///     Function that must be called in the main function, this function creates a ray in all directions and add a fixedjoint for all directions that have a block 
    /// </summary>
    private void Configure()
    {
        if (this.gameObject == mainBlock)
        {
            //Here like the indentify blocks function we add to the nonCollidingDirs list all directions that not have a block, i think i can just probaly call InfentifyBlocks here but whatever
            for (int i = 0; i < _directions.Length; i++)
            {
                bool colliding = Physics.Raycast(transform.position, transform.TransformDirection(_directions[i]), out _hit[i], rayDistance);

                if (!(colliding && _hit[i].collider.gameObject.tag == "Block"))
                {
                    nonCollidingDirs.Add(i);
                }
            }
        }
        else
        {
            //Here we add joint to pieces for they connect to the main block
            //We also configure the breakForce and the breakTorque to this "main joint"
            FixedJoint mainJoint = gameObject.AddComponent<FixedJoint>();
            mainJoint.connectedBody = mainBlock.gameObject.GetComponent<Rigidbody>();
            mainJoint.breakForce = breakForce;
            mainJoint.breakTorque = breakTorque;
            linkedBlocks.Add(mainBlock);

            //Here we add joint to pieces, they search blocks around and connect each other
            //ps.. for more "fixed joins" also add join to found block
            for (int i = 0; i < _directions.Length; i++)
            {
                bool colliding = Physics.Raycast(transform.position, transform.TransformDirection(_directions[i]), out _hit[i], rayDistance);

                if (colliding && _hit[i].collider.gameObject.tag == "Block" && _hit[i].collider.gameObject != mainBlock)
                {
                    linkedBlocks.Add(_hit[i].collider.gameObject);//Here we add to the linkedBlocks list all blocks linked to the block created
                    FixedJoint joint = gameObject.AddComponent<FixedJoint>();
                    joint.breakForce = breakForce;
                    joint.breakTorque = breakTorque;
                    Rigidbody nearblockRB = _hit[i].collider.gameObject.GetComponent<Rigidbody>();
                    joint.connectedBody = nearblockRB;
                    joint.enablePreprocessing = false;
                    //for the ps adjust: just get hit[i].collider.gameObject and .AddComponent<FixedJoint>() checking if this not already have this joint.
                }
                else
                {
                    nonCollidingDirs.Add(i);//Here we add to the nonCollidingDirs list all directions that not have a block linked
                }
            }
        }
        // else
        // {
        //     //Here we add joint to pieces for they connect to the main block
        //     //We also configure the breakForce and the breakTorque to this "main joint"
        //     HingeJoint mainJoint = gameObject.AddComponent<HingeJoint>();
        //     mainJoint.connectedBody = mainBlock.gameObject.GetComponent<Rigidbody>();
        //     mainJoint.breakForce = breakForce;
        //     mainJoint.breakTorque = breakTorque;
        //     linkedBlocks.Add(mainBlock);

        //     //Here we add joint to pieces, they search blocks around and connect each other
        //     //ps.. for more "fixed joins" also add join to found block
        //     for (int i = 0; i < _directions.Length; i++)
        //     {
        //         bool colliding = Physics.Raycast(transform.position, transform.TransformDirection(_directions[i]), out _hit[i], rayDistance);

        //         if (colliding && _hit[i].collider.gameObject.tag == "Block" && _hit[i].collider.gameObject != mainBlock)
        //         {
        //             linkedBlocks.Add(_hit[i].collider.gameObject);//Here we add to the linkedBlocks list all blocks linked to the block created
        //             HingeJoint joint = gameObject.AddComponent<HingeJoint>();
        //             joint.breakForce = breakForce;
        //             joint.breakTorque = breakTorque;
        //             Rigidbody nearblockRB = _hit[i].collider.gameObject.GetComponent<Rigidbody>();
        //             joint.connectedBody = nearblockRB;
        //             joint.enablePreprocessing = false;
        //             joint.enableCollision = true;
        //             joint.massScale = 10f;
        //             joint.connectedMassScale = 1f;

        //             joint.anchor = _directions[i] / 2;
        //             joint.axis = _directions[i];

        //             joint.useMotor = true;
        //             joint.motor = new JointMotor
        //             {
        //                 targetVelocity = 100.0f,
        //                 force = 100.0f
        //             };

        //             // joint.useLimits = true;
        //             // joint.limits = new JointLimits
        //             // {
        //             //     contactDistance = 0f
        //             // };
        //             //for the ps adjust: just get hit[i].collider.gameObject and .AddComponent<FixedJoint>() checking if this not already have this joint.
        //         }
        //         else
        //         {
        //             nonCollidingDirs.Add(i);//Here we add to the nonCollidingDirs list all directions that not have a block linked
        //         }
        //     }
        // }
    }
    /// <summary>
    ///     This function resets the block to the default material, 
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Observation
    ///     </para>
    ///     <para>
    ///         The diference of this and the SetToDefaultMaterial is that this function will be called on the update function and this function was made for we can reset material by exiting the EditMode or the DestroyMode without remove mouse over the block that we suposed want to delete.
    ///     </para>
    /// </remarks>

    public void ResetMaterial()
    {
        if ((!GameManager.instance.IsDetroying() || !GameManager.instance.IsEditing()) && (GetCurrentMaterial() != defaultMaterial))
        {
            SetToDefaultMaterial();
        }
    }
    /// <summary>
    ///     This function set the block to the destroy material.
    /// </summary>
    private void SetToDestroyMaterial()
    {
        this.gameObject.GetComponentInChildren<Renderer>().material = destroyMaterial;
    }
    /// <summary>
    ///     This function set the block to the default material.
    /// </summary>
    private void SetToDefaultMaterial()
    {
        this.gameObject.GetComponentInChildren<Renderer>().material = defaultMaterial;
    }
    /// <summary>
    ///     This function gets the current material used by block.
    /// </summary>
    private Material GetCurrentMaterial()
    {
        return this.gameObject.GetComponentInChildren<Renderer>().material;
    }
    /// <summary>
    ///     This is native function that check if the mouse exited over the block, i used this function to reset the block to the default material.
    /// </summary>
    private void OnMouseExit()
    {
        if (GameManager.instance.IsDetroying() && (this.gameObject != mainBlock))
        {
            SetToDefaultMaterial();
        }
    }
    /// <summary>
    ///     This is native function that check if the mouse is over the block, i used this function to set the block to the destroy material.
    /// </summary>
    private void OnMouseEnter()
    {
        if (GameManager.instance.IsDetroying() && (this.gameObject != mainBlock))
        {
            SetToDestroyMaterial();
        }
    }
    /// <summary>
    ///     This is native function that check if the mouse is clicked over the block, i used this function to set the selected block to camera lock over he.
    /// </summary>
    private void OnMouseDown()
    {
        GameManager.instance.SetSelectedBlock(this.gameObject);
    }
    /// <summary>
    ///     This is native function that check if the joint was breaked, i leave this here because we can probably use this function for instantiate particles or anything like this.
    /// </summary>
    private void OnJointBreak(float breakForce)
    {
        // print("broke");
    }
    /// <summary>
    ///     This is native function that check if the object was destroyed, i use this function to ordene all blocks linked in this, check the new direction without a block.
    /// </summary>
    private void OnDestroy()
    {
        foreach (GameObject block in linkedBlocks)
        {
            if (block != null)
            {
                block.GetComponent<ConfigureJoint>().IdentityBlocks();
            }
        }
    }

    /// <summary>
    ///     This public function is used for check directions that's not contains a block.
    /// </summary>
    public void IdentityBlocks()
    {
        nonCollidingDirs.Clear();
        for (int i = 0; i < _directions.Length; i++)
        {
            bool colliding = Physics.Raycast(transform.position, transform.TransformDirection(_directions[i]), out _hit[i], rayDistance);

            if (!(colliding && _hit[i].collider.gameObject.tag == "Block"))
            {
                // print(name + " - " + "Direção: " + i + ";");
                nonCollidingDirs.Add(i);
            }
        }
    }
    /// <summary>
    ///     Public function thats used for get the linkedBlocks list.
    /// </summary>
    /// <returns>
    ///     linkedBlocks list
    /// </returns>
    public List<GameObject> GetLinkedBlocks()
    {
        return linkedBlocks;
    }
    /// <summary>
    ///     Public function thats used for get the nonCollindingDirs list.
    /// </summary>
    /// <returns>
    ///     nonCollidingDirs list
    /// </returns>
    public List<int> GetNonColliderDirs()
    {
        return nonCollidingDirs;
    }
    /// <summary>
    ///     Private function that is used to draw and debug casted rays
    /// </summary>
    private void TestDraw()
    {
        for (int i = 0; i < _directions.Length; i++)
        {
            if (Physics.Raycast(transform.position, _directions[i], out _hit[i], 1.0f))
            {
                if (_hit[i].collider.gameObject.tag == "Block")
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(_directions[i]) * _hit[i].distance, Color.blue);
                }
            }
            else
            {
                Debug.DrawRay(transform.position, _directions[i] * rayDistance, Color.white);
            }
        }
    }

    /// <summary>
    ///     Private function to lock blocks when is in EditMode
    /// </summary>
    private void InEditMode()
    {
        myRigidBody.useGravity = false;
        myRigidBody.constraints = RigidbodyConstraints.FreezeAll;
        myRigidBody.Sleep();
        myRigidBody.isKinematic = true;
    }
    /// <summary>
    ///     Private function to unlock blocks when is out of EditMode
    /// </summary>
    private void OutOfEditMode()
    {
        myRigidBody.useGravity = true;
        myRigidBody.constraints = RigidbodyConstraints.None;
        myRigidBody.isKinematic = false;
    }
}