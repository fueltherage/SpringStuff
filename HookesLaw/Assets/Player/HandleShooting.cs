using UnityEngine;
using System.Collections;

public class HandleShooting : MonoBehaviour {
	
	public Rigidbody[] Bullets;
	public int CurrentlyActiveBullet;
	public Transform BulletPoint;
	public float ShootSpeed = 0.3f;
	public float elapsedTime = 0f;
	public Transform DirectionReference;
	public float BulletStrength = 2000;
	public Rigidbody Spike;
	
	// Use this for initialization
	void Start () {
		
		CurrentlyActiveBullet = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		elapsedTime += Time.deltaTime;
		
		
		//Spike.GetComponent<HandleSwinging>().Attached
		if(Input.GetMouseButtonDown(0) && elapsedTime > ShootSpeed){
			
			Rigidbody temp = (Rigidbody)Instantiate(Bullets[CurrentlyActiveBullet],BulletPoint.transform.position,BulletPoint.transform.rotation);
			Vector3 tempDirection = Vector3.Normalize(BulletPoint.transform.position - DirectionReference.position);
			temp.AddForce(tempDirection * BulletStrength);
			
			elapsedTime = 0f;
		}
		if(Input.GetKeyDown(KeyCode.Q)){
			
			NextWeapon();
			
		}
		else if(Input.GetKeyDown(KeyCode.E)){
			
			PreviousWeapon();
		}
	
	}
	
	void NextWeapon(){
		
		if(CurrentlyActiveBullet < Bullets.Length - 1){
				CurrentlyActiveBullet++;
		}
		else{
			CurrentlyActiveBullet = 0;	
		}
		
	}
	
	void PreviousWeapon(){
		
		if(CurrentlyActiveBullet > 0){
				CurrentlyActiveBullet--;
		}
		else{
			CurrentlyActiveBullet = Bullets.Length-1;	
		}
		
	}
}
