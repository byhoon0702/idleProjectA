using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

	public Transform mapRoot;
	public MapObject map { get; private set; }

	public void ChangeMap(GameObject mapPrefab)
	{
		if (mapPrefab == null)
		{
			return;
		}
		if (map != null)
		{
			if (mapPrefab.name == map.name)
			{
				return;
			}
			Destroy(map.gameObject);
			map = null;
		}

		map = Instantiate(mapPrefab, mapRoot).GetComponent<MapObject>();
		map.name = map.name.Replace("(Clone)", "");
		map.transform.localPosition = Vector3.zero;
		map.transform.localScale = Vector3.one;
	}

	public Vector3 GetBossSpawn()
	{
		if (map == null)
		{
			return Vector3.zero;
		}

		return map.bossSpawnPos.position;
	}

}
