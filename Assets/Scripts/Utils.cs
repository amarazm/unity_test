namespace MyGame
{
    using UnityEngine;

    public class Utilities
    {
        public static unitFacing GetFacing(float angle, float zone = 23f)
        {
            // zone размер интервала от точек направлений  (0, 45, 90, 135 ...)
            float y = Mathf.Abs(angle);

            if (Mathf.Abs(y - 45f) <= zone) return unitFacing.NE;

            if (Mathf.Abs(y - 90f) <= zone) return unitFacing.E;

            if (Mathf.Abs(y - 360f) <= zone) return unitFacing.N;

            if (Mathf.Abs(y - 135f) <= zone) return unitFacing.SE;

            if (Mathf.Abs(y - 180f) <= zone) return unitFacing.S;

            if (Mathf.Abs(y - 225f) <= zone) return unitFacing.SW;

            if (Mathf.Abs(y - 270f) <= zone) return unitFacing.W;

            if (Mathf.Abs(y - 315f) <= zone) return unitFacing.NW;

            return unitFacing.NE;
        }

        public static float GetAngle(Vector3 unit_vector, Vector3 point_vector)
        {
            Vector3 dir = point_vector - unit_vector;

            float angle = Vector3.Angle(dir, Vector3.forward);

            if (dir.x < 0f) { angle -= 360; }

            return Mathf.Abs(angle);
        }

        public static float GetDistanceBetweenAngles(float cur_angle, float target_angle)
        {
            if (Mathf.Abs(cur_angle - target_angle) > 180f)
                return 360 - Mathf.Abs(cur_angle - target_angle);
            else
                return Mathf.Abs(cur_angle - target_angle);
        }

        public static float ChangeAngle(float cur_angle, float target_angle, float step)
        {
            if (Mathf.Abs(cur_angle - target_angle) <= step)
                return target_angle;

            if (Mathf.Abs(cur_angle - target_angle) > 180f)
            {
                if (cur_angle - target_angle < 0f)
                {
                    if (cur_angle - step >= 0f)
                        return cur_angle - step;
                    else
                        return cur_angle - step + 359f;
                }
                else
                {
                    if (cur_angle + step < 360f)
                        return cur_angle + step;
                    else
                        return cur_angle + step - 359f;

                }
            }
            else
            {
                if (cur_angle > target_angle)
                    return cur_angle - step;
                if (cur_angle < target_angle)
                    return cur_angle + step;
            }


            return 0;
        }
    }
}
