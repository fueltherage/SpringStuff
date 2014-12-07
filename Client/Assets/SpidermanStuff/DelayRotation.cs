using UnityEngine;
using System.Collections;

public class DelayRotation : MonoBehaviour {

	// Use this for initialization
    public Transform reference;
    public float Delay = 0.3f;
    Quaternion previousRotation;
	void Start () 
    {
        previousRotation = transform.localRotation;   
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.localRotation *= Quaternion.Inverse(transform.parent.rotation);
        transform.rotation = Quaternion.Slerp(previousRotation, reference.rotation,Time.deltaTime * Delay);
        previousRotation = transform.rotation;
	}
}
