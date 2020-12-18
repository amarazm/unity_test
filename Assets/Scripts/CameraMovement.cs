namespace MyGame
{
    using System;
    using UnityEngine;

    public class CameraMovement : MonoBehaviour
    {

        public unitFacing facing;

        internal float speed = 0.08f;

        internal float zoomSpeed = 20.0f;

        internal float rotateSpeed;

        internal float maxHeight = 60f;

        internal float minHeight = 3f;

        internal Sprite compass;

        internal void Start()
        {
        }

        internal void FixedUpdate()
        {
            bool is_rotate = false;
            bool is_move = false;
            RaycastHit hit;

            float hsp = speed * Input.GetAxis("Horizontal");
            float vsp = speed * Input.GetAxis("Vertical");
            float scrollSp = -zoomSpeed * Input.GetAxis("Mouse ScrollWheel");

            Vector3 rotateValue;
            Vector3 old_position = transform.position; 

            /*if (Input.GetKey(KeyCode.Z)) {
              rotateValue = new Vector3( -1, 0, 0);
              transform.eulerAngles = transform.eulerAngles - rotateValue;            
            } else
            if (Input.GetKey(KeyCode.X)) {
              rotateValue = new Vector3( 1, 0, 0);
              transform.eulerAngles = transform.eulerAngles - rotateValue;            
            } else*/
            if (Input.GetKey(KeyCode.Q))
            {
                rotateValue = new Vector3(0, 1, 0);
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                if (Physics.Raycast(ray, out hit, 100))
                {
                        transform.RotateAround(hit.point, Vector3.up, 1);
                }
                //transform.eulerAngles = transform.eulerAngles - rotateValue;
                facing = CameraFacing.Instance.getFacing();
                is_rotate = true;
            }
            else
            if (Input.GetKey(KeyCode.E))
            {
                rotateValue = new Vector3(0, -1, 0);

                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                if (Physics.Raycast(ray, out hit, 100))
                {
                    transform.RotateAround(hit.point, Vector3.up, -1);
                }

                //transform.eulerAngles = transform.eulerAngles - rotateValue;
                facing = CameraFacing.Instance.getFacing();
                is_rotate = true;
            }


            if ( (vsp != 0) || (hsp != 0)|| (scrollSp !=0))
            {
                Vector3 verticalMove = new Vector3(0, scrollSp, 0);
                Vector3 lateralMove = hsp * transform.right;
                Vector3 forwardMove = transform.forward;
                forwardMove.y = 0;
                forwardMove.Normalize();
                forwardMove *= vsp;
                Vector3 move = verticalMove + lateralMove + forwardMove;

                if (transform.position.y + move.y > minHeight && transform.position.y + move.y < maxHeight)
                {
                    is_move = true;                    
                    transform.position += move;
                }
            }

            if (is_rotate || is_move)
            {
                CameraEvents.cameraRotate.Invoke(new CameraEventData(transform.eulerAngles.y, transform.position, old_position ));
            }


        }
    }
}
