using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;

public class KolobokBehaviour : MonoBehaviour {
	public GameObject[] spheres;
	GameObject center;
	int num = 20;
	float density = 1000;
	
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

		float delta = Mathf.PI * 2 / num;
		float angle = 0;
		float R = 1;
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


			DistanceJoint2D distanceJoint = g.AddComponent<DistanceJoint2D>();
			distanceJoint.maxDistanceOnly = true;
			distanceJoint.connectedBody = cBody;
			distanceJoint.distance = R;

			Spring s = g.AddComponent<Spring>();
			s.connectedBody = cBody;
			s.k = density;
			s.distance = R;		
			//s.affectConnected = false;

			CircleCollider2D cc = g.AddComponent<CircleCollider2D>();
			cc.sharedMaterial = Resources.Load<PhysicsMaterial2D>("Materials/KolobokMat");

			spheres[n] = g;
		
			angle += delta;
		}

		//Сохраняем число вершин меша и сами вершины
		var storage = meshHolder.AddComponent<DataStorage> ();
		storage.AddEntry ("count", num);
		storage.AddEntry ("spheres", spheres);

		float h = Mathf.Sqrt(2 * R * R * (1 - Mathf.Cos (delta)))*0.99f;
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


			var connection = spheres[i].AddComponent<Spring>();
			connection.connectedBody = spheres[(i - 1 + num) % num]
										.GetComponent<Rigidbody2D>();
			connection.distance = h;	
			connection.k = density;


			DistanceJoint2D distanceJoint = spheres[i].AddComponent<DistanceJoint2D>();
			distanceJoint.connectedBody = spheres[(i - 1 + num) % num]
											.GetComponent<Rigidbody2D>();
			distanceJoint.distance = h;	
			distanceJoint.maxDistanceOnly = true;


			angle += delta;
		}

		mesh.vertices = verticies;
		mesh.triangles = triangles;
		mesh.uv = uv;

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();
	}
}
