using UnityEngine;
using System.Collections;

public class SpringSpray : SpringAbility 
{
    public float CoolDown = 0.3f;
    public float SprayRadius = 10f;

    GameObject SpringTemp;
    protected bool Firing = false;
    private float TimeElapsed = 0;
    public void Update()
    {
        TimeElapsed += Time.deltaTime;
        if (Firing && TimeElapsed >= CoolDown)
        {
            SpraySprings();
            TimeElapsed = 0;
        }
    }
    public override void Use()
    {
        base.Use();
        Firing = true;
    }

    public override void UseEnd()
    {
        base.UseEnd();
        Firing = false;
    }

    public void SpraySprings()
    {
        base.Use();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, 10.0f,hitMask))
        {
            Collider[] colliders = Physics.OverlapSphere(rayHit.point, SprayRadius, hitMask);
            for (int i = 0; i < colliders.Length; i++)
            {
                SpringTemp = Instantiate(SpringType.gameObject, rayHit.point, Quaternion.identity) as GameObject;
                Spring spring = SpringTemp.GetComponent<Spring>();
                spring.FirstNodeAt(rayHit);
                spring.SecondNodeAt(colliders[i].transform);
                spring.active = true;
                SpringShooter.ConnectedSprings.Add(spring);
            }
        }
    }
}
