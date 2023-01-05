using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#if !PROJECT_B
namespace SG
{
	[RequireComponent(typeof(VIVID.LoopScrollRect))]
	[DisallowMultipleComponent]
	public class InitOnStart : MonoBehaviour
	{
		public int totalCount = -1;
		void Start()
		{
			var ls = GetComponent<VIVID.LoopScrollRect>();
			ls.totalCount = totalCount;
			ls.RefillCells();
		}
	}
}
#endif
