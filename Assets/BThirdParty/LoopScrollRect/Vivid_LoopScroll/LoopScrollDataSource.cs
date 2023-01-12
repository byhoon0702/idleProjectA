

using UnityEngine;
namespace VIVID
{
	public interface ILoopItem
	{
		int Index
		{
			get;
			set;
		}
	}
	public abstract class LoopScrollDataSource
	{
		public abstract void ProvideData(Transform transform, int idx);
		public abstract void InitializeData(Transform transform, int idx);
	}

	public class LoopScrollSendIndexSource : LoopScrollDataSource
	{

		public static readonly LoopScrollSendIndexSource Instance = new LoopScrollSendIndexSource();

		public LoopScrollSendIndexSource()
		{

		}

		public override void ProvideData(Transform transform, int idx)
		{
			transform.SendMessage("ScrollCellContent", idx);
		}

		public override void InitializeData(Transform transform, int idx)
		{
			transform.SendMessage("ScrollCellContent", idx);
		}
	}

	public class LoopScrollArraySource<T> : LoopScrollDataSource where T : SCLoopScrollItem
	{
		T[] objectsToFill;

		public LoopScrollArraySource(T[] objectsToFill)
		{
			this.objectsToFill = objectsToFill;
		}

		public override void ProvideData(Transform transform, int idx)
		{
			transform.SendMessage("ScrollCellContent", objectsToFill[idx]);
		}
		public override void InitializeData(Transform transform, int idx)
		{
			transform.SendMessage("ScrollCellContent", objectsToFill[idx]);
		}
	}

}
