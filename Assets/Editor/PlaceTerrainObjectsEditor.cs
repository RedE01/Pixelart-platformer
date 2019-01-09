using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(PlaceTerrainObjects))]
public class PlaceTerrainObjectsEditor : Editor {

	GameObject m_target;
	SerializedProperty objectPrefab;
	Camera mainCamera;
	GameObject createdGameObject;
	List<GameObject> prefabs = new List<GameObject>();
	int prefabSelected = 0;
	int depthVal = 0;

	void OnEnable() {
		objectPrefab = serializedObject.FindProperty("ObjectPrefabs");
		mainCamera = Camera.main;
	}

	void OnSceneGUI() {
		Handles.BeginGUI();

		Event e = Event.current;
		
		SerializedProperty iterator = serializedObject.FindProperty("ObjectPrefabs");
		iterator.NextVisible(true);
		for(int i = 0; iterator.NextVisible(true); i++) {
			int buttonX = 10 + 110 * (i % 2);
			int buttonY = 50 + 26 * (i / 2);
			if(GUI.Button(new Rect(buttonX, buttonY, 100, 25), iterator.objectReferenceValue.name)) {
				prefabSelected = i;
				InstantiateObject(e.mousePosition);
				createdGameObject.GetComponent<SpriteRenderer>().sortingOrder = depthVal;
			}
		}
		if(GUI.Button(new Rect(140, 15, 25, 25), "-")) {
			depthVal--;
		}
		GUI.Button(new Rect(165, 15, 25, 25), depthVal.ToString());
		if (GUI.Button(new Rect(190, 15, 25, 25), "+")) {
			depthVal++;
		}

		GUI.color = new Color(1, 1, 1, 0.2f);
		GUI.Button(new Rect(5, 5, 225, 300), "");

		Handles.EndGUI();
	}

	void InstantiateObject(Vector2 mousePos) {
		createdGameObject = (GameObject)Instantiate(getPrefab(), getMouseToWorldPos(mousePos), Quaternion.identity);
	}

	Vector3 getMouseToWorldPos(Vector2 mousePos) {
		Vector3 o = HandleUtility.GUIPointToWorldRay(mousePos).origin;
		o.z = 0;
		return o;
	}

	Object getPrefab() {
		return objectPrefab.GetArrayElementAtIndex(prefabSelected).objectReferenceValue;
	}
}
