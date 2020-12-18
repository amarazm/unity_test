namespace MyGame
{
    using UnityEngine;
    using UnityEngine.Events;

    public static class MouseEvents
    {
        public static mouseEvent mouseClick = new mouseEvent();
    }

    public class mouseEvent : UnityEvent<ClickEventData>
    {
    }


;

    public class ClickEventData
    {
        public mouseButtons keyPressed;

        public bool is_unit_selected;

        public bool is_terrain;

        public Vector3 point;

        public GameObject unit;

        public ClickEventData(mouseButtons keypressed, Vector3 point, GameObject unit, bool is_unit_selected = false, bool is_terrain = false)
        {
            this.keyPressed = keypressed;
            this.point = point;
            this.unit = unit;
            this.is_unit_selected = is_unit_selected;
            this.is_terrain = is_terrain;
        }
    }
}
