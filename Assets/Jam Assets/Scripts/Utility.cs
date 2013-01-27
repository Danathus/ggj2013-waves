using UnityEngine;
using System.Collections;

public class Utility  {
	
	// create a quad in world space that fits the whole screen at any distance from the perspective camera
	static public Mesh CreateFullscreenPlane(float distanceFromCamera) {
		
		Mesh plane = new Mesh();
		Vector3[] verts = new Vector3[4];
		Color[] colors = new Color[4] { Color.white, Color.white, Color.white, Color.white };
		Vector2[] uvs = new Vector2[4];
		int[] tris = new int[6] { 0, 1, 2, 2, 1, 3 };
		
		Camera cam = Camera.main;
		
		Vector3 center = cam.transform.position + cam.transform.forward * distanceFromCamera;
		
		float halfHeight = distanceFromCamera * Mathf.Tan(Mathf.Deg2Rad * cam.fov * 0.5f);
		float halfWidth = halfHeight * cam.aspect;
		
		verts[0] = center - Vector3.right * halfWidth + Vector3.up * halfHeight;
		verts[1] = center + Vector3.right * halfWidth + Vector3.up * halfHeight;
		verts[2] = center - Vector3.right * halfWidth - Vector3.up * halfHeight;
		verts[3] = center + Vector3.right * halfWidth - Vector3.up * halfHeight;
		
		uvs[0] = new Vector2(0.0f, 1.0f);
		uvs[1] = new Vector2(1.0f, 1.0f);
		uvs[2] = new Vector2(0.0f, 0.0f);
		uvs[3] = new Vector2(1.0f, 0.0f);
		
		plane.vertices = verts;
		plane.colors = colors;
		plane.triangles = tris;	
		plane.uv = uvs;
		
		plane.RecalculateNormals();
		
		return plane;
	}
	
}
