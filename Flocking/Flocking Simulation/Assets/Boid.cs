using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;
    [SerializeField] FlockingManager fManager;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        if (fManager == null)
        {
            fManager = FindObjectOfType<FlockingManager>();
        }
        //Choose character randomly (Just for fun)
        this.GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
        fManager.boids.Add(this);
        //rb.velocity = Random.insideUnitCircle.normalized * Random.Range(1f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}