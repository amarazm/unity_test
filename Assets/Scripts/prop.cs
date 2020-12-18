namespace MyGame
{
    using System.Collections.Generic;
    using UnityEngine;

    public class prop : MonoBehaviour
    {
        public Animator anim;

        public string unitName;

        public float Angle = 0f;

        internal playerTypes player = playerTypes.Human;

        public string unit_type_name;
        public string description;

        public int hitpoints;

        public float speed;

        public Vector3 move_to_position;       

        public int experience = 0;

        public int armor = 0;

        public int defence = 0;

        public int attack = 0;

        public int rangeAttack = 0;

        public int leadership = 100;

        public int closeWeaponSkill = 10;

        public int rangedWeaponSkill = 10;

        public int ammunition = 10;

        public bool is_selected = false;

        public unitStatuses status = unitStatuses.Idle;

        public unitFacing facing;
        // отличие направления камеры и юнита
        private int offset; 


        public bool returnToOrigin = false;

        private Vector3 origPos;

        internal Dictionary<int, string> AnimStatesIDLE = new Dictionary<int, string>  {
    {(int)unitFacing.E, "IDLE_E" },
    {(int)unitFacing.S, "IDLE_S" },
    {(int)unitFacing.W, "IDLE_W" },
    {(int)unitFacing.N, "IDLE_N" },
    {(int)unitFacing.NE, "IDLE_NE" },
    {(int)unitFacing.NW, "IDLE_NW" },
    {(int)unitFacing.SE, "IDLE_SE" },
    {(int)unitFacing.SW, "IDLE_SW" }
  };

        internal Dictionary<int, string> AnimStatesWALK = new Dictionary<int, string>  {
    {(int)unitFacing.E, "WALK_E" },
    {(int)unitFacing.S, "WALK_S" },
    {(int)unitFacing.W, "WALK_W" },
    {(int)unitFacing.N, "WALK_N" },
    {(int)unitFacing.NE, "WALK_NE" },
    {(int)unitFacing.NW, "WALK_NW" },
    {(int)unitFacing.SE, "WALK_SE" },
    {(int)unitFacing.SW, "WALK_SW" }
  };

        private string currentAnimState;

        public void MoveTo(Vector3 pt)
        {
            move_to_position = pt;
            FaceTo(pt);
            Walk();
        }

        private void OnDrawGizmos()
        {
            
            float size = 1f;
            //Gizmos.DrawLine(transform.position, move_to_position);
            Debug.DrawLine(transform.position, move_to_position, Color.red, 10);
            //            Gizmos.DrawLine(transform.position.normalized, Vector3.right.normalized);
            /*
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.forward);
            Gizmos.color = Color.white;
            */
        }

        public void StopMovement()
        {
            status = unitStatuses.Idle;
            FaceTo(move_to_position);
            move_to_position = new Vector3(0f, 0f, 0f);
        }

        public void Walk()
        {
            status = unitStatuses.Walk;
        }


        public void FaceTo(Vector3 pt)
        {
            Vector3 dir = pt - transform.position;
            Angle = Vector3.Angle(dir, Vector3.forward);
            if (dir.x < 0f) { Angle -= 360; }

            facing = Utilities.GetFacing(Angle);
//            Debug.Log("y=" + Angle + " " + dir.x + ":" + dir.y);
        }

        public void FaceTo(float _angle)
        {
            facing = Utilities.GetFacing(_angle);
            
        }

        internal void ChangeAnimationState(string newState)
        {
            // stop playing same animation
            if (currentAnimState == newState) return;
            // play animation
            //Debug.Log("ChangeAnimationState:" + unit_type_name + "_" + newState);
            anim.Play(unit_type_name + "_"+newState);


            currentAnimState = newState;
        }

        public void TakeDamage(int amount)
        {
            hitpoints -= amount;
        }

        internal void Init()
        {
            if (unit_type_name == "")
                unit_type_name = "Bernhard";

            CameraEvents.cameraRotate.AddListener(onCameraRotate);
            CameraEvents.cameraMove.AddListener(onCameraMove);
        }

        internal void Start()
        {
            Init();
        }

        private void FixedUpdate()
        {
            // дошел до точки
            if (transform.position == move_to_position)
            {
                status = unitStatuses.Idle;
            } else
            {
                CalcOffsetToCamera(CameraFacing.Instance.GetAngle());
                Walk();
            }

            if (status == unitStatuses.Walk)
            {
                transform.position = Vector3.MoveTowards(transform.position, move_to_position, speed * Time.deltaTime);
            }
        }

        public void onCameraRotate(CameraEventData data)
        {
            //Debug.Log(data.angle);
            RotateSprite2Camera(data);
        }

        public void onCameraMove(CameraEventData data)
        {
            //Debug.Log(data.new_position.ToString());
            RotateSprite2Camera(data);
        }

        public void CalcOffsetToCamera( float _camera_angle )
        {
            offset = (int)Utilities.GetFacing(_camera_angle);
            offset -= (int)facing;
            if (offset < 0)
            {
                offset += 8;
            }
        }

        void RotateSprite2Camera(CameraEventData data)
        {
            //offset = (int)CameraFacing.Instance.getFacing();
            CalcOffsetToCamera(data.angle);

            // rotate sprite to camera
            transform.LookAt(data.new_position);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
            transform.Rotate(new Vector3(1f, 0, 0), -45f);
        }

        internal void LateUpdate()
        {

            if (status == unitStatuses.Walk)
            {
                ChangeAnimationState(AnimStatesWALK[(int)offset]);
            }
            else if (status == unitStatuses.Idle)
            {
                ChangeAnimationState(AnimStatesIDLE[(int)offset]);
            }
        }
    }
}
