using UnityEngine;
using System.Collections;

public class SpringQuad : MonoBehaviour {

	// Use this for initialization
    public GameObject SoftPoint;
    public Spring SpringType;
    public float Width = 1;
    public float Height = 1;

    GameObject springTemp;
	void Start () 
    {
        CreateQuad();
        //Debug.Break();
	}

    void CreateQuad()
    {
        float halfWidth = Width /2;
        float halfHeight = Height/2;
        GameObject topLeft = (GameObject)Instantiate(SoftPoint, transform.position + new Vector3(-halfWidth, -halfHeight, 0), transform.rotation);
        GameObject topRight = (GameObject)Instantiate(SoftPoint, transform.position + new Vector3(halfWidth, -halfHeight, 0), transform.rotation);
        GameObject bottomRight = (GameObject)Instantiate(SoftPoint, transform.position + new Vector3(halfWidth, halfHeight, 0), transform.rotation);
        GameObject bottomLeft = (GameObject)Instantiate(SoftPoint, transform.position + new Vector3(-halfWidth, halfHeight, 0), transform.rotation);

        Spring TLspring = (Spring)Instantiate(SpringType, topLeft.transform.position, topLeft.transform.rotation);
        Spring TRspring = (Spring)Instantiate(SpringType, topRight.transform.position, topRight.transform.rotation);
        Spring BRspring = (Spring)Instantiate(SpringType, bottomRight.transform.position, bottomRight.transform.rotation);
        Spring BLspring = (Spring)Instantiate(SpringType, bottomLeft.transform.position, bottomLeft.transform.rotation);
        Spring CrossSpring1 = (Spring)Instantiate(SpringType, topLeft.transform.position, topLeft.transform.rotation);
        Spring CrossSpring2 = (Spring)Instantiate(SpringType, topRight.transform.position, topRight.transform.rotation);
        
        TLspring.FirstNodeAt(topLeft.transform);
        TLspring.SecondNodeAt(topRight.transform);
        TLspring.active = true;
        TRspring.FirstNodeAt(topRight.transform);
        TRspring.SecondNodeAt(bottomRight.transform);
        TRspring.active = true;
        BRspring.FirstNodeAt(bottomRight.transform);
        BRspring.SecondNodeAt(bottomLeft.transform);
        BRspring.active = true;
        BLspring.FirstNodeAt(bottomLeft.transform);
        BLspring.SecondNodeAt(topLeft.transform);
        BLspring.active = true;
        CrossSpring1.FirstNodeAt(topLeft.transform);
        CrossSpring2.FirstNodeAt(topRight.transform);
        CrossSpring1.SecondNodeAt(bottomRight.transform);
        CrossSpring2.SecondNodeAt(bottomLeft.transform);
        CrossSpring1.active = true;
        CrossSpring2.active = true;
    }
}
