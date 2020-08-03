using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    
    public float rotationSpeed;

    public float cameraDistance;
    public float speed;

    public float mouseSensitivity = 1;
    public float mouseSmoothing = 2;
    public GameObject player;
    public GameObject playerCamera;

    private float rotateAngle(float original, float dAngle)
    {
        if (dAngle > 0) return (original + dAngle) % 360;
        else return (original + (360 + dAngle)) % 360;
    }

    public float degreeToRad(float angle)
    {
        float result;
        result = angle * (float)Mathf.PI;
        result = result / 180;
        return result;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get camera rotation to rotate player 
        float cameraRotationY = playerCamera.transform.rotation.eulerAngles[1];
        float playerRotationY = player.transform.rotation.eulerAngles[1];
        // get input
        float hVal = Input.GetAxis("Horizontal");
        float vVal = Input.GetAxis("Vertical");
        Vector3 playerPos;
        // if there's input
        if (vVal != 0 || hVal != 0)
        {
            if (hVal == 0)
            {
                // straight backwards
                if (vVal < 0) cameraRotationY = rotateAngle(cameraRotationY, 180);
                // if forwards no rotation needed
            }
            else if (vVal == 0)
            {
                // straight right
                if (hVal > 0) cameraRotationY = rotateAngle(cameraRotationY, 90);
                // straight left
                else cameraRotationY = rotateAngle(cameraRotationY, -90);
            }
            else
            {
                if (vVal > 0)
                {
                    // forward right
                    if (hVal > 0) cameraRotationY = rotateAngle(cameraRotationY, 45);
                    // forward left
                    else cameraRotationY = rotateAngle(cameraRotationY, -45);
                }
                else
                {
                    // backward right
                    if (hVal > 0) cameraRotationY = rotateAngle(cameraRotationY, 135);
                    // backward left
                    else cameraRotationY = rotateAngle(cameraRotationY, -135);
                }
            }

            float dAngle = cameraRotationY - playerRotationY;

            // find closest rotation - clockwise or counterclockwise
            if (dAngle < -180) dAngle += 360;
            else if (dAngle >  180) dAngle -= 360;

            // rotate player
            if (dAngle > 1)
            {
                playerRotationY += rotationSpeed;
                player.transform.localRotation = Quaternion.Euler(0, playerRotationY, 0);
            }
            else if(dAngle < -1)
            {
                playerRotationY -= rotationSpeed;
                player.transform.localRotation = Quaternion.Euler(0, playerRotationY, 0);
            }
            // move player
            playerPos = player.transform.position;
            playerPos[2] += Mathf.Cos(Mathf.Deg2Rad*playerRotationY) * speed;
            playerPos[0] += Mathf.Sin(Mathf.Deg2Rad*playerRotationY) * speed;
            player.transform.position = playerPos;
           
        }

        playerPos = player.transform.position;
        // move camera accordingly
        Vector3 cameraPos = playerCamera.transform.position;  
        cameraPos[1] = 20;
        if (Vector3.Distance(playerCamera.transform.position, playerPos) > cameraDistance + 1)
        {
            cameraPos = Vector3.MoveTowards(cameraPos, playerPos, 0.05f);
        }
        else if (Vector3.Distance(playerCamera.transform.position, playerPos) < cameraDistance - 1)
        {
            cameraPos = Vector3.MoveTowards(cameraPos, playerPos, -0.05f);
        }

        Vector2 smoothMouseDelta = Vector2.Scale(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")), Vector2.one * mouseSensitivity);    

        float dz = playerPos[2] - cameraPos[2];
        float dx = playerPos[0] - cameraPos[0];
        float cameraToPlayerAngle = Mathf.Atan2(dx, dz) * Mathf.Rad2Deg;
        float playerToCameraAngle = rotateAngle(cameraToPlayerAngle, -90);

        playerCamera.transform.localRotation = Quaternion.Euler(0, cameraToPlayerAngle, 0);
        playerCamera.transform.position = cameraPos;

    }
}
