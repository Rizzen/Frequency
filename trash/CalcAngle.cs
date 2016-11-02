using UnityEngine;
using System.Collections;

public class CalcAngle : MonoBehaviour {

	public static float calcAngleBetween (Vector3 firstOrigin, Vector3 firstEnd, Vector3 secondOrigin, Vector3 secondEnd)
	{
		//Косинус угла между векторами равен скалярному произведению векторов, поделенному на произведение модулей векторов.
		//Сначала находим скалярное произведение
		Vector3 firstCoordinates = new Vector3 (firstEnd.x - firstOrigin.x, firstEnd.y - firstOrigin.y, firstEnd.z - firstOrigin.z); 
		//print (firstCoordinates);
		Vector3 secondCoordinates = new Vector3 (secondEnd.x - secondOrigin.x, secondEnd.y - secondOrigin.y, secondEnd.z - secondOrigin.z); 
		//print (secondCoordinates);
		float scalar = (firstCoordinates.x * secondCoordinates.x) + (firstCoordinates.y * secondCoordinates.y) + (firstCoordinates.z * secondCoordinates.z);
		//print (scalar);
		float modA = Mathf.Sqrt (Mathf.Pow (firstCoordinates.x, 2) + Mathf.Pow (firstCoordinates.y, 2) + Mathf.Pow (firstCoordinates.z, 2));
		//print (modA);
		float modB = Mathf.Sqrt (Mathf.Pow (secondCoordinates.x, 2) + Mathf.Pow (secondCoordinates.y, 2) + Mathf.Pow (secondCoordinates.z, 2));
		//print (modB);
		float cosA = scalar/(modA+modB);
		//print (cosA);
		float AcosA = Mathf.Acos (cosA);
		//print (AcosA);
		//print (AcosA * Mathf.Rad2Deg);
		return AcosA*Mathf.Rad2Deg;
	}

	public static float AngleSighned (Vector3 vector1, Vector3 vector2, Vector3 normal)
	{
		return Mathf.Atan2 (Vector3.Dot (normal, Vector3.Cross (vector1, vector2)), Vector3.Dot (vector1, vector2)) * Mathf.Rad2Deg;
	}

	public static float angle360 (Vector3 from, Vector3 to, Vector3 right)

	{
		float angle = Vector3.Angle (from, to);
		return (Vector3.Angle (right, to) > 90f) ? 360f - angle : angle;
	}



}
