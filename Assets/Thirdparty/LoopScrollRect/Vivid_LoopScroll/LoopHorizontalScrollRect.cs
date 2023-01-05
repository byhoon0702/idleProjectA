using UnityEngine;
using UnityEngine.UI;
using System.Collections;

#if !PROJECT_B
namespace VIVID
{
	[AddComponentMenu("UI/Loop Horizontal Scroll Rect", 50)]
	[DisallowMultipleComponent]
	public class LoopHorizontalScrollRect : LoopScrollRect
	{
		public Rect view_offset;

		protected override float GetSize(RectTransform item)
		{
			float size = contentSpacing;
			if (m_grid_layout != null)
			{
				size += m_grid_layout.cellSize.x;
			}
			else
			{
				size += LayoutUtility.GetPreferredWidth(item);
			}
			return size;
		}

		protected override float GetDimension(Vector2 vector)
		{
			return -vector.x;
		}

		protected override Vector2 GetVector(float value)
		{
			return new Vector2(-value, 0);
		}

		protected override void Awake()
		{
			base.Awake();
			directionSign = 1;

			GridLayoutGroup layout = content.GetComponent<GridLayoutGroup>();
			if (layout != null && layout.constraint != GridLayoutGroup.Constraint.FixedRowCount)
			{
				Debug.LogError("[LoopHorizontalScrollRect] unsupported GridLayoutGroup constraint");
			}
		}

		protected override bool UpdateItems(Bounds viewBounds, Bounds contentBounds)
		{
			bool changed = false;

			// special case: handling move several page in one frame
			if (viewBounds.max.x - view_offset.x < contentBounds.min.x && m_item_type_end > m_item_type_start)
			{
				float currentSize = contentBounds.size.x;
				float elementSize = (currentSize - contentSpacing * (CurrentLines - 1)) / CurrentLines;
				ReturnToTempPool(false, m_item_type_end - m_item_type_start);
				m_item_type_end = m_item_type_start;

				int offsetCount = Mathf.FloorToInt((contentBounds.min.x - viewBounds.max.x) / (elementSize + contentSpacing));
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
				content.anchoredPosition -= new Vector2(offset + (reverseDirection ? currentSize : 0), 0);
				contentBounds.center -= new Vector3(offset + currentSize / 2, 0, 0);
				contentBounds.size = Vector3.zero;

				changed = true;
			}

			if (viewBounds.min.x + view_offset.width > contentBounds.max.x && m_item_type_end > m_item_type_start)
			{
				int maxItemTypeStart = -1;
				if (totalCount >= 0)
				{
					maxItemTypeStart = Mathf.Max(0, totalCount - (m_item_type_end - m_item_type_start));
					maxItemTypeStart = (maxItemTypeStart / contentConstraintCount) * contentConstraintCount;
				}
				float currentSize = contentBounds.size.x;
				float elementSize = (currentSize - contentSpacing * (CurrentLines - 1)) / CurrentLines;
				ReturnToTempPool(true, m_item_type_end - m_item_type_start);
				// TODO: fix with contentConstraint?
				m_item_type_start = m_item_type_end;

				int offsetCount = Mathf.FloorToInt((viewBounds.min.x - contentBounds.max.x) / (elementSize + contentSpacing));
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
				content.anchoredPosition += new Vector2(offset + (reverseDirection ? 0 : currentSize), 0);
				contentBounds.center += new Vector3(offset + currentSize / 2, 0, 0);
				contentBounds.size = Vector3.zero;

				changed = true;
			}

			if (viewBounds.max.x + view_offset.width < contentBounds.max.x - threshold)
			{
				float size = DeleteItemAtEnd(), totalSize = size;
				while (size > 0 && viewBounds.max.x < contentBounds.max.x - threshold - totalSize)
				{
					size = DeleteItemAtEnd();
					totalSize += size;
				}
				if (totalSize > 0)
					changed = true;
			}

			if (viewBounds.min.x - view_offset.x > contentBounds.min.x + threshold)
			{
				float size = DeleteItemAtStart(), totalSize = size;
				while (size > 0 && viewBounds.min.x > contentBounds.min.x + threshold + totalSize)
				{
					size = DeleteItemAtStart();
					totalSize += size;
				}
				if (totalSize > 0)
					changed = true;
			}

			if (viewBounds.max.x + view_offset.width > contentBounds.max.x)
			{
				float size = NewItemAtEnd(), totalSize = size;
				while (size > 0 && viewBounds.max.x > contentBounds.max.x + totalSize)
				{
					size = NewItemAtEnd();
					totalSize += size;
				}
				if (totalSize > 0)
					changed = true;
			}

			if (viewBounds.min.x - view_offset.x < contentBounds.min.x)
			{
				float size = NewItemAtStart(), totalSize = size;
				while (size > 0 && viewBounds.min.x < contentBounds.min.x - totalSize)
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
