using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame 
{

public interface IKillable
{
    void Kill();
}

public interface IDamageable
{
    int  hitpoints { get; set; }    
    void TakeDamage(int amount );    
}

public interface IMoveable
{
    Vector3 move_to_position { get; set; }     
    float speed {get; set; }
    unitFacing facing { get; set; }

    void MoveTo( Vector3 point );
    void StopMovement();
    void Walk();
    void FaceTo(Vector3 pt);
    void FaceTo(float angle);
    }

public interface ISelectable {
    bool is_selected { get; set; }

    void Select();
    void Unselect();
}

 public interface IUnit : ISelectable, IMoveable, IKillable
{
   
        void Attack(Vector3 point);
        void Init();

}


public interface IFormation {
     formTypes formation {get; set;}
     unitFacing facing { get; set;} 

     Vector3[] positions { set; get; }

        void Init();
        void Regroup();


}

}