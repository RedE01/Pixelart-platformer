using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlaceTerrainObjects))]
public class PlaceTerrainObjectsEditor : Editor {

	PlaceTerrainObjects m_target;

	void OnSceneGUI() {

		m_target = (PlaceTerrainObjects)target;

		GUIStyle textStyle = new GUIStyle();
		textStyle.fontSize = 20;
		textStyle.normal.textColor = Color.white;
		textStyle.alignment = TextAnchor.MiddleCenter;

		Handles.Label(m_target.transform.position, "Hello", textStyle);
	}
}
