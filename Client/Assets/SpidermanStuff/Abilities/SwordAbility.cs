using UnityEngine;
using System.Collections;

public class SwordAbility : Ability 
{
    public GameObject SlashCollider;
    public float SlashColliderOffsetZ = 3;

    protected Vector3 StartV = Vector3.zero;

    public override void Start()
    {
        base.Start();
        SlashCollider = Instantiate(SlashCollider, transform.position, Quaternion.identity) as GameObject;
        //SlashCollider.gameObject.SetActive(false);
    }
    public override void Use()
    {
        base.Use();
        StartV = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,SlashColliderOffsetZ));
    }

    public override void UseEnd()
    {
        base.UseEnd();
        Vector3 End = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, SlashColliderOffsetZ));
        Debug.DrawLine(StartV, End, Color.red, 1.0f);
        Vector3 difference = End - StartV;
        SlashCollider.transform.position = Vector3.Lerp(End, StartV, 0.5f);
        SlashCollider.transform.localScale = new Vector3(SlashCollider.transform.localScale.x, SlashCollider.transform.localScale.y, difference.magnitude);
        SlashCollider.transform.rotation = Quaternion.LookRotation(difference.normalized,Vector3.up);
        SlashCollider.SetActive(true);
        for (int i = 0; i < SpringShooter.ConnectedSprings.Count; i++)
        {
            SpringShooter.ConnectedSprings[i].CheckIfCut();
        }
        //SlashCollider.SetActive(false);
    }
}
