using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSprite : MonoBehaviour
{
    public Sprite sprite;
    private BoxCollider boxCollider;
 
    void Start()
     {
         GameObject go = new GameObject("New Sprite");
         SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
         sprite = Resources.Load<Sprite> ("Assets/Sprites/Neutral_alpha.png");
         renderer.sprite = sprite;
         boxCollider = go.AddComponent<BoxCollider>();
//         boxCollider.transform.
     }   

    // Update is called once per frame
    void Update()
    {
        
    }
}
