using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpringShooter : MonoBehaviour {

	// Use this for initialization
    public Transform AttachRef;
    public float Distance = 30;
    public float PullForce = 300;
    public float SpringForce = 200;
    public List<SpringAbility> Abilities = new List<SpringAbility>();
    public List<SpringAbility> ConnectedAbilities = new List<SpringAbility>();
    public static List<Spring> ConnectedSprings = new List<Spring>();
    public SpringAbility CurrentAbility;
	void Start () 
    {
        ConnectAbilities();
	}
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetButtonDown("Fire1"))
        {
            CurrentAbility.Use();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            CurrentAbility.UseEnd();
        }
	}

    void ConnectAbilities()
    {
        for (int i = 0; i < Abilities.Count; i++)
        {
            GameObject abilityObject = (GameObject)Instantiate(Abilities[i].gameObject);
            abilityObject.transform.parent = transform;
            abilityObject.transform.localPosition = Vector3.zero;
            SpringAbility ability = abilityObject.GetComponent<SpringAbility>();
            ConnectedAbilities.Add(ability);
            ability.parent = this;
        }
        CurrentAbility = ConnectedAbilities[1];
    }
    public void DetachAll()
    {
        for (int i = 0; i < ConnectedSprings.Count; i++)
        {
            Destroy(ConnectedSprings[i]);
        }
    }

}
