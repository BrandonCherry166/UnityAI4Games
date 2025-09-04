using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    public float radius;

    public float kCohesion;
    public float kAlignment;
    public float kSeparation;

    public float maxForce;
    public float maxSpeed;
    public List <Boid> boids;

    [SerializeField] GameObject boidPrefab;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < boids.Count; i++) 
        {
            Rigidbody2D rb = boids[i].GetComponent<Rigidbody2D>();
            Vector2 cohesion = ComputeCohesion(i);
            Vector2 alignment = ComputeAlignment(i);
            Vector2 separation = ComputeSeparation(i);

            Vector2 totalForce = cohesion + alignment + separation;
            rb.velocity += totalForce * Time.fixedDeltaTime;

            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }

            rb.MovePosition(rb.position + rb.velocity * Time.fixedDeltaTime);


            //Force to screen space
            Vector2 screenMin = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
            Vector2 screenMax = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
            float margin = 0.5f; // how close to the edge before steering

            Vector2 steer = Vector2.zero;
            if (rb.position.x < screenMin.x + margin) steer.x = 1;
            if (rb.position.x > screenMax.x - margin) steer.x = -1;
            if (rb.position.y < screenMin.y + margin) steer.y = 1;
            if (rb.position.y > screenMax.y - margin) steer.y = -1;

            rb.velocity += steer * maxForce;
        }
    }

    Vector2 ComputeCohesion(int currentBoidIndex)
    {
        List<Vector2> positions = new List<Vector2>();
        Vector2 currentPos = new Vector2(boids[currentBoidIndex].GetComponent<Rigidbody2D>().position.x, boids[currentBoidIndex].GetComponent<Rigidbody2D>().position.y);
        for (int i = 0; i < boids.Count; i++)
        {
            if (i == currentBoidIndex)
            { //Skip Comparing with Self
                continue;
            }

            if ((boids[currentBoidIndex].GetComponent<Rigidbody2D>().position - boids[i].GetComponent<Rigidbody2D>().position).magnitude <= radius)
            {
                positions.Add(boids[i].GetComponent<Rigidbody2D>().position);
            }
        }

        if (positions.Count > 0)
        {
            Vector2 totalPos = positions[0];
            for (int i = 1; i < positions.Count; i++)
            { //Positions 0 is already added
                totalPos += positions[i];
            }
            totalPos = totalPos / positions.Count;

            Vector2 finalForceVector = totalPos - currentPos;

            if (finalForceVector.magnitude > 0)
            {
                finalForceVector = finalForceVector.normalized * kCohesion;
            }

            return finalForceVector;
        }
        return new Vector2(0, 0);
    }

    Vector2 ComputeAlignment(int currentBoidIndex)
    {
        Vector2 totalVelocity = new Vector2(0, 0);
        int numNeighbors = 0;
        for (int i = 0; i < boids.Count; i++)
        {
            if ((boids[currentBoidIndex].GetComponent<Rigidbody2D>().position - boids[i].GetComponent<Rigidbody2D>().position).magnitude <= radius)
            {
                totalVelocity += boids[i].GetComponent<Rigidbody2D>().velocity;
                numNeighbors++;
            }
        }
        if (numNeighbors == 0)
        {
            return Vector2.zero;
        }
        Vector2 avgVelocity = totalVelocity / (float)numNeighbors;
        return avgVelocity * kAlignment;
    }

    Vector2 ComputeSeparation(int currentBoidIndex)
    {
        Vector2 totalForce = new Vector2(0,0);
        for (int i = 0; i < boids.Count; i++)
        {
            if (i == currentBoidIndex)
            {
                continue;
            }
            double distance = (boids[i].GetComponent<Rigidbody2D>().position - boids[currentBoidIndex].GetComponent<Rigidbody2D>().position).magnitude;
            if (distance <= radius && distance > 0)
            {
                Vector2 direction = boids[currentBoidIndex].GetComponent<Rigidbody2D>().position - boids[i].GetComponent<Rigidbody2D>().position;
                direction = direction / (float) distance; //Normalize
                direction = direction / (float) distance; //Apply 1/distance
                totalForce += direction;
            }
        }
        totalForce = totalForce * kSeparation;
        if (totalForce.magnitude > maxForce)
        {
            totalForce = totalForce.normalized * maxForce;
        }
        return totalForce;
    }

    public void SpawnBoid()
    {
        GameObject newBoid = Instantiate(boidPrefab, Random.insideUnitCircle * 5f, Quaternion.identity);
        Boid boidcomp = newBoid.GetComponent<Boid>();
        boidcomp.fManager = this;
        boids.Add(boidcomp);

    }

    public void RemoveBoid()
    {
        if (boids.Count == 0) return;
        Boid last = boids[boids.Count - 1];
        boids.RemoveAt(boids.Count - 1);
        Destroy(last.gameObject);
    }

}
