using UnityEngine;
using System.Collections;

public class Controller3rdperson : MonoBehaviour {

	// Use this for initialization
    public float MoveSpeed = 100;
    public float MaxRunSpeed = 10;
    public float MaxWalkSpeed = 3;
    public float CurrentMaxSpeed;
    public float turnSpeed = 0.05f;
    public float JumpImpulse = 3;
    public float DesiredTimescale = 1;
    public float SlowMotionScale = 0.6f;
    public float SlowMotionSpeed = 1;
    
    bool SlowMotion = false;
    bool Running = false;

	void Start () {
        CurrentMaxSpeed = MaxWalkSpeed;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel(1);
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //transform.rotation *= Quaternion.EulerAngles(0, horizontal * turnSpeed, 0);
        
        Vector2 transformedAxis = RotatePoint(new Vector2(horizontal, vertical), -Camera.main.transform.eulerAngles.y);

        rigidbody.AddForce(new Vector3(transformedAxis.x, 0, transformedAxis.y).normalized * MoveSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody.AddForce(transform.up * JumpImpulse, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            CurrentMaxSpeed = Mathf.Lerp(CurrentMaxSpeed,MaxRunSpeed,Time.deltaTime * turnSpeed);
            Running = true;

        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && Running)
        {
            CurrentMaxSpeed = Mathf.Lerp(CurrentMaxSpeed, MaxWalkSpeed, Time.deltaTime * turnSpeed);
            Running = false;
        }

        if (Running)
        {
            CurrentMaxSpeed = Mathf.Lerp(CurrentMaxSpeed, MaxRunSpeed, Time.deltaTime * turnSpeed);
        }
        else
        {
            CurrentMaxSpeed = Mathf.Lerp(CurrentMaxSpeed, MaxWalkSpeed, Time.deltaTime * turnSpeed);
        }

        if (Time.timeScale != DesiredTimescale)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, DesiredTimescale, Time.deltaTime * SlowMotionSpeed);
        }
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SlowMotion = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            SlowMotion = false;
        }
        if (SlowMotion)
        {
            DesiredTimescale = SlowMotionScale;
        }
        else
        {
            DesiredTimescale = 1;
        }

        Vector3 velocity = rigidbody.velocity;
        Vector3 velDirection = rigidbody.velocity.normalized;
        velocity.y = 0;

        if (velocity.sqrMagnitude > (CurrentMaxSpeed * CurrentMaxSpeed))
        {
            rigidbody.velocity = new Vector3(velDirection.x * CurrentMaxSpeed, rigidbody.velocity.y, velDirection.z * CurrentMaxSpeed);
        }
	}
    Quaternion newRotation;
    void Update()
    {
        Vector3 velNormalized = rigidbody.velocity.normalized;
        if (rigidbody.velocity.sqrMagnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(velNormalized.x, 0, velNormalized.z), Vector3.up) * Quaternion.Euler(0,270,0), Time.deltaTime * turnSpeed);
            newRotation = transform.rotation;
        }
        else
        {
            //Rotate towards camera facing direction
            //transform.rotation = Quaternion.Slerp(transform.rotation, Camera.main.transform.rotation * Quaternion.Euler(0, 270, 0), Time.deltaTime * turnSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation,Time.deltaTime * turnSpeed);
        }
    }

    public Vector2 RotatePoint(Vector2 point, float angle)
    {
        Vector2 result = Vector2.zero;

        float SinAngle = Mathf.Sin(Mathf.Deg2Rad * angle);
        float CosAngle = Mathf.Cos(Mathf.Deg2Rad * angle);

        result.x = CosAngle * point.x - point.y * SinAngle;
        result.y = SinAngle * point.x + point.y * CosAngle;

        return result;
    }
}
