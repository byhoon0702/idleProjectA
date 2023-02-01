using UnityEngine;

public class MapController : MonoBehaviour
{
	public SpriteRenderer spriteMap;
	public SpriteRenderer spriteFarMap;
	public SpriteRenderer spriteMiddleMap;

	public void Reset()
	{
		Debug.Log(spriteMiddleMap.sprite.rect);

		Vector2 originalCloseSize = new Vector2(spriteMap.sprite.rect.width, spriteMap.sprite.rect.height);
		Vector2 originalMiddleSize = new Vector2(spriteMiddleMap.sprite.rect.width, spriteMiddleMap.sprite.rect.height);
		Vector2 originalFarSize = new Vector2(spriteFarMap.sprite.rect.width, spriteFarMap.sprite.rect.height);


		Vector3 closeResetPos = spriteMap.transform.localPosition;
		Vector3 middleResetPos = spriteMiddleMap.transform.localPosition;
		Vector3 farResetPos = spriteFarMap.transform.localPosition;

		closeResetPos.x = middleResetPos.x = farResetPos.x = 0;

		spriteMap.transform.localPosition = closeResetPos;
		spriteMiddleMap.transform.localPosition = middleResetPos;
		spriteFarMap.transform.localPosition = farResetPos;

		spriteMap.size = originalCloseSize / 100;
		spriteMiddleMap.size = originalMiddleSize / 100;
		spriteFarMap.size = originalFarSize / 100;

	}
	public void Scroll(Vector2 scroll)
	{
		spriteMap.size += scroll * 2;
		spriteMap.transform.Translate(-scroll * 2, Space.Self);
		spriteMiddleMap.size += scroll;
		spriteFarMap.size += scroll;
		spriteFarMap.transform.Translate(scroll / 5, Space.Self);
	}

	public void SetBG(string _closeBg, string _middleBg, string _farBg)
	{
		spriteMap.sprite = Resources.Load<Sprite>($"BG/{_closeBg}");
		spriteMiddleMap.sprite = Resources.Load<Sprite>($"BG/{_middleBg}");
		spriteFarMap.sprite = Resources.Load<Sprite>($"BG/{_farBg}");
	}

}
