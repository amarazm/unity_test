using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Utils
    {

        // Update is called once per frame
        public unitFacing getFacing(float angle, float zone = 23f)
        {
            float y = Mathf.Abs(angle);

            if (Mathf.Abs(y - 45f) <= zone) return unitFacing.NE;

            if (Mathf.Abs(y - 90f) <= zone) return unitFacing.E;

            if (Mathf.Abs(y - 360f) <= zone) return unitFacing.N;

            if (Mathf.Abs(y - 135f) <= zone) return unitFacing.SE;

            if (Mathf.Abs(y - 180f) <= zone) return unitFacing.S;

            if (Mathf.Abs(y - 225f) <= zone) return unitFacing.SW;

            if (Mathf.Abs(y - 270f) <= zone) return unitFacing.W;

            if (Mathf.Abs(y - 315f) <= zone) return unitFacing.NW;

            return unitFacing.N;

        }
    }

}