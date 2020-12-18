using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraAngle = 0f;
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;    

    private float targetZoom;            
    float zoomFactor = 3f;        
    float zoomLerpSpeed = 10;                    

    public float SharpnessZoom = 100;// скорость зума
    public float CameraPosition = 100;
    public int CameraZoomMax = 10;
    public int CameraZoomMin = 500;
    public float CameraSpeed = 10;
    public RaycastHit Hit;
    //public MouseState MS;


    // Start is called before the first frame update
    void Start()
    {
       // cam = Camera.main;
     //  MS - gameObject.GetComponent<MouseState>();
    }

    // Update is called once per frame
    void Update()
    {
        float scrollData;

        CameraHeightPosition();
        CameraMovement();
        Debug.Log(transform.localEulerAngles.y);
/*
        if (Input.GetMouseButton(2) ) {
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3( pitch, yaw, 0.0f);       
        } 

        scrollData = Input.GetAxis("Mouse ScrollWheel");
        targetZoom -= scrollData * zoomFactor;     
*/  
    }

    void CameraHeightPosition()
    {
        Vector3 DirectionRay = transform.TransformDirection( Vector3.forward);
        if (Physics.Raycast( transform.position, DirectionRay, out Hit, 1200) ) {
            Debug.Log(Hit.collider.name + "," + Hit.collider.tag);            
            if (Hit.collider.tag == "terrain") {
                Debug.Log("distance="+ Hit.distance + ", pos="+ CameraPosition);
                if (Hit.distance < CameraPosition) {
                    transform.position += new Vector3 (0, CameraPosition - Hit.distance,0);
                }    
                else
                {
                    transform.position -= new Vector3 (0, Hit.distance - CameraPosition,0);                    
                }   

            if (Input.GetAxis("Mouse ScrollWheel") < 0 //&& CameraPosition < CameraZoomMin
            ) {
                CameraPosition += 2 * SharpnessZoom * Time.deltaTime;
                CameraSpeed += 0.007f;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0 //&& CameraPosition > CameraZoomMax
            ) {
                CameraPosition -= 2 * SharpnessZoom * Time.deltaTime;
                CameraSpeed -= 0.007f;                
            }

            }
        }//raycast
    }

    void CameraMovement()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) ) {
            transform.position += new Vector3 ( 0, 0, 1 );
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) ) {
            transform.position += new Vector3 ( 0, 0, -1 );
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) ) {
            transform.position += new Vector3 ( -1, 0, 0 );
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) ) {
            transform.position += new Vector3 ( 1, 0, 0 );
        }

    }

    
}
