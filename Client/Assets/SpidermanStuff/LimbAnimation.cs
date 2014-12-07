using UnityEngine;
using System.Collections;

public class LimbAnimation : MonoBehaviour {

	// Use this for initialization
    public IkLimb LeftLeg;
    public IkLimb RightLeg;
    public float MaxHipHeight = 1;
    public float StrideDistance = 2;
    public float Kneeheight = 2;
    public float StepsPerSecond = 1; // step is defined by Mathf.PI / 90 degrees
    public bool Animating = true;
    public Controller3rdperson RigidReference;
    float Degrees = 0;
    Vector3 DegreeDisplacement1 = Vector3.zero;
    Vector3 DegreeDisplacement2 = Vector3.zero;
    Vector3 leftLocalStart;
    Vector3 rightLocalStart;
    Vector3 leftMod;
    Vector3 rightMod;


    float MaxStrideDistance;
    float MaxStepsPerSecond;
    float MaxKneeHeight;

	void Start () 
    {
        MaxStrideDistance = StrideDistance;
        MaxStepsPerSecond = StepsPerSecond;
        MaxKneeHeight = Kneeheight;

        leftLocalStart = LeftLeg.Controller.localPosition;
        rightLocalStart = RightLeg.Controller.localPosition;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Animating)
        {
            Degrees += Mathf.PI * StepsPerSecond * Time.deltaTime;
            DegreeDisplacement1.x = -Mathf.Sin(Degrees + Mathf.PI / 2);
            DegreeDisplacement1.y = Mathf.Sin(Degrees);
            DegreeDisplacement2.x = -Mathf.Sin(Degrees + Mathf.PI + Mathf.PI / 2);
            DegreeDisplacement2.y = Mathf.Sin(Degrees + Mathf.PI);
        }

        else
        {
            DegreeDisplacement1 = iTween.Vector3Update(DegreeDisplacement1, Vector3.zero, 1f);
            DegreeDisplacement2 = iTween.Vector3Update(DegreeDisplacement2, Vector3.zero, 1f);
        }
        if (RigidReference != null)
        {
            float SpeedMod = RigidReference.rigidbody.velocity.magnitude / RigidReference.MaxRunSpeed;
            if (SpeedMod > 1)
            {
                SpeedMod = 1;
            }
            StrideDistance = SpeedMod* MaxStrideDistance;
            StepsPerSecond = (SpeedMod + 0.01f) *MaxStepsPerSecond;
            Kneeheight = SpeedMod * MaxKneeHeight;
            leftMod.y = SpeedMod * MaxHipHeight;
            rightMod.y = SpeedMod * MaxHipHeight;
            if (SpeedMod < 0.00001f)
            {
                Animating = false;
            }
            else
            {
                Animating = true;
            }
        }
        

        Vector3 tempVec1 = new Vector3(DegreeDisplacement1.x * StrideDistance,leftLocalStart.y + leftMod.y + DegreeDisplacement1.y * Kneeheight);
        Vector3 tempVec2 = new Vector3(DegreeDisplacement2.x * StrideDistance,rightLocalStart.y + rightMod.y + DegreeDisplacement2.y * Kneeheight);

        LeftLeg.Controller.localPosition = tempVec1;
        RightLeg.Controller.localPosition = tempVec2;
	}

    public static float Sin(float x)
    {
        if (x < -3.14159265f)
            x += 6.28318531f;
        else
            if (x > 3.14159265f)
                x -= 6.28318531f;

        if (x < 0)
            return x * (1.27323954f + 0.405284735f * x);
        else
            return x * (1.27323954f - 0.405284735f * x);
    }

}
