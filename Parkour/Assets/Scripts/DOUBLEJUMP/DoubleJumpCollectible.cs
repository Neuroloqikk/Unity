using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DoubleJumpCollectible : MonoBehaviour
{
    [SerializeField] private int doubleJumpAmount;
    public PlayerInventory playerInventory;

    private int playerDOubleJumps;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerDOubleJumps = playerInventory.doubleJumps;
            playerInventory.doubleJumps = doubleJumpAmount;
            gameObject.SetActive(false);
        }
    }


    private void Update()
    {
        print(transform.position.y);
    }

}
 