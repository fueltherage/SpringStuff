using UnityEngine;
using System.Collections;

public class HandleSwinging : MonoBehaviour {
	
	public bool Attached = false;
	public ArrayList MassList = new ArrayList();
	public ArrayList SpringList = new ArrayList();
	public Rigidbody MassTemplate;
	public int numberOfSprings = 4;
	public Rigidbody Player;
	public Transform refPoint;
	public Transform bulletPoint;
	bool isFiring = false;
	public float ForceMultiplier = 2000f;
	public bool debug = true;
	public bool moveTowards = false;
	public float SpringForce = 50f;
	public float Damping = 1f;
	// Use this for initialization
	void Start () {
		
		gameObject.transform.position = bulletPoint.position;
		gameObject.rigidbody.isKinematic = true;
		Physics.IgnoreCollision(gameObject.collider,Player.collider);
		Physics.gravity = new Vector3(0,-10,0);
		//Physics.IgnoreCollision(Player.collider,MassTemplate.collider);
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetMouseButtonDown(1) && !isFiring){
			gameObject.rigidbody.isKinematic = false;
			Vector3 direction = Vector3.Normalize(bulletPoint.position - refPoint.position);
			isFiring = true;
			//gameObject.rigidbody.WakeUp();
			gameObject.rigidbody.AddForce(ForceMultiplier * direction);
			
		}
		
		if(Input.GetMouseButtonUp(1) && isFiring){
			
			DetachSprings();
			
			gameObject.rigidbody.isKinematic = true;
			
			Attached = false;
			isFiring = false;
			moveTowards = false;
			
			ResetSpike();
			
			
		}
		
		
		if(!Attached && !isFiring){
			ResetSpike();	
		}
		
		if(debug){
			debugGame();	
		}
		
		
		
	}
	
	void FixedUpdate(){
		if(Attached){
			
			for(int i = 0;i<SpringList.Count;i++){
				
			
				((Spring)(SpringList[i])).Update(Time.deltaTime);
				
			}
			
			
		}
	}
	void OnCollisionEnter(Collision c)
	{
		if(c.collider.tag == "Attachable"){
			
			Attached = true;
			AttachSprings();
			gameObject.rigidbody.isKinematic = true;
			
			
		}
		
	}
	
	void debugGame(){
		
		if(Attached){
		for(int i = 0;i<MassList.Count-1;i++){
			//Debug.DrawLine(((Rigidbody)(MassList[i])).transform.position,((Rigidbody)(MassList[i+1])).transform.position);	
		}
		}
	}
	void AttachSprings(){
		
		if(numberOfSprings == 1){
			Vector3 Direction = Vector3.Normalize(gameObject.transform.position - Player.transform.position);
			float DistanceBetweenPoints = (float)Vector3.Magnitude(gameObject.transform.position - Player.transform.position) / 3f;
			
			MassList.Add(Instantiate(MassTemplate,gameObject.transform.position - (Direction *  DistanceBetweenPoints),gameObject.transform.rotation));
			MassList.Add(Instantiate(MassTemplate,((Rigidbody)(MassList[0])).position - (Direction *  DistanceBetweenPoints),gameObject.transform.rotation));
			
			SpringList.Add(new Spring(gameObject.rigidbody,((Rigidbody)(MassList[0])),SpringForce,Damping,DistanceBetweenPoints));
			SpringList.Add(new Spring(((Rigidbody)(MassList[0])),((Rigidbody)(MassList[1])),SpringForce,Damping,DistanceBetweenPoints));
			SpringList.Add(new Spring(((Rigidbody)(MassList[1])),Player.rigidbody,SpringForce,Damping,DistanceBetweenPoints));
			//SpringList.Add(new Spring(Player,gameObject.rigidbody,10,2f,20));
			
			print("yes");
			
		}
		else{
		
		Vector3 Direction = Vector3.Normalize(gameObject.transform.position - Player.transform.position);
		float DistanceBetweenPoints = Vector3.Magnitude(gameObject.transform.position - Player.transform.position) / numberOfSprings;
		
		for(int i = 0;i<numberOfSprings;i++){
			
			MassList.Add(Instantiate(MassTemplate,gameObject.transform.position - (Direction * i * DistanceBetweenPoints),gameObject.transform.rotation));
		}
		
		SpringList.Add(new Spring((Rigidbody)MassList[MassList.Count - 1],Player.rigidbody,SpringForce,Damping,DistanceBetweenPoints-1));
		
		for(int i = 0;i<MassList.Count-1;i++){
			
			SpringList.Add(new Spring((Rigidbody)MassList[i],(Rigidbody)MassList[i+1],SpringForce,Damping,DistanceBetweenPoints-1));
			
			}
		
			SpringList.Add(new Spring(gameObject.rigidbody,(Rigidbody)MassList[0],SpringForce,Damping,DistanceBetweenPoints-1));
	}
	}
	void DetachSprings()
	{
		
		for(int i = 0;i<MassList.Count ; i++){
			Object temp = (Object)MassList[i];
			MassList.RemoveAt(i);
			DestroyObject((temp));
			
		}
		MassList.Clear();
		SpringList.Clear();
		
	}
	
	void ResetSpike(){
		
		//gameObject.rigidbody.Sleep();
		gameObject.rigidbody.isKinematic = true;
		gameObject.rigidbody.velocity = Vector3.zero;
		gameObject.transform.position = bulletPoint.position;
			
	}
	
}
