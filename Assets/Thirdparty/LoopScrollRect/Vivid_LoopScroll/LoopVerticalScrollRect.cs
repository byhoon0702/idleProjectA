using UnityEngine;
using UnityEngine.UI;
using System.Collections;
#if !PROJECT_B
namespace VIVID
{
	[AddComponentMenu("UI/Loop Vertical Scroll Rect", 51)]
	[DisallowMultipleComponent]
	public class LoopVerticalScrollRect : LoopScrollRect
	{
		protected override float GetSize(RectTransform item)
		{
			float size = contentSpacing;
			if (m_grid_layout != null)
			{
				size += m_grid_layout.cellSize.y;
			}
			else
			{
				size += LayoutUtility.GetPreferredHeight(item);
			}
			return size;
		}

		protected override float GetDimension(Vector2 vector)
		{
			return vector.y;
		}

		protected override Vector2 GetVector(float value)
		{
			return new Vector2(0, value);
		}

		protected override void Awake()
		{
			base.Awake();
			directionSign = -1;

			GridLayoutGroup layout = content.GetComponent<GridLayoutGroup>();
			if (layout != null && layout.constraint != GridLayoutGroup.Constraint.FixedColumnCount)
			{
				Debug.LogError("[LoopHorizontalScrollRect] unsupported GridLayoutGroup constraint");
			}
		}

		protected override bool UpdateItems(Bounds viewBounds, Bounds contentBounds)
		{
			bool changed = false;

			// special case: handling move several page in one frame
			if (viewBounds.max.y < contentBounds.min.y && m_item_type_end > m_item_type_start)
			{
				int maxItemTypeStart = -1;
				if (totalCount >= 0)
				{
					maxItemTypeStart = Mathf.Max(0, totalCount - (m_item_type_end - m_item_type_start));
				}
				float currentSize = contentBounds.size.y;
				float elementSize = (currentSize - contentSpacing * (CurrentLines - 1)) / CurrentLines;
				ReturnToTempPool(true, m_item_type_end - m_item_type_start);
				m_item_type_start = m_item_type_end;

				int offsetCount = Mathf.FloorToInt((contentBounds.min.y - viewBounds.max.y) / (elementSize + contentSpacing));
				if (maxItemTypeStart >= 0 && m_item_type_start + offsetCount * contentConstraintCount > maxItemTypeStart)
				{
					offsetCount = Mathf.FloorToInt((float)(maxItemTypeStart - m_item_type_start) / contentConstraintCount);
				}
				m_item_type_start += offsetCount * contentConstraintCount;
				if (totalCount >= 0)
				{
					m_item_type_start = Mathf.Max(m_item_type_start, 0);
				}
				m_item_type_end = m_item_type_start;

				float offset = offsetCount * (elementSize + contentSpacing);
				content.anchoredPosition -= new Vector2(0, offset + (reverseDirection ? 0 : currentSize));
				contentBounds.center -= new Vector3(0, offset + currentSize / 2, 0);
				contentBounds.size = Vector3.zero;

				changed = true;
			}

			if (viewBounds.min.y > contentBounds.max.y && m_item_type_end > m_item_type_start)
			{
				float currentSize = contentBounds.size.y;
				float elementSize = (currentSize - contentSpacing * (CurrentLines - 1)) / CurrentLines;
				ReturnToTempPool(false, m_item_type_end - m_item_type_start);
				m_item_type_end = m_item_type_start;

				int offsetCount = Mathf.FloorToInt((viewBounds.min.y - contentBounds.max.y) / (elementSize + contentSpacing));
				if (totalCount >= 0 && m_item_type_start - offsetCount * contentConstraintCount < 0)
				{
					offsetCount = Mathf.FloorToInt((float)(m_item_type_start) / contentConstraintCount);
				}
				m_item_type_start -= offsetCount * contentConstraintCount;
				if (totalCount >= 0)
				{
					m_item_type_start = Mathf.Max(m_item_type_start, 0);
				}
				m_item_type_end = m_item_type_start;

				float offset = offsetCount * (elementSize + contentSpacing);
				content.anchoredPosition += new Vector2(0, offset + (reverseDirection ? currentSize : 0));
				contentBounds.center += new Vector3(0, offset + currentSize / 2, 0);
				contentBounds.size = Vector3.zero;

				changed = true;
			}

			if (viewBounds.min.y > contentBounds.min.y + threshold)
			{
				float size = DeleteItemAtEnd(), totalSize = size;
				while (size > 0 && viewBounds.min.y > contentBounds.min.y + threshold + totalSize)
				{
					size = DeleteItemAtEnd();
					totalSize += size;
				}
				if (totalSize > 0)
					changed = true;
			}

			if (viewBounds.max.y < contentBounds.max.y - threshold)
			{
				float size = DeleteItemAtStart(), totalSize = size;
				while (size > 0 && viewBounds.max.y < contentBounds.max.y - threshold - totalSize)
				{
					size = DeleteItemAtStart();
					totalSize += size;
				}
				if (totalSize > 0)
					changed = true;
			}

			if (viewBounds.min.y < contentBounds.min.y)
			{
				float size = NewItemAtEnd(), totalSize = size;
				while (size > 0 && viewBounds.min.y < contentBounds.min.y - totalSize)
				{
					size = NewItemAtEnd();
					totalSize += size;
				}
				if (totalSize > 0)
					changed = true;
			}

			if (viewBounds.max.y > contentBounds.max.y)
			{
				float size = NewItemAtStart(), totalSize = size;
				while (size > 0 && viewBounds.max.y > contentBounds.max.y + totalSize)
				{
					size = NewItemAtStart();
					totalSize += size;
				}
				if (totalSize > 0)
					changed = true;
			}

			if (changed)
			{
				ClearTempPool();
			}

			return changed;
		}

		protected override void OnDestroy()
		{
			RemovePool();
		}
	}
}
#endif
