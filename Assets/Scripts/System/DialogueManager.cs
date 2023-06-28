using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

public struct DialogueTag
{
	public const string SPEAKER_TAG = "speaker";
	public const string CAMERA_TAG = "camera";
	public const string ANIMATION_TAG = "animation";
	//public const string SPEAKER_TAG = "speaker";
}


public class DialogueManager : MonoBehaviour
{
	public static DialogueManager it;

	[SerializeField] private TextAsset currentInkJson;

	[SerializeField] private Transform dialogueUIRoot;
	[SerializeField] private GameObject talkBoxPrefab;
	[SerializeField] private TextAsset currentStoryJson;

	private InkExternalFunction inkExternalFunction;
	private Story currentStory;
	public bool dialogueIsPlaying
	{ get; private set; }


	private Unit currentSpeaker;
	//	const string SPEAKER_TAG = "speaker";

	private void Awake()
	{
		it = this;
		inkExternalFunction = new InkExternalFunction();
	}



	public void ChangeInkJson(TextAsset inkJson)
	{
		currentInkJson = inkJson;
	}

	public void BeginDialogue()
	{
		currentStory = new Story(currentInkJson.text);

		//inkExternalFunction.Bind(currentStory, "changeSpeaker", (System.Action<string>)FindSpeaker);
		ContinueStory();
	}
	Action onCompleteDialogue;
	public void EnterDialogueMode(Action _onCompleteDialogue)
	{
		RemoveTalkBox();

		dialogueIsPlaying = true;
		onCompleteDialogue = _onCompleteDialogue;
	}


	public Unit FindSpeaker(string name)
	{
		Unit speaker = null;
		switch (name)
		{
			case "PlayerUnit":

				speaker = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUnit>();

				break;
			case "EnemyUnit":
				var enemies = GameObject.FindObjectsOfType<EnemyUnit>();
				for (int i = 0; i < enemies.Length; i++)
				{
					if (enemies[i].isBoss)
					{
						speaker = enemies[i];
						break;
					}
				}
				break;
		}
		return speaker;
	}

	public void CreateTalkBox(string dialogue, Unit speaker)
	{
		RemoveTalkBox();
		var gameObject = Instantiate(talkBoxPrefab, dialogueUIRoot);
		UITalkBubble bubble = gameObject.GetComponent<UITalkBubble>();
		bubble.Show(dialogue, speaker, false, () => { ContinueStory(); });
	}

	public void CreateSkillBubble(string dialogue, Unit speaker)
	{
		RemoveTalkBox();
		var gameObject = Instantiate(talkBoxPrefab, dialogueUIRoot);
		UITalkBubble bubble = gameObject.GetComponent<UITalkBubble>();
		bubble.Show(dialogue, speaker, true);
	}

	public void RemoveTalkBox()
	{
		int childCount = dialogueUIRoot.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i)
		{
			GameObject.Destroy(dialogueUIRoot.transform.GetChild(i).gameObject);
		}
	}

	private void ContinueStory()
	{
		if (currentStory.canContinue)
		{
			var dialouge = currentStory.Continue();

			HandleTag(currentStory.currentTags);
			if (currentSpeaker == null)
			{
				ContinueStory();
				return;
			}
			CreateTalkBox(dialouge, currentSpeaker);
		}
		else
		{
			ExitDialogueMode();
		}
	}

	private void DisplayChoices()
	{
		List<Choice> choices = currentStory.currentChoices;
	}

	public void HandleTag(List<string> tags)
	{
		foreach (string tag in tags)
		{
			string[] splitTag = tag.Split(':');
			if (splitTag.Length != 2)
			{

			}
			string tagKey = splitTag[0].Trim();
			string tagValue = splitTag[1].Trim();
			switch (tagKey)
			{
				case DialogueTag.SPEAKER_TAG:
					currentSpeaker = FindSpeaker(tagValue);
					break;
				case DialogueTag.CAMERA_TAG:
					Camera(tagValue);
					break;


			}
		}
	}

	public void Camera(string value)
	{
		var speaker = FindSpeaker(value);



	}


	public void ExitDialogueMode()
	{
		dialogueIsPlaying = false;
		//inkExternalFunction.Unbind(currentStory, "changeSpeaker");

		onCompleteDialogue?.Invoke();
		onCompleteDialogue = null;
	}
}
