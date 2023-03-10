﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDTExamples
{
	public class DrawCapsuleExample : MonoBehaviour
	{
		private float m_GridSize = 4.0f;
		private Vector3 m_Position;

		void Update()
		{
			// Draw grid
			DrawDebugTools.DrawGrid(transform.position, m_GridSize, 1.0f, 0.0f);

			// Draw shape
			m_Position = transform.position + new Vector3(0.0f, 2.0f, 0.0f);
			DrawDebugTools.DrawCapsule(m_Position, 2.0f, 1.0f, Quaternion.identity, Color.green);

			// Draw 3d label
			m_Position = transform.position + new Vector3(0.0f, 0.0f, -m_GridSize / 2.0f - 0.6f);
			DrawDebugTools.DrawString3D(m_Position, Quaternion.Euler(-90.0f, 180.0f, 0.0f), "CAPSULE", TextAnchor.LowerCenter, Color.white, 2.0f);
		}
	}
}