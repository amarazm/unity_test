namespace MyGame
{
    using System.Collections.Generic;
    using UnityEngine;

    public class SpawnUnit : MonoBehaviour
    {
        public GameObject unitPrefab;

        public Sprite[] unitSprites;

        public List<GameObject> units;

        internal UnitFormation group_formation;

        public void MakeRandomSprite(int sz = 5)
        {
            int j = 0;

            group_formation = new UnitFormation();

            group_formation.Init();
            group_formation.Row3Formation(sz);


            for (int i = 0; i < sz; i++)
            {
                //GameObject newUnit = PrefabUtility.InstantiatePrefab(unitPrefab)  as GameObject;     
                GameObject newUnit = Instantiate(unitPrefab);
                var prop = newUnit.GetComponent<prop>();

                //
                int arrayIdx = Random.Range(0, unitSprites.Length);
                Sprite unitSprite = unitSprites[arrayIdx];
                string unitName = prop.anim.runtimeAnimatorController.name;
                Debug.Log("controller=" + prop.anim.runtimeAnimatorController.name);
                //     
                newUnit.name = unitSprite.name;
                //     newUnit.transform.position = new Vector3(Random.Range(-1*sz,1*sz),0,Random.Range(-1*sz,1*sz));

                newUnit.transform.position = new Vector3(i * 0.5f - ((i / 5) * 0.5f * 5f), 0, (i / 5) * 0.5f);
                //     newUnit.transform.Rotate( new Vector3(1f, 0f, 0f) , 45);
                //
                prop.unitName = unitSprite.name;
                prop.unit_type_name = "Man";
                prop.speed = 0.5f;

                newUnit.GetComponent<SpriteRenderer>().sprite = unitSprite;

                prop.MoveTo(group_formation.positions[i]);

                //prop.MoveTo(new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100)));
                //prop.MoveTo(new Vector3(Random.Range(105, 110), 0, Random.Range(105, 110)));
                //prop.MoveTo(new Vector3(Random.Range(-105, -110), 0, Random.Range(-105, -110)));
            }
        }

        public void MoveFormation()
        {
            group_formation.MoveFormation(new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100)));
        }
    }
}
