using UnityEngine;
using System.Collections;

public class Spring : MonoBehaviour {
    public LayerMask cutMask;
	public GameObject springPoint;
	public LineRenderer line;
	public GameObject point1;
	public GameObject point2;
	public bool active = false;


	public float SpringStrength = 1.0f; 
	public float radius = 1.0f;
	public float damping = 1;
	public float LineMin = 0.25f;
	public float LineMax = 4.0f; 
	public float stretchDistance = 3.0f;
	float LineHeight;


	float elapsedTime=0;

	Vector3 pastDif1;
	Vector3 pastDif2;
	// Use this for initialization
	void Start ()
	{
		pastDif1 = Vector3.zero;
		pastDif2 = Vector3.zero;
		line =  GetComponent<LineRenderer>();
		line.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		if(active)
		SpringForce();

	}
	void OnDestroy()
	{
		print ("Destroyed points");
		Destroy(point1);
		Destroy(point2);
	}
	void SpringForce()
	{
		Vector3 parentDifference = point1.transform.parent.position - point2.transform.parent.position;
		Vector3 difference = point1.transform.position - point2.transform.position;
		 

		Vector3 point1ParentOffset = point1.transform.position - point1.transform.parent.position;
		Vector3 point2ParentOffset = point2.transform.position - point2.transform.parent.position;

		Vector3 Cross = Vector3.Cross(point1ParentOffset.normalized, parentDifference.normalized);
		float theta = Mathf.Asin(-Cross.magnitude);
		Vector3 w = Cross.normalized * theta / Time.fixedDeltaTime;
		Quaternion q = point1.transform.parent.rotation * point1.transform.parent.rigidbody.inertiaTensorRotation;
		Vector3 T = q * Vector3.Scale(point1.transform.parent.rigidbody.inertiaTensor, (Quaternion.Inverse(q) * w));

		Vector3 pointForce = -SpringStrength * difference.normalized * (difference.magnitude - radius);

		point1.transform.parent.rigidbody.AddTorque(T,ForceMode.Force);
		point1.transform.parent.rigidbody.AddForce(pointForce);
		point1.transform.parent.rigidbody.AddForce((difference - pastDif1)*damping);

		difference = -difference;

		Cross = Vector3.Cross(point2ParentOffset.normalized, parentDifference.normalized);
		theta = Mathf.Asin(Cross.magnitude);
		 w = Cross.normalized * theta / Time.fixedDeltaTime;
		 q = point2.transform.parent.rotation * point2.transform.parent.rigidbody.inertiaTensorRotation;
		 T = q * Vector3.Scale(point2.transform.parent.rigidbody.inertiaTensor, (Quaternion.Inverse(q) * w));
		
		pointForce = -SpringStrength * difference.normalized * (difference.magnitude - radius);


		point2.transform.parent.rigidbody.AddTorque(T,ForceMode.Force);
		point2.transform.parent.rigidbody.AddForce(pointForce);
		point2.transform.parent.rigidbody.AddForce((difference - pastDif2)*damping);


		pastDif1 = -difference;
		pastDif2 = difference;

		ConnectLine();
		elapsedTime = Time.deltaTime;
		LineHeight = Mathf.Clamp(stretchDistance* (radius/difference.magnitude) , LineMin, LineMax);
		//LineHeight = difference.magnitude;
		//Debug.Log(LineHeight + " " +difference.magnitude);
		
		//line.renderer.material.SetTextureOffset("_MainTex", new Vector2(difference.magnitude/3.0f, 0));
		//line.renderer.material.SetTextureScale("_MainTex", new Vector2(difference.magnitude/3.0f, 1));
		line.SetWidth(LineHeight, LineHeight);

	}
	public void FirstNodeAt(RaycastHit rayHit)
	{
		point1 = Instantiate(springPoint, rayHit.point, Quaternion.identity) as GameObject;
		point1.transform.parent = rayHit.transform;
	}

    public void FirstNodeAt(Transform trans)
    {
        point1 = Instantiate(springPoint, trans.position, Quaternion.identity) as GameObject;
        point1.transform.parent = trans;
    }

	public void SecondNodeAt(RaycastHit rayHit)
	{
		point2 = Instantiate(springPoint, rayHit.point, Quaternion.identity) as GameObject;
		point2.transform.parent = rayHit.transform;
	}

    public void SecondNodeAt(Transform trans)
    {
        point2 = Instantiate(springPoint, trans.position, Quaternion.identity) as GameObject;
        point2.transform.parent = trans;
    }

    public void CheckIfCut()
    {
		if(point1 == null || point2 == null) return;
        Vector3 difference = point1.transform.position - point2.transform.position;
		Debug.DrawLine(point2.transform.position,point2.transform.position + difference.normalized * difference.magnitude,Color.black,1.0f);
        if(Physics.Raycast(point2.transform.position,difference.normalized,difference.magnitude,cutMask))
        {
            Destroy(gameObject);
        }
    }
	void ConnectLine()
	{
		line.enabled = true;
		line.SetPosition(0,point1.transform.position);
		line.SetPosition(1,point2.transform.position);	
	}
}
