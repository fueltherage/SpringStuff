using UnityEngine;
using System.Collections;

public class IkLimb : MonoBehaviour {

	// Use this for initialization
    public Transform HipJoint;
    public Transform UpperLeg;
    public Transform KneeJoint;
    public Transform LowerLeg;
    public Transform Foot;
    public Transform Controller;
    public bool FlipElbow = false;

    private Transform HipJointTransform;
    private Transform KneeJointTransform;

    private float KneeAngle;
    private float HipVerticalAngle;
    private float HipHorizontalAngle;



	void Start () 
    {
        HipJointTransform = HipJoint;
        KneeJointTransform = KneeJoint;
	}
	
	// Update is called once per frame
	void Update () 
    {
        CalculateKneeAngle();
        AdjustLimb();
	}

    void AdjustLimb()
    {
        //KneeJoint = KneeJointTransform;
        //HipJoint = HipJointTransform;

        //Currently trying to get the rotation of the knees and hips to be relative to the current pointing direction of the hips.
        //This way i could rotate the hips without having to worry about the axis that hipjoint and legs rotate on for their IK
        //However, it is possible that the axis i need to realign is simply not being included in the IK.
        //the first step that you should take is to now Incorporate the Y angle that the leg is needing to be displaced to accomodate the IK aswell
        //BTW, Make sure to add constraints so the legs don't go insane and try to achieve angles that don't seem possible.

        
        Quaternion hipHorizontalQuat = Quaternion.AngleAxis((HipHorizontalAngle) * Mathf.Rad2Deg, Vector3.forward);
        //Quaternion hipVerticalQuat = Quaternion.AngleAxis(HipVerticalAngle * Mathf.Rad2Deg, Vector3.up);
        HipJoint.transform.localRotation = hipHorizontalQuat;
        KneeJoint.transform.localRotation = Quaternion.AngleAxis((KneeAngle + Mathf.PI / 2) * Mathf.Rad2Deg, Vector3.forward) * Quaternion.Inverse(hipHorizontalQuat);

        /*
        Vector3 rotationAxis = Controller.position - HipJoint.position;
        rotationAxis.Normalize();
        
        HipJoint.transform.rotation = Quaternion.LookRotation(rotationAxis, Vector3.up);
        HipJoint.transform.rotation = HipJoint.transform.rotation * Quaternion.AngleAxis((HipHorizontalAngle)* Mathf.Rad2Deg, HipJoint.right);
        KneeJoint.transform.rotation = Quaternion.LookRotation((Controller.position - KneeJoint.position).normalized);
        */
    }

    void CalculateKneeAngle()
    {
        /*           A
         *    /---------------.
         * B /         .
         *  /    .   C
         * /.         
         * 
         */
        Vector3 A = Foot.position - KneeJoint.position;
        Vector3 B = KneeJoint.position - HipJoint.position;
        Vector3 C = Controller.position - HipJoint.position;

        Vector3 HipToController = Controller.localPosition;
        Vector3 KneeToController = Controller.localPosition - KneeJoint.localPosition;
        bool local = true;
        if (local)
        {
            Transform oldParentFoot = Foot.parent;
            Transform oldParentKnee = KneeJoint.parent;
            Transform oldParentController = Controller.parent;

            Foot.parent = transform;
            KneeJoint.parent = transform;
            Controller.parent = transform;

            A = Foot.localPosition - KneeJoint.localPosition;
            B = KneeJoint.localPosition - HipJoint.localPosition;
            C = Controller.localPosition - HipJoint.localPosition;
            KneeToController = Controller.localPosition - KneeJoint.localPosition;

            A.z = 0;
            B.z = 0;
            C.z = 0;
            KneeToController.z = 0;

            Foot.parent = oldParentFoot;
            KneeJoint.parent = oldParentKnee;
            Controller.parent = oldParentController;
        }

        HipToController = C;
        KneeAngle = Mathf.Atan2(KneeToController.y, KneeToController.x);

        float elbowSign = (FlipElbow) ? -1 : 1;
        float CosFunc = C.sqrMagnitude / ((B.sqrMagnitude + A.sqrMagnitude)*2);
        if (CosFunc > 1)
        {
            CosFunc = 1;
        }
        CosFunc -= 1;
        HipHorizontalAngle = elbowSign * Mathf.Acos(CosFunc) + Mathf.Atan2(HipToController.y, HipToController.x);
        HipVerticalAngle = Mathf.Atan2(C.x, C.z);
    }

}
