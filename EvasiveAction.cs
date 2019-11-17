using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class EvasiveAction : MonoBehaviour
{
    public float dodge;
    public float smoothing;
    public float tilt;
    public Vector2 startWait;
    public Vector2 actionTime;
    public Vector2 actionWait;
    public Boundary boundary;

    private float currentSpeed;
    private float targetAction;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody> ();
        currentSpeed = rb.velocity.z;
        StartCoroutine(Evade());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float newAction = Mathf.MoveTowards (rb.velocity.x, targetAction, Time.deltaTime * smoothing);
        rb.velocity = new Vector3(newAction, 0.0f, currentSpeed);
        rb.position = new Vector3
        (
            Mathf.Clamp (rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }

    IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));

        while (true)
        {
            targetAction = Random.Range(1, dodge) * -Mathf.Sign (transform.position.x);
            yield return new WaitForSeconds(Random.Range (actionTime.x, actionTime.y));
            targetAction = 0;
            yield return new WaitForSeconds(Random.Range(actionWait.x, actionWait.y));
        }
    }
}
