using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PathCreation
{
	[CustomEditor(typeof(PathGenerator))]
	public class PathGeneratorEditor : Editor
	{
		private PathGenerator m_target;
		private void OnEnable()
		{
			m_target = (PathGenerator)target;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("Generate"))
			{
				m_target.Generate();
			}


		}

	}
}
