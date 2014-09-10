using UnityEngine;
using System.Collections;

public class KolobokMeshBehaviour : MonoBehaviour
{
	MeshFilter kolobokMesh;
	GameObject kolobokCenter;
	DataStorage data;

	bool ready = false;
	GameObject[] shellPoints;

	void initialize() {
		kolobokMesh = GameObject.Find ("KOLOBOK_MESH").GetComponent<MeshFilter> ();
		kolobokCenter = GameObject.Find ("KOLOBOK_CENTER");
		data = kolobokMesh.GetComponent<DataStorage> ();


		shellPoints = (GameObject[]) data.GetEntry("spheres");

		ready = true;
	}

	void LateUpdate() {
		if (!ready) initialize ();

		updateMesh ();
	}

	void updateMesh () {

		Mesh mesh = kolobokMesh.sharedMesh;
		
		Vector3[] verticies = new Vector3[mesh.vertices.Length];


		verticies [0] = new Vector3();
		int n = shellPoints.Length;
		for(int i = 0; i < n; i++) {
			verticies [3 * i + 2] = shellPoints[i].transform.localPosition - kolobokCenter.transform.localPosition;
			verticies [(3 * i + 3)%(3 * n)] = shellPoints[i].transform.localPosition - kolobokCenter.transform.localPosition;
		}
		mesh.vertices = verticies;

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		
		kolobokMesh.sharedMesh = mesh;
	}
}
