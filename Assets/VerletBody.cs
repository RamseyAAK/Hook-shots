using UnityEngine;

public class VerletBody : MonoBehaviour
{
    public Transform tf;
    private Transform prev_tf;

    void Start()
    {
        prev_tf = tf;
    }


    void Update()
    {
        tf.position -= new Vector3(0.0f, 0.001f, 0.0f);

        Transform temp = tf;
        tf.position += tf.position - prev_tf.position;
        prev_tf = temp;
    }
} 
