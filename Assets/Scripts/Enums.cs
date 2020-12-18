namespace MyGame
{
    public class Constants
    {
        public const string c_terrain_tag = "Terrain";
    }

public  enum mouseButtons
    {
        left_button,
        right_button,
        middle_button
    }

public enum unitStatuses
{
    Idle,
    Walk,
    Fly,
    Attack,
    Cast,
    Died
}

public enum unitFacing {
  N,
  NE,
  E,
  SE,
  S,
  SW,
  W,
  NW  
  
}


public enum playerTypes {
  Human,
  Computer1 ,
  Computer2
}

public enum formTypes {
  one_row,  
  two_rows,
  three_rows,
  four_rows,

  one_line,
  two_lines,
  three_lines,

  v_line,
  auto
}

}