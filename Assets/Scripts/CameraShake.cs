using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
	public static CameraShake Instance;


	private void Awake()
	{
		Instance = this;
	}

	

	public void Shakeeee(float d, float m)
	{
		StartCoroutine(Shake(d, m));
	}

	public IEnumerator Shake(float duration, float magnitude)
	{
		Vector3 originalPos = transform.localPosition;
		float elapsed = 0f;
		while(elapsed < duration)
		{
			float x = Random.Range(-1f,1f) * magnitude;
			float y = Random.Range(-1f,1f) * magnitude;
			transform.localPosition = new Vector3(x,y, originalPos.z);
			elapsed += Time.deltaTime;
			yield return null;
		}

		transform.localPosition = originalPos;
	}
}
