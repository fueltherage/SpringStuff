using UnityEngine;
using System.Collections;

public class SpringSelect : SpringAbility {

	// Use this for initialization
    private bool firstShot = true;
    private GameObject SpringTemp;

    public override void Use()
    {
        base.Use();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, 10.0f,hitMask))
        {
            Spring lastSpring;
            if (firstShot)
            {
                SpringTemp = Instantiate(SpringType.gameObject, rayHit.point, Quaternion.identity) as GameObject;
                lastSpring = SpringTemp.GetComponent<Spring>();
                lastSpring.FirstNodeAt(rayHit);
                firstShot = false;
            }
            else
            {
                lastSpring = SpringTemp.GetComponent<Spring>();
                lastSpring.SecondNodeAt(rayHit);
                lastSpring.GetComponent<Spring>().active = true;
                SpringShooter.ConnectedSprings.Add(lastSpring);
                firstShot = true;
            }
        }
    }
}
