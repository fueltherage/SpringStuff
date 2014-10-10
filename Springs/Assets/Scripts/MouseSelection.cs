using UnityEngine;
using System.Collections;

public class MouseSelection : MonoBehaviour {

	public GameObject SpringGO;
	public string ObjectTag = "Entity";
	bool firstShot = true;
	GameObject SpringTemp;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rayHit;
			if(Physics.Raycast(ray,out rayHit, 10.0f ))
			{
				if(rayHit.transform.tag == ObjectTag)
				{
					if(firstShot)
					{
						SpringTemp = Instantiate(SpringGO, rayHit.point, Quaternion.identity) as GameObject;
						SpringTemp.GetComponent<Spring>().FirstNodeAt(rayHit);
						firstShot = false;
					}else 
					{
						SpringTemp.GetComponent<Spring>().SecondNodeAt(rayHit);
						SpringTemp.GetComponent<Spring>().active=true;
						firstShot = true;
					}
				}
			}
		}
	}
}
