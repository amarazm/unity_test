namespace MyGame
{
    using System.Collections.Generic;
    using UnityEngine;

    public class formation_unit : MonoBehaviour
    {
        internal UnitFormation form;

        public float angle;

        public float speed;

        public GameObject unitPrefab;

        private List<prop> sprites;

        internal void Awake()
        {
            int unit_cnt = 5;

            speed = 1f;
            angle = 0f;
            form = new UnitFormation();
            sprites = new List<prop>();

            form.SetFormation(unit_cnt, formTypes.auto);

            form.onEndMove += Form_onEndMove;
            form.onEndRotate += Form_onEndRotate;
            form.onStartMove += Form_onStartMove;
            form.onStartRotate += Form_onStartRotate;

            transform.position = form.Get_leader_coord();

            for (int i = 0; i < unit_cnt; i++)
            {

                GameObject newUnit = Instantiate(unitPrefab);
                var prop = newUnit.GetComponent<prop>();
                sprites.Add(prop);
                //
                //int arrayIdx = Random.Range(0, unitSprites.Length);
                //Sprite unitSprite = unitSprites[arrayIdx];
                //string unitName = unitSprite.name;
                //     
                newUnit.name = "man" + i;
                //     newUnit.transform.position = new Vector3(Random.Range(-1*sz,1*sz),0,Random.Range(-1*sz,1*sz));

                newUnit.transform.position = form.positions[i];
                //     newUnit.transform.Rotate( new Vector3(1f, 0f, 0f) , 45);
                //
                prop.unitName = newUnit.name;
                prop.speed = 0.3f;

                //newUnit.GetComponent<SpriteRenderer>().sprite = unitSprite;

                prop.MoveTo(form.positions[i]);
            }


            //            form.MoveFormation(new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100)));

            MouseEvents.mouseClick.AddListener(onClick);
        }

        private void IdleAnimation()
        {
            if ((form.is_moving == false) && (form.is_rotating == false))
            {

                for (int i = 0; i < sprites.Count; i++)
                {
                    sprites[i].StopMovement();
                }
            }
        }

        private void WalkAnimation()
        {
            if ((form.is_moving == true) || (form.is_rotating == true))
            {
                for (int i = 0; i < sprites.Count; i++)
                {
                    sprites[i].Walk();
                    sprites[i].FaceTo(form.angle);
                }
            }
        }

        private void Form_onEndRotate(UnitFormation data)
        {
            Debug.Log("End rotate");
           // IdleAnimation();
        }

        private void Form_onEndMove(UnitFormation data)
        {
            Debug.Log("End move");
          //  IdleAnimation();
        }

        private void Form_onStartMove(UnitFormation data)
        {
            Debug.Log("Start move");
            WalkAnimation();
        }

        private void Form_onStartRotate(UnitFormation data)
        {
            Debug.Log("Start rotate");
            WalkAnimation();
        }

        internal void onClick(ClickEventData data)
        {
            if (data.keyPressed == mouseButtons.left_button)
            {
                form.SetRotateFormation(data.point);
                form.MoveFormation(data.point);
                Debug.Log("Clicked:" + data.point);

            }
            else if (data.keyPressed == mouseButtons.right_button)
            {
                form.SetRotateFormation(data.point);
                form.move_positions.Clear();

                angle = Utilities.GetAngle(form.Get_leader_coord(), data.point);

                Debug.Log("Clicked2:" + data.point + " angle=" + angle);
            }
        }

        internal void FixedUpdate()
        {
            if (form != null)
                form.UpdatePositions(1f * Time.deltaTime);

            //transform.position = form.leader_point;
            angle = form.angle;
            //Utilities.getAngle(form.leader_point, form.face_position);
            //angle = Vector3.Angle(transform.position, transform.position +Vector3.forward);

            for (int i = 0; i < sprites.Count; i++)
            {
               // sprites[i].MoveTo(form.positions[i]);
                //sprites[i].FaceTo(form.face_position*10);
                //if (form.angle < 180)                 sprites[i].FaceTo(Mathf.Abs(form.angle+180));                else                 sprites[i].FaceTo(Mathf.Abs(form.angle - 180));
            }
        }

        private void OnDrawGizmos()
        {
            if (form is null) return;

            form.OnDrawGizmos();

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.forward);
            Gizmos.color = Color.white;
        }
    }
}
