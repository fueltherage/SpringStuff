using UnityEngine;
using System.Collections;

public class SpringSelf : SpringAbility {

    private GameObject SpringTemp;
    public override void Use()
    {
        base.Use();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, 10.0f,hitMask))
        {
            SpringTemp = Instantiate(SpringType.gameObject, rayHit.point, Quaternion.identity) as GameObject;
            Spring spring = SpringTemp.GetComponent<Spring>();
            spring.FirstNodeAt(parent.gameObject.transform);
            spring.SecondNodeAt(rayHit);
            spring.active = true;
            SpringShooter.ConnectedSprings.Add(spring);
        }
    }
}
