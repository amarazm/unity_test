namespace MyGame
{
    using UnityEngine;
    using UnityEngine.Events;

    public static class CameraEvents
    {
        public static cameraEvent cameraRotate = new cameraEvent();
        public static cameraEvent cameraMove = new cameraEvent();
    }

    public class cameraEvent : UnityEvent<CameraEventData>
    {
    }


;

    public class CameraEventData
    {
        public float angle;
        public Vector3 new_position;
        public Vector3 old_position;


        public CameraEventData(float angle, Vector3 new_position , Vector3 old_position )
        {
            this.angle = angle;
            this.new_position = new_position;
            this.old_position = old_position;
        }


    }
}
