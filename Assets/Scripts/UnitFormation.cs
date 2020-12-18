using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace MyGame 
{
    
public class UnitFormation 
{
        //направление куда направлен отряд        
        public float angle;
        // направление куда отряд должен развернуться
        public float target_angle;
        //  скорость поворота в градусах
        float rotate_speed;
        float unit_speed;
        
        //число юнитов
        public int num_units;
        public int leader_index;
        
        public Vector3[] positions;

        public Vector3[] regroup_positions;
        public List<Vector3> move_positions;

        public delegate void FormationEvent(UnitFormation data);

        public event FormationEvent onStartMove;
        public event FormationEvent onEndMove;
        public event FormationEvent onStartRotate;
        public event FormationEvent onEndRotate;

        public event FormationEvent onStartRegroup;
        public event FormationEvent onEndRegroup;


        // начальный угол у формирования
        float starting_angle;
        private float min_found_distance;
        private int[] min_distance_found_indexes;

        // расстояние между юнитами 
        private float space_width;
        // расстояние между юнитами 
        private float space_height;

        // размеры юнитов
        private float unit_width;
        private float unit_height;

        private bool initialized;

        public bool is_moving;
        public bool is_rotating;
        public bool is_regrouping;


        public void Init()
        {
            move_positions = new List<Vector3>();
            starting_angle = 0f;
            angle = starting_angle;
            target_angle = angle;
            space_width = 0.1f;
            space_height = 0.1f;
            
            initialized = true;
            rotate_speed = 2f;
            unit_speed = 0.3f;
            leader_index = 0;
            num_units = 0;
            
            unit_width = 0.2f;
            unit_height = 0.2f;

            is_moving = false;
            is_rotating = false;
            is_regrouping = false;

            min_found_distance = 99999f;
        }

        public int Get_leader_index( int sz)
        {
            if (sz > 1)
                return 2;
            else
                return 1;
        }


        public Vector3 Get_leader_coord ()
        {
            if (leader_index < num_units)
                return positions[leader_index];
            else
                return Vector3.zero;
        }

        public void Set_formation_size( int sz)
        {
            positions = new Vector3[sz];
            leader_index = Get_leader_index(sz);
            num_units = sz;
        }

        public void Regroup(int unit_count)
        {        
        
        }

        /*public void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 100, 20), " = "+ angle);
        }*/

        public void OnDrawGizmos()
        {
            if ( (positions is null)|| ( num_units == 0 ) )
                return;

            for ( int i=0; i<num_units; i++)
            {
                switch (i)
                {
                    case 0:
                        Gizmos.color = Color.red;
                        break;
                    case 1:
                        Gizmos.color = Color.magenta;
                        break;
                    case 2:
                        Gizmos.color = Color.black;
                        break;
                    case 3:
                        Gizmos.color = Color.blue;
                        break;
                    case 4:
                        Gizmos.color = Color.cyan;
                        break;
                }                    
                
                Gizmos.DrawWireSphere(positions[i], 0.2f);
                //

                if ( !(regroup_positions is null) && (regroup_positions.Length > 0) )
                {
                    Gizmos.DrawWireSphere(regroup_positions[i]+new Vector3(0.001f, 0, 0.001f), 0.2f);
                    //
                }
            }

            //angle = Vector3.Angle(leader_point, Vector3.forward);
        }


        public void MoveFormation( Vector3 pts)
        {
            if (move_positions.Contains(pts))
            {
                return;
            }

            is_moving = true;
            onStartMove(this);
            move_positions.Clear();
            move_positions.Add(pts);
        }

        private void swap(ref int x, ref int  y)
        {
            int temp = x;
            x = y;
            y = temp;
        }

        private void swap(ref Vector3 x, ref Vector3 y)
        {
            Vector3 temp = x;
            x = y;
            y = temp;
        }

        private float getDistance(int index_from, int index_to)
        {
            if ((index_from >= num_units) || (index_to >= num_units)) {
                Debug.LogError("Wrong index "+ index_from+":"+ index_to);
                return 999f;
                    }
            return Vector3.Distance(positions[index_from], regroup_positions[index_to]);
        }

        private void generate(int[] a, int start_index, int sz)
        {
            if (start_index == sz - 1)
            { //Подсчет очередной перестановки
                float distance = 0f;
                for (int i = 1; i < sz; ++i)
                {
                    distance = distance + getDistance(i, a[i]);
                }
                Debug.Log("found_distance=" + distance + ": " + string.Join(",", a));

                if (distance < min_found_distance)
                {
                    min_found_distance = distance;
                    if (min_distance_found_indexes.Length != a.Length)
                    {
                        Array.Resize(ref min_distance_found_indexes, sz);
                    }
                    a.CopyTo(min_distance_found_indexes, 0);
                }
            }
            else
            {
                for (int j = start_index; j < sz; ++j)
                { //Запускаем процесс обмена
                    swap(ref a[start_index], ref a[j]); //a[t] со всеми последующими
                    start_index++;
                    generate(a, start_index, sz); //Рекурсивный вызов
                    start_index--;
                    swap(ref a[start_index], ref a[j]);
                }
            }
        }

        public void SetOptimalMoves(int sz)
        {
            if (sz < 3)
            {
                return ;
            }

            // путь лидера Vector3.Distance(p_from[0], p_to[0]);
            // поэтому пропускаем 0 индекс

/*            float[,] matrix_distances = new float[sz, sz];
            for (int i=1;i<sz;i++)
            {
                for (int j = i; j < sz; j++)
                {
                    matrix_distances[i,j] = Vector3.Distance(p_from[i], p_to[j]);
                    if (i!=j)
                    {
                        matrix_distances[i, j] = matrix_distances[j, i];
                    }
                }
            }*/

            int[] arr = Enumerable.Range(0, sz).ToArray();
            if (min_distance_found_indexes is null)
            {
                min_distance_found_indexes = new int[3];
            }

            if (min_distance_found_indexes.Length != num_units)
            {
                Array.Resize(ref min_distance_found_indexes, num_units);
            }

            generate( arr, 1, sz);
            Debug.Log("min_distance=" + min_found_distance + ": " + string.Join(",", min_distance_found_indexes));
            
            Vector3[] tmp = new Vector3[sz];
            regroup_positions.CopyTo(tmp,0);
            // проставим оптимальные маршруты для юнитов
            for (int i=1;i<sz;i++)
            {
                regroup_positions[i] = tmp[min_distance_found_indexes[i]];
            }

        }


        // расчитаем куда надо переместиться юнитам при повороте
        void CalcRegroupMovesOnTurning( float turn_angle)
        {
            is_regrouping = false;
            // если нужно развернуть отряд на 180 градусов проще переместить лидера
            switch (num_units)
            {
                case 5:
                    regroup_positions = GetFormation(num_units, positions[leader_index], turn_angle);
                    SetOptimalMoves(num_units);
                    is_regrouping = true;
                    break;
            }
        }

        public void SetRotateFormation( Vector3 pts)
        {
            target_angle = 360f - Utilities.GetAngle(Get_leader_coord(), pts);

            // если требуется поворот более чем на 90 градусов, то проще поменять лидера местами
            if (Utilities.GetDistanceBetweenAngles(angle,  target_angle) > 90f)
            {
                Debug.Log("Rotate From:"+ angle + " To: " + target_angle);
                Debug.Log("onStartRegroup");
                is_regrouping = false;
                // onStartRegroup(this);
                CalcRegroupMovesOnTurning(target_angle);
                is_regrouping = true;
            }
            else
            {
                is_rotating = true;
                onStartRotate(this);

                Debug.Log("target_angle=" + target_angle + " angle=" + angle);
            }
        }

        private Vector3 RotateVector2D(Vector3 oldDirection, float move_angle, Vector3 leader_coord )
        {
            float newX = Mathf.Cos(move_angle * Mathf.Deg2Rad) * (oldDirection.x- leader_coord.x) - Mathf.Sin(move_angle * Mathf.Deg2Rad) * (oldDirection.z- leader_coord.z);
            float newZ = Mathf.Sin(move_angle * Mathf.Deg2Rad) * (oldDirection.x- leader_coord.x) + Mathf.Cos(move_angle * Mathf.Deg2Rad) * (oldDirection.z- leader_coord.z);
            float newY = oldDirection.y;
            return new Vector3(newX+ leader_coord.x, newY, newZ + leader_coord.z);
        }

        void UpdateRegroup()
        {
            int units_moved = 0;

            if ( (!is_regrouping) || (regroup_positions is null) )
                return;

            float distance_delta = unit_speed * Time.deltaTime;

            for (int i=0; i< num_units; i++)
            {
                if (!positions[i].Equals(regroup_positions[i]))
                {
                    units_moved++;
                    positions[i] = Vector3.MoveTowards(positions[i], regroup_positions[i], distance_delta);
                }
            }

            if (units_moved == 0)
            {
                angle = target_angle;
                is_regrouping = false;
                Debug.Log("onEndRegroup");
                //                onEndRegroup.Invoke(this);
            }

        }

        private void UpdateRotation()
        {
            if (!is_rotating)
                return;

            float rotate_step = 1f;

            // делаем вращение
            if ((angle != target_angle) && Mathf.Abs(angle - target_angle) > rotate_step)
            {
                float new_angle = Utilities.ChangeAngle(angle, target_angle, rotate_step);

                if (new_angle < angle)
                    rotate_step = -rotate_step;

                for (int i = 0; i < num_units; i++)
                {
                    positions[i] = RotateVector2D(positions[i], rotate_step, Get_leader_coord());
                }
                this.angle = new_angle;
            }
            else
            {
                if (is_rotating)
                {
                    is_rotating = false;
                    onEndRotate(this);
                }

            }

        }

        public void UpdateMovement()
        {
            if ( (!is_moving)||(is_regrouping))
                return;

            // делаем перемещение
            Vector3 leader_point = Get_leader_coord();
            Vector3 old_leader_point = leader_point;
            float distance_delta = unit_speed * Time.deltaTime;

            leader_point = Vector3.MoveTowards(leader_point, move_positions[0], distance_delta);
            // если уже в точке куда шли
            if ((Mathf.Abs(leader_point.x - move_positions[0].x) <= distance_delta) && (Mathf.Abs(leader_point.y - move_positions[0].y) <= distance_delta) && (Mathf.Abs(leader_point.z - move_positions[0].z) <= distance_delta) && is_moving)
            {
                is_moving = false;
                onEndMove(this);
                move_positions.Clear();
            }

            for (int i = 0; i < num_units; i++)
            {
                positions[i] = positions[i] + (leader_point - old_leader_point);
            }

        }

        public void UpdatePositions( float distance_delta)
        {

            float rotate_step = rotate_speed;
            // если некого двигать, выходим
            if ((positions is null) || (num_units == 0))
            {
                return;
            }

            UpdateRegroup();
            UpdateRotation();

           // 
            if ( (move_positions is null) || (move_positions.Count==0) )
                return;

            UpdateMovement();

        }

        public void LineFormation (int size)
        {
            SetFormation(size, formTypes.one_line);
        }

        public void RowFormation(int size)
        {
            SetFormation(size, formTypes.one_row);
        }

        public void Row2Formation(int size)
        {
            SetFormation(size, formTypes.two_rows);
        }

        public void Row3Formation(int size)
        {
            SetFormation(size, formTypes.three_rows);
        }

        public void Line2Formation(int size)
        {
            SetFormation(size, formTypes.two_lines);
        }


        Vector3[] GetFormation( int size , Vector3 leader_pos, float rotate_angle)
        {
            Vector3[] res = new Vector3[size];

            if (size == 5)
            {
                res[0] = leader_pos;
                res[1] = new Vector3(leader_pos.x + space_width + unit_width, 0, leader_pos.z);
                res[2] = new Vector3(leader_pos.x - space_width - unit_width, 0, leader_pos.z);
                res[3] = new Vector3(leader_pos.x - (space_width + unit_width) / 2, 0, leader_pos.z - (space_height + unit_height));
                res[4] = new Vector3(leader_pos.x + (space_width + unit_width) / 2, 0, leader_pos.z - (space_height + unit_height));
            }
            // если нужно, то повернём формирование на угол
            if (rotate_angle > 0f)
            {
                for (int i = 0; i< size; i++)
                {
                    res[i] = RotateVector2D(res[i], rotate_angle, leader_pos);
                }
            }

            return res;
        }

            public void SetFormation( int size, formTypes formation )
    {
            int units_per_row;

            if (!initialized)
                Init();

            Set_formation_size(size);

            angle = starting_angle;
            target_angle = angle;

            num_units = size;

            switch (formation)
            {
                case formTypes.one_row:
                    for (int i = 0; i < size; i++)
                    {
                        positions[i] = new Vector3(0, 0, unit_height * i + space_height * i);
                    }
                    break;
                case formTypes.auto:
                    switch (size)
                    {
                        case 1:
                            positions[0] = new Vector3(0, 0, 0);
                            leader_index = 0;
                            break;
                        case 2:
                            positions[0] = new Vector3(0, 0, 0);
                            positions[1] = new Vector3(0 + space_width + unit_width, 0, 0);
                            leader_index = 0;
                            break;
                        case 3:
                            positions[0] = new Vector3(0, 0, 0);
                            positions[1] = new Vector3(0 - space_width - unit_width, 0 + space_height + unit_height, 0);
                            positions[2] = new Vector3(0 + space_width - unit_width, 0 + space_height + unit_height, 0);
                            leader_index = 0;
                            break;
                        case 4:
                            positions[0] = new Vector3(0, 0, 0);
                            positions[1] = new Vector3(0 + space_width + unit_width, 0, 0);
                            positions[2] = new Vector3(0 , 0, 0 + space_height + unit_height);
                            positions[3] = new Vector3(0 + space_width + unit_width, 0, 0 + space_height + unit_height);
                            leader_index = 0;
                            break;
                        case 5:
                            positions = GetFormation(5, Vector3.zero, 0f);
                            leader_index = 0;
                            break;
                            default:
                                for (int i = 0; i < size; i++)
                                {
                                    positions[i] = new Vector3((i / 4) * (unit_width + space_width), 0, unit_height * (i % 4) + space_height * (i % 4));
                                }
                              break;
                    }
                    break;
            }

            //       
    }
}

    public class UnitFormationEventData
    {
        UnitFormation form;


    }

}