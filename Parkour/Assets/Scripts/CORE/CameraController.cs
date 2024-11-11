using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{
    //RoomCamera
    [Header("Camera")]
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;
    private Camera cameraFreeWalk;
    [SerializeField] private float zoomSpeed = 20f;
    [SerializeField] private float minZoomFOV = 10f;
    [SerializeField] private float maxZoomFOV = 160f;
    [SerializeField] private float yOffset = 2f;

    //Follow player
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;

    private void Awake()
    {
        cameraFreeWalk = GetComponent<Camera>();
    }

    private void Update()
    {

        //Room Camera
        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed);

        //Follow Player
        transform.position = new Vector3(transform.position.x, player.position.y + yOffset, transform.position.z);

        //Follow Player Ahead
        //transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
        //lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ZoomIn();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ZoomOut();
        }
    }

    public void ZoomIn()
    {
        cameraFreeWalk.fieldOfView -= zoomSpeed;
        if (cameraFreeWalk.fieldOfView < minZoomFOV)
        {
            cameraFreeWalk.fieldOfView = minZoomFOV;
        }
    }


    public void ZoomOut()
    {
        cameraFreeWalk.fieldOfView += zoomSpeed;
        if (cameraFreeWalk.fieldOfView > maxZoomFOV)
        {
            cameraFreeWalk.fieldOfView = maxZoomFOV;
        }
    }

    public void MoveToNewRoom(Transform _newRoom)
    { 
        currentPosX = _newRoom.position.x;
    }
}
