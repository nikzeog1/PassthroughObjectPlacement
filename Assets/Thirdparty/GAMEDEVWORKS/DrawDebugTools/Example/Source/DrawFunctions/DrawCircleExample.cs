﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDTExamples
{
	public class DrawCircleExample : MonoBehaviour
	{
		private float m_GridSize = 4.0f;
		private Vector3 m_Position;

		void Update()
		{
			// Draw grid
			DrawDebugTools.DrawGrid(transform.position, m_GridSize, 1.0f, 0.0f);

			// Draw shape
			m_Position = transform.position + new Vector3(0.0f, 0.5f, 0.0f);
			DrawDebugTools.DrawCircle(m_Position, 1.0f, 12, Color.green, EDrawPlaneAxis.XZ);

			// Draw 3d label
			m_Position = transform.position + new Vector3(0.0f, 0.0f, -m_GridSize / 2.0f - 0.6f);
			DrawDebugTools.DrawString3D(m_Position, Quaternion.Euler(-90.0f, 180.0f, 0.0f), "CIRCLE", TextAnchor.LowerCenter, Color.white, 2.0f);
		}
	}
}