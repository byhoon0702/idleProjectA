/// Credit setchi (https://github.com/setchi)
/// Sourced from - https://github.com/setchi/FancyScrollView

namespace UnityEngine.UI.Extensions
{

	public class FancyScrollViewCell<TData, TContext> : MonoBehaviour where TContext : class
	{

		protected bool m_is_initialize = false;
		/// <summary>
		/// 컨텍스트를 설정합니다
		/// </summary>
		/// <param name="context"></param>
		public virtual void SetContext(TContext context)
		{
		}

		public virtual void InitializeContent(TData itemData, bool forceUpdate = false)
		{
			if (forceUpdate)
			{
				m_is_initialize = false;
			}

			if (m_is_initialize)
			{
				return;
			}
			m_is_initialize = true;
		}


		/// <summary>
		/// 셀 내용을 업데이트합니다
		/// </summary>
		/// <param name="itemData"></param>
		public virtual void UpdateContent(TData itemData, bool forceUpdate = false)
		{
		}

		/// <summary>
		/// 셀의 위치를 업데이트합니다
		/// </summary>
		/// <param name="position"></param>
		public virtual void UpdatePosition(float position)
		{
		}

		/// <summary>
		/// 셀의 표시 / 숨기기를 설정합니다
		/// </summary>
		/// <param name="visible"></param>
		public virtual void SetVisible(bool visible)
		{
			gameObject.SetActive(visible);
		}

		/// <summary>
		/// 이 셀에 표시되는 데이터의 인덱스
		/// </summary>
		public int DataIndex
		{
			get; set;
		}
	}

	public class FancyScrollViewCell<TData> : FancyScrollViewCell<TData, FancyScrollViewNullContext>
	{

	}
}
