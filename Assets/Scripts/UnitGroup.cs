using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyGame
{

    public class UnitGroup : MonoBehaviour
    {
        UnitFormation _form;
        unitFacing _face;
        public List<GameObject> _units;
        // Start is called before the first frame update
        void Start()
        {
            _face = unitFacing.E;
            _form = new UnitFormation();
        }


        public void Init(List<GameObject> units)
        {
            _form.SetFormation(units.Count, formTypes.three_rows);
        }


        public void Regroup( )
        {
        }

        // Update is called once per frame
        void Update()
        {
            //_units _form.positions
        }
    }

}