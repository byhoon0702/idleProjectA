/// Credit setchi (https://github.com/setchi)
/// Sourced from - https://github.com/setchi/FancyScrollView

using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	public class FancyScrollView<TData, TContext> : MonoBehaviour where TContext : class
	{
		[SerializeField, Range(float.Epsilon, 1f)]
		float cellInterval;
		[SerializeField, Range(0f, 1f)]
		float cellOffset;
		[SerializeField]
		bool loop;
		[SerializeField]
		GameObject cellBase;

		protected float currentPosition;
		readonly List<FancyScrollViewCell<TData, TContext>> cells =
			new List<FancyScrollViewCell<TData, TContext>>();

		protected TContext context;
		protected List<TData> cellData = new List<TData>();

		protected bool initContent = false;
		protected void Awake()
		{
			cellBase.SetActive(false);
		}

		/// <summary>
		/// 컨텍스트를 설정합니다
		/// </summary>
		/// <param name="context"></param>
		protected void SetContext(TContext context)
		{
			this.context = context;

			for (int i = 0; i < cells.Count; i++)
			{
				cells[i].SetContext(context);
			}
		}

		/// <summary>
		/// 셀을 생성 합니다
		/// </summary>
		/// <returns></returns>
		FancyScrollViewCell<TData, TContext> CreateCell()
		{
			var cellObject = Instantiate(cellBase);
			cellObject.SetActive(true);
			var cell = cellObject.GetComponent<FancyScrollViewCell<TData, TContext>>();

			var cellRectTransform = cell.transform as RectTransform;

			var scale = cell.transform.localScale;
			var sizeDelta = Vector2.zero;
			var offsetMin = Vector2.zero;
			var offsetMax = Vector2.zero;

			if (cellRectTransform)
			{
				sizeDelta = cellRectTransform.sizeDelta;
				offsetMin = cellRectTransform.offsetMin;
				offsetMax = cellRectTransform.offsetMax;
			}

			cell.transform.SetParent(cellBase.transform.parent);

			cell.transform.localScale = scale;
			if (cellRectTransform)
			{
				cellRectTransform.sizeDelta = sizeDelta;
				cellRectTransform.offsetMin = offsetMin;
				cellRectTransform.offsetMax = offsetMax;
			}

			cell.SetContext(context);
			cell.SetVisible(false);
			cell.UpdatePosition(currentPosition);
			return cell;
		}

#if UNITY_EDITOR
		float prevCellInterval, prevCellOffset;
		bool prevLoop;

		void LateUpdate()
		{
			//if (prevLoop != loop ||
			//	prevCellOffset != cellOffset ||
			//	prevCellInterval != cellInterval)
			//{
			//	UpdatePosition(currentPosition);

			//	prevLoop = loop;
			//	prevCellOffset = cellOffset;
			//	prevCellInterval = cellInterval;
			//}
		}
#endif

		/// <summary>
		/// 셀의 내용을 업데이트합니다
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="dataIndex"></param>
		void UpdateCellForIndex(FancyScrollViewCell<TData, TContext> cell, int dataIndex, bool forceUpdate)
		{
			if (loop)
			{
				dataIndex = GetLoopIndex(dataIndex, cellData.Count);
			}
			else if (dataIndex < 0 || dataIndex > cellData.Count - 1)
			{
				// 셀에 해당하는 데이터가 존재하지 않으면 셀을 표시하지 않습니다
				cell.SetVisible(false);
				return;
			}

			cell.SetVisible(true);
			cell.DataIndex = dataIndex;
			cell.InitializeContent(cellData[dataIndex], forceUpdate);
			cell.UpdateContent(cellData[dataIndex], forceUpdate);
		}


		/// <summary>
		/// 순환 index를 가져옵니다
		/// </summary>
		/// <param name="index"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		int GetLoopIndex(int index, int length)
		{
			if (index < 0)
			{
				index = (length - 1) + (index + 1) % length;
			}
			else if (index > length - 1)
			{
				index = index % length;
			}
			return index;
		}


		protected void InitContents(Int32 in_start_index = 0)
		{
			initContent = false;
			currentPosition = in_start_index;
			UpdatePosition(currentPosition, true);
		}

		/// <summary>
		/// 표시 내용을 업데이트합니다
		/// </summary>
		protected void UpdateContents(bool forceUpdate = false)
		{
			UpdatePosition(currentPosition, forceUpdate);
		}


		/// <summary>
		/// 스크롤 위치를 업데이트합니다
		/// </summary>
		/// <param name="position"></param>
		protected void UpdatePosition(float position, bool forceUpdate = false)
		{
			currentPosition = position;

			var visibleMinPosition = position - (cellOffset / cellInterval);
			var firstCellPosition = (Mathf.Ceil(visibleMinPosition) - visibleMinPosition) * cellInterval;
			var dataStartIndex = Mathf.CeilToInt(visibleMinPosition);
			var count = 0;
			var cellIndex = 0;

			for (float pos = firstCellPosition; pos <= 1f; pos += cellInterval, count++)
			{
				if (count >= cells.Count)
				{
					cells.Add(CreateCell());
				}
			}

			UpdateCell(firstCellPosition, dataStartIndex, forceUpdate);

			cellIndex = GetLoopIndex(dataStartIndex + count, cells.Count);

			for (; count < cells.Count; count++, cellIndex = GetLoopIndex(dataStartIndex + count, cells.Count))
			{
				cells[cellIndex].SetVisible(false);
			}
		}

		public void UpdateCell(float firstCellPosition, int dataStartIndex, bool forceUpdate)
		{
			int count = 0;
			int cellIndex = 0;

			for (float pos = firstCellPosition; pos <= 1f; count++, pos += cellInterval)
			{
				var dataIndex = dataStartIndex + count;
				cellIndex = GetLoopIndex(dataIndex, cells.Count);
				if (cells[cellIndex].gameObject.activeSelf)
				{
					cells[cellIndex].UpdatePosition(pos);
				}
				UpdateCellForIndex(cells[cellIndex], dataIndex, forceUpdate);
			}
		}
	}

	public sealed class FancyScrollViewNullContext
	{

	}

	public class FancyScrollView<TData> : FancyScrollView<TData, FancyScrollViewNullContext>
	{

	}
}
