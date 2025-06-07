using UnityEngine;
using UnityEditor;
using System.Collections;

//Reference - "Unity3D - 2D Array In Inspector" by Sumeet Khobare
//	Youtube Link - https://www.youtube.com/watch?v=uoHc-Lz9Lsc
//	Dropbox Link - https://www.dropbox.com/scl/fi/ayg7mbir2s445arlyf2ua/2DArray-In-Inspector-Scripts.zip?dl=0&e=1&rlkey=2olpj5a5131ujryls9pmykk5l
#if (UNITY_EDITOR) 
[CustomPropertyDrawer(typeof(HashiwokakeroLayout))]
public class CustPropertyDrawer : PropertyDrawer {

	public override void OnGUI(Rect position,SerializedProperty property,GUIContent label){
		EditorGUI.PrefixLabel(position,label);
		Rect newposition = position;
		newposition.y += 18f;
		SerializedProperty data = property.FindPropertyRelative("rows");
		for(int j=0;j<7;j++){
			SerializedProperty row = data.GetArrayElementAtIndex(j).FindPropertyRelative("row");
			newposition.height = 18f;
			if(row.arraySize != 7)
				row.arraySize = 7;
			newposition.width = position.width/14;
			for(int i=0;i<7;i++){
				EditorGUI.PropertyField(newposition,row.GetArrayElementAtIndex(i),GUIContent.none);
				newposition.x += newposition.width;
			}


			newposition.x = position.x;
			newposition.y += 18f;
		}
	}


	public override float GetPropertyHeight(SerializedProperty property,GUIContent label){
		return 18f * 8;
	}
}
#endif