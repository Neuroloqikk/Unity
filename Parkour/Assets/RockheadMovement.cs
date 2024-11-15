using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RockheadMovement : MonoBehaviour
{
    private Animator rockheadAnim;
    private bool isOnRightWall = true;
    private bool isMoving = false;

    [Header("Parent configs")]
    public Transform parentBody;
    public float speed = 2f;

    private void Awake()
    {
        rockheadAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isMoving)
        {
            if (isOnRightWall)
                parentBody.position = new Vector3(parentBody.position.x - speed, parentBody.position.y, parentBody.position.z);
            else
                parentBody.position = new Vector3(parentBody.position.x + speed, parentBody.position.y, parentBody.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isOnRightWall)
        {
            if (collision.tag == "LeftWall")
            { 
                rockheadAnim.SetTrigger("isLeftHit");
                isMoving = false;
                isOnRightWall = false;
            }
    }
        else
        {
            if (collision.tag == "RightWall")
            {
                rockheadAnim.SetTrigger("isRightHit");
                isMoving = false;
                isOnRightWall = true;
            } 
        }
    }

    private void GoAgainstOppositeWall()
    {
        isMoving = true;
        rockheadAnim.SetBool("isMoving", true);
    }

    private void ChangeBackToIdle()
    {
        rockheadAnim.SetBool("isMoving", false);
    }
}
