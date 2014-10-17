using System;
using UnityEngine;


	public class Spring
	{
		public Rigidbody mass1;
		public Rigidbody mass2;
		public float Tension;
		public float Damping;
		public float RestingDistance;
		
		public Spring (Rigidbody mass1, Rigidbody mass2, float Tension, float Damping, float RestingDistance)
		{
			this.mass1 = mass1;
			this.mass2 = mass2;
			this.Tension = Tension;
			this.Damping = Damping;
			this.RestingDistance = RestingDistance;
			
			
			
		}
		public void Update(float DeltaTime)
		{
			float Distance = Vector3.Distance(mass1.transform.position,mass2.transform.position);
			Vector3 Direction = Vector3.Normalize(mass2.transform.position - mass1.transform.position);
			Vector3 force =  -Tension * (RestingDistance - Distance) * Direction;
			Debug.DrawLine(mass1.position,mass1.position);
			force -= Damping * (mass1.velocity - mass2.velocity);
			
			/*print(Distance);
			print(RestingDistance);
			print("=========================");*/
			
			//mass2.AddForce( -Direction * (Distance - RestingDistance) * Tension);
            //mass2.AddForce(-Damping * mass2.velocity);
            //mass1.AddForce(Direction * (Distance - RestingDistance) * Tension);
            //mass1.AddForce(-Damping * mass1.velocity);
			mass1.AddForce(force);
			mass2.AddForce(-force);
			
		
			/*mass1.AddForce(-force - (mass1.velocity * Damping));
			mass2.AddForce(force - (mass2.velocity * Damping));*/
		
			
			
		}
	}


