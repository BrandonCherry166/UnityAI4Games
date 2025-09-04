using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    [SerializeField] double radius;
    [SerializeField] float k;
    [SerializeField] float maxForce;
    private GameObject [] boids;
    List<Boid> currentState, newState;
    // Start is called before the first frame update
    void Start()
    {
        boids = GameObject.FindGameObjectsWithTag("Boid");
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < currentState.Count; i++)
        {
            Vector2 cohesionForce = ComputeCohesion(i);
            Vector2 allignmentForce = ComputeAllignment(i);
            Vector2 separationForce = ComputeSeparation(i);

            Vector2 totalForce = cohesionForce + allignmentForce + separationForce;
            newState[i].GetComponent<Rigidbody2D>().velocity = currentState[i].GetComponent<Rigidbody2D>().velocity + totalForce * Time.deltaTime;
            newState[i].GetComponent<Rigidbody2D>().position = currentState[i].GetComponent<Rigidbody2D>().position = newState[i].GetComponent<Rigidbody2D>().velocity * Time.deltaTime;
        }
        SwitchLists(currentState, newState);
    }

    Vector2 ComputeCohesion(int currentBoidIndex)
    {
        List<Vector2> positions = new List<Vector2>();
        Vector2 currentPos = new Vector2(boids[currentBoidIndex].GetComponent<Rigidbody2D>().position.x, boids[currentBoidIndex].GetComponent<Rigidbody2D>().position.y);
        for (int i = 0; i < boids.Length; i++)
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
                finalForceVector = finalForceVector.normalized * k;
            }

            return finalForceVector;
        }
        return new Vector2(0, 0);
    }

    Vector2 ComputeAllignment(int currentBoidIndex)
    {
        Vector2 totalVelocity = new Vector2(0, 0);
        int numNeighbors = 0;
        for (int i = 0; i < boids.Length; i++)
        {
            if ((boids[currentBoidIndex].GetComponent<Rigidbody2D>().position - boids[i].GetComponent<Rigidbody2D>().position).magnitude <= radius)
            {
                totalVelocity += boids[i].GetComponent<Rigidbody2D>().velocity;
                numNeighbors++;
            }
        }
        if (numNeighbors == 0)
        {
            return boids[currentBoidIndex].GetComponent<Rigidbody2D>().velocity;
        }
        Vector2 avgVelocity = totalVelocity / (float)numNeighbors;
        return avgVelocity * k;
    }

    Vector2 ComputeSeparation(int currentBoidIndex)
    {
        Vector2 totalForce = new Vector2(0,0);
        for (int i = 0; i < boids.Length; i++)
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
        totalForce = totalForce * k;
        if (totalForce.magnitude > maxForce)
        {
            totalForce = totalForce.normalized * maxForce;
        }
        return totalForce;
    }

    void SwitchLists (List <Boid> list1, List<Boid> list2)
    {
        List<Boid> tempList = list1;
        list1 = list2;
        list2 = tempList;
    }

}
