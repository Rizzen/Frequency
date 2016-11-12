using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoWCovering : MonoBehaviour {

	[Range(0,25)]
	public float viewRadius;
	[Range(0,363)]
	public float viewAngle;
	[Range(-180,180)]
	public float zeroAngle; // для вращения зоны покрытия
	public float meshResolution; //количество линий на градус
	public LayerMask obstacleMask;
	public MeshFilter viewMeshFilter;
	public int edgeResolveIterations;
	public float edgeDstThreshold; // если расстояние между двумя точками превышает этот порог, то запускаем FindEdge снова
	Mesh viewMesh;


	void Start()
	{
		viewMesh = new Mesh ();
		viewMesh.name = "View Mesh";
		viewMeshFilter.mesh = viewMesh;
	}


	#region Covering
	ViewCastInfo ViewCast (float globalAngle)
	{
		Vector3 dir = DirFromAngle (globalAngle, true);
		RaycastHit hit;

		if (Physics.Raycast (transform.position, dir, out hit, viewRadius, obstacleMask)) {
			return new ViewCastInfo (true, hit.point, hit.distance, globalAngle);
		} 
		else {
			return new ViewCastInfo (false, transform.position + dir*viewRadius, viewRadius, globalAngle);
		}
	}

	public Vector3 DirFromAngle (float angleInDegrees, bool angleIsGlobal)  // из угла находим нормализованное направление (синус и косинус изменяются от -1 до 1)
	{
		if (!angleIsGlobal) 
		{
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3 (Mathf.Sin(angleInDegrees*Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees*Mathf.Deg2Rad));
	}


	public void DrawFieldOfView (float _viewAngle, float _zeroAngle)
	{
		int stepCount = Mathf.RoundToInt(_viewAngle * meshResolution);
		float stepAngleSize = viewAngle / stepCount;
		List<Vector3> viewPoints = new List<Vector3> ();
		ViewCastInfo oldViewCast = new ViewCastInfo ();

		for (int i = 0; i < stepCount; i++) {
			float angle = transform.eulerAngles.y + _zeroAngle - _viewAngle / 2 + stepAngleSize * i;
			ViewCastInfo newViewCast = ViewCast (angle);

			if (i > 0)  //в первой итерации oldViewCast пустой
			{
				bool edgeDstThresholdExceeded = Mathf.Abs (oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
				if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded)) {
					EdgeInfo edge = FindEdge (oldViewCast, newViewCast);

					if (edge.pointA != Vector3.zero) {
						viewPoints.Add (edge.pointA);	
					}
					if (edge.pointB != Vector3.zero) {
						viewPoints.Add (edge.pointB);	
					}
				}
			}
			viewPoints.Add (newViewCast.point);
			oldViewCast = newViewCast;
		}

		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];

		int[] triangles = new int[(vertexCount - 2) * 3];
		vertices [0] = Vector3.zero;


		//расставляем вертиксы и заполняем трисы
		for (int i = 0; i < vertexCount - 1; i++)
		{
			vertices [i + 1] = transform.InverseTransformPoint (viewPoints [i]);

			if (i < vertexCount - 2) {
				triangles [i * 3] = 0;
				triangles [i * 3 + 1] = i + 1;
				triangles [i * 3 + 2] = i + 2;
			}
		}

		//рисуем меш

		viewMesh.Clear ();
		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals ();
	}



	public void UnDraw()
	{
		viewMesh.Clear ();
	}


	EdgeInfo FindEdge (ViewCastInfo minViewCast, ViewCastInfo maxViewCast) //для нахождения грани препятствия, в которую уперся трис (для малых меш резолюшенов)
	{
		float minAngle = minViewCast.angle;
		float maxAngle = maxViewCast.angle;
		Vector3 minPoint = Vector3.zero;
		Vector3 maxPoint = Vector3.zero;

		for (int i = 0; i < edgeResolveIterations; i++) {
			float angle = (minAngle + maxAngle) / 2;
			ViewCastInfo newViewCast = ViewCast (angle);

			bool edgeDstThresholdExceeded = Mathf.Abs (minViewCast.dst - newViewCast.dst) > edgeDstThreshold;

			if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded) {
				minAngle = angle;
				minPoint = newViewCast.point;
			} else {
				maxAngle = angle;
				maxPoint = newViewCast.point;
			}
		}
		return new EdgeInfo (minPoint, maxPoint);
	}

	void LateUpdate()
	{
		//DrawFieldOfView (viewAngle,zeroAngle);
	}



	public struct ViewCastInfo
	{
		public bool hit;
		public Vector3 point;
		public float dst;
		public float angle;

		public ViewCastInfo (bool _hit, Vector3 _point, float _dst, float _angle)
		{
			hit = _hit;
			point = _point;
			dst = _dst;
			angle = _angle;
		}
	}

	public struct EdgeInfo {
		public Vector3 pointA;
		public Vector3 pointB;

		public EdgeInfo(Vector3 _pointA, Vector3 _pointB) {
			pointA = _pointA;
			pointB = _pointB;
		}
	}

	#endregion

}
