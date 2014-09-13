using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;

public class KolobokBehaviour : MonoBehaviour {
	GameObject center;
	GameObject[] spheres;
		
	DistanceJoint2D[] radialJoints;
	DistanceJoint2D[] shellJoints;
	Spring[] radialSprings;	
	Spring[] shellSprings;
	int num = 20;
	float density = 1000;
	
	const float minRadius = 0.5f;
	const float maxRadius = 2f;
	const float deltaR = 0.03f;
	float Radius = 1;
	public float R
	{
		set{ 
			if(value < minRadius || value > maxRadius) return;
			Radius = value;
			float h = calculateShellDistance();
			for(int i = 0; i < num; i++) {
				radialJoints[i].distance = Radius;
				radialSprings[i].distance = Radius;
				shellJoints[i].distance = h;
				shellSprings[i].distance = h;
			}			
		}
		get{return Radius;}
	}
	
	
	void Start () {			
		center = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		center.transform.parent = this.transform;
		center.transform.localPosition = new Vector3 ();
		center.transform.localScale = new Vector3 (0.2f, 0.2f, 1);
		center.name = "KOLOBOK_CENTER";
		
		Component.DestroyImmediate (center.GetComponent<SphereCollider> ());
		Component.DestroyImmediate (center.GetComponent<MeshRenderer> ());

		Rigidbody2D cBody = center.AddComponent<Rigidbody2D> ();
		cBody.mass = 1f;
		
		GameObject.Find("Main Camera").GetComponent<TrackingCam>().target = center.transform;

		var meshHolder = new GameObject ();
		meshHolder.name = "KOLOBOK_MESH";	
		meshHolder.transform.parent = center.transform;
		meshHolder.transform.localPosition = new Vector3 ();
		meshHolder.AddComponent<KolobokMeshBehaviour>();

		var meshRenderer = meshHolder.AddComponent<MeshRenderer> ();
		meshRenderer.material = Resources.Load<Material> ("Materials/testo");
		
		var mf = meshHolder.AddComponent<MeshFilter> ();
		var mesh = mf.sharedMesh;
		if (mesh == null) {
			mf.sharedMesh = new Mesh();
			mesh = mf.sharedMesh;
		}
		mesh.Clear ();

		int[] triangles     = new int[3 * num];
		Vector3[] verticies = new Vector3[3 * num];
		Vector2[] uv        = new Vector2[3 * num];
		verticies [0] = new Vector3 ();
		spheres = new GameObject[num];
		radialJoints = new DistanceJoint2D[num];
		radialSprings = new Spring[num];

		float delta = Mathf.PI * 2 / num;
		float angle = 0;
		
		for(int n = 0; n < num; n++) {
			GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			g.transform.position = new Vector3(R * Mathf.Cos(angle), R * Mathf.Sin(angle), 0)
											+ center.transform.position;
			g.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
			g.transform.parent = this.transform;
			Component.DestroyImmediate (g.GetComponent<MeshRenderer>());
			Component.DestroyImmediate (g.GetComponent<SphereCollider>());

			//сохраним номер шарика чтобы обновлять меш потом
			g.name = "KOLOBOK_" + n.ToString();

			//Creating Mesh
			verticies[3 * n + 1] = new Vector3();
			verticies[3 * n + 2] = new Vector3(R * Mathf.Cos(angle), R * Mathf.Sin(angle), 0);
			verticies[(3 * n + 3)%(3 * num)] = new Vector3(R * Mathf.Cos(angle), R * Mathf.Sin(angle), 0);

			uv[3 * n + 1] = new Vector2(0.5f, 0.5f);
			uv[3 * n + 2] = new Vector2(0.5f + Mathf.Cos(angle) / 2, 0.5f + Mathf.Sin(angle) / 2);
			uv[(3 * n + 3)%(3 * num)] = new Vector2(0.5f + Mathf.Cos(angle) / 2, 0.5f + Mathf.Sin(angle) / 2);		

			Rigidbody2D body = g.AddComponent<Rigidbody2D>();
			body.mass = 1f;				

			radialJoints[n] = g.AddComponent<DistanceJoint2D>();
			radialJoints[n].maxDistanceOnly = true;
			radialJoints[n].connectedBody = cBody;
			radialJoints[n].distance = R;
			
			radialSprings[n] = g.AddComponent<Spring>();
			radialSprings[n].connectedBody = cBody;
			radialSprings[n].k = density;
			radialSprings[n].distance = R;		
			
			CircleCollider2D cc = g.AddComponent<CircleCollider2D>();
			cc.sharedMaterial = Resources.Load<PhysicsMaterial2D>("Materials/KolobokMat");

			spheres[n] = g;
		
			angle += delta;
		}

		//Сохраняем число вершин меша и сами вершины
		var storage = meshHolder.AddComponent<DataStorage> ();
		storage.AddEntry ("count", num);
		storage.AddEntry ("spheres", spheres);


		shellJoints = new DistanceJoint2D[num];
		shellSprings = new Spring[num];

		float h = calculateShellDistance();
		angle = 0;
		for (int i = 0; i < num; i++) {
			triangles[3 * i + 0] = 3 * i; // center
			triangles[3 * i + 1] = 3 * i + 1;
			triangles[3 * i + 2] = 3 * i + 2;

			GameObject dummy = new GameObject();
			dummy.transform.parent = spheres[i].transform;

			dummy.transform.localPosition = new Vector3();
			var dBody = dummy.AddComponent<Rigidbody2D>();
			dBody.mass = 0;
		

			var dHingeJoint = dummy.AddComponent<HingeJoint2D>();
			dHingeJoint.useLimits = true;
			dHingeJoint.connectedBody = spheres[i].GetComponent<Rigidbody2D>();

			
			shellSprings[i] = spheres[i].AddComponent<Spring>();
			shellSprings[i].connectedBody = spheres[(i - 1 + num) % num]
										.GetComponent<Rigidbody2D>();
			shellSprings[i].distance = h;	
			shellSprings[i].k = density;
			
			shellJoints[i] = spheres[i].AddComponent<DistanceJoint2D>();
			shellJoints[i].connectedBody = spheres[(i - 1 + num) % num]
											.GetComponent<Rigidbody2D>();
			shellJoints[i].distance = h;	
			shellJoints[i].maxDistanceOnly = true;


			angle += delta;
		}

		mesh.vertices = verticies;
		mesh.triangles = triangles;
		mesh.uv = uv;

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();
	}
	
	float calculateShellDistance () 
	{
		float delta = Mathf.PI * 2 / num;
		return Mathf.Sqrt(2 * R * R * (1 - Mathf.Cos (delta)))*0.99f;
	}
	
	void Update() {
		if(Input.GetKey(KeyCode.UpArrow)) {
			R = R + deltaR;
		}
		if(Input.GetKey(KeyCode.DownArrow)) {
			R = R - deltaR;
		}		
	}
}
