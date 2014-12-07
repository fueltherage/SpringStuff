using UnityEngine;
using System.Collections;

public class WeaponSlot : MonoBehaviour {

	// Use this for initialization
    public Ability AbilityTemplate;
    public GameObject AbilityObj;
    public Ability ability;

	void Start () 
    {
        GameObject temp = Instantiate(AbilityTemplate.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
        ability = temp.GetComponent<Ability>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetMouseButtonDown(0))
        {
            ability.Use();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ability.UseEnd();
        }
	}
}
