using UnityEngine;
using System.Collections;

public class AudioManger :MonoSingleton<AudioManger> {
	AudioListener mListener;
	
	public delegate void AudioCallBack();  
	
	public void PlayClipData(AudioClip audioClip,AudioCallBack callback)
	{
		NGUITools.PlaySound(audioClip);
		StartCoroutine(DelayedCallback(audioClip.length,callback));
	}

	private IEnumerator DelayedCallback(float time,AudioCallBack callback)
	{
		yield return new WaitForSeconds (time);
		if(callback != null){
			callback ();
			callback = null;
		}
	}
	
	public void PlayAudio(AudioClip clip){
		if (mListener == null )
		{
			mListener = GameObject.FindObjectOfType(typeof(AudioListener)) as AudioListener;

			if (mListener == null)
			{
				Camera cam = Camera.main;
				if (cam == null) cam = GameObject.FindObjectOfType(typeof(Camera)) as Camera;
				if (cam != null) mListener = cam.gameObject.AddComponent<AudioListener>();
			}
		}

		if (mListener != null && mListener.enabled)
		{
			AudioSource source = mListener.GetComponent<AudioSource>();
			if (source == null) source = mListener.gameObject.AddComponent<AudioSource>();
			source.clip = clip;
			source.Play();
		}
		
	}
	
	public void StopAudio(){
		if (mListener == null )
		{
			mListener = GameObject.FindObjectOfType(typeof(AudioListener)) as AudioListener;

			if (mListener == null)
			{
				Camera cam = Camera.main;
				if (cam == null) cam = GameObject.FindObjectOfType(typeof(Camera)) as Camera;
				if (cam != null) mListener = cam.gameObject.AddComponent<AudioListener>();
			}
		}

		if (mListener != null && mListener.enabled)
		{
			AudioSource source = mListener.GetComponent<AudioSource>();
			if (source == null) source = mListener.gameObject.AddComponent<AudioSource>();
			source.Stop();
		}
	}

}
