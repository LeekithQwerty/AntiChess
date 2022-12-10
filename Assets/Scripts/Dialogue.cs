using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Dialogue : MonoBehaviour
{
	public float delay = 0.1f;
	public bool killCartoonChessPiece=false;
	private string[] fullText = new string[] { "Yassas.... I am the last king of Athens, I shall sacrifice myself to win the war against the spartans \n \n Press ENTER", "\nPress ENTER to kill me", "\nWelcome to the game made for sadists \n \n Press ENTER" };
	private string currentText = "";
	int counter=0;
	[SerializeField] TextMeshProUGUI storyText;
	[SerializeField] GameObject platform;
	[SerializeField] GameObject startScene;
	[SerializeField] GameObject storyScene;
	// Use this for initialization
	void Start()
	{
		counter = 0;
		StartCoroutine(ShowText());
	}

	IEnumerator ShowText()
	{
		Debug.Log(fullText[0]);
		for (int i = 0; i <= fullText[counter].Length; i++)
		{
			currentText = fullText[counter].Substring(0, i);
			storyText.text = currentText;
			yield return new WaitForSeconds(delay);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			Debug.Log("YESSS");
			counter = counter + 1;
			if(counter==2)
			{
				killCartoonChessPiece = true;
				Destroy(platform);
			}
			if(counter==3)
			{

				storyScene.SetActive(false);
				startScene.SetActive(true);
				
			}
			if(counter<3)
				StartCoroutine(ShowText());
		}

	}
	
}
