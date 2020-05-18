using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KGUI
{
	[AddComponentMenu("KGUI/Effects/Text Gradient")]
	public class KGUITextGradient : BaseMeshEffect
	{
		public Color _gradientTop = Color.white;
		public Color _gradientBottom = Color.white;
		[Range(-180f, 180f)]
		public float m_angle = 0f;

		public override void ModifyMesh(VertexHelper vh)
		{
			if(enabled)
			{
				Rect rect = graphic.rectTransform.rect;
				Vector2 dir = KGUIGradientUtils.RotationDir(m_angle);
				KGUIGradientUtils.Matrix2x3 localPositionMatrix = KGUIGradientUtils.LocalPositionMatrix(new Rect(0f, 0f, 1f, 1f), dir);

				UIVertex vertex = default(UIVertex);
				for (int i = 0; i < vh.currentVertCount; i++) {

					vh.PopulateUIVertex (ref vertex, i);
					Vector2 position = KGUIGradientUtils.VerticePositions[i % 4];
					Vector2 localPosition = localPositionMatrix * position;
					vertex.color *= Color.Lerp(_gradientBottom, _gradientTop, localPosition.y);
					vh.SetUIVertex (vertex, i);
				}
			}
		}
	}
}

