using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        //Choose character randomly (Just for fun)
        this.GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}