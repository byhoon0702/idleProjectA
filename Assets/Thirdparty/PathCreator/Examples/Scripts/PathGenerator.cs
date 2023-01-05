using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
namespace PathCreation
{
	[RequireComponent(typeof(PathCreator))]
	public class PathGenerator : MonoBehaviour
	{
		public PathCreator m_creator;
		public BezierPath.ControlMode m_control_mode;
		public PathSpace m_space;
		public RectTransform[] m_waypoints;
		public bool m_closed_loop;


		public void Generate()
		{
			if (m_waypoints == null)
			{
				return;
			}
			if (m_waypoints.Length == 0)
			{
				return;
			}
			BezierPath bezierPath = new BezierPath(m_waypoints, m_closed_loop, m_space);
			bezierPath.ControlPointMode = m_control_mode;
			m_creator.bezierPath = bezierPath;

		}



	}
}
