using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager ctrl;
	public SoundDB bgmDB, sfxDB;
	//public float bgmvol = 1f;
	//public float sfxvol = 1f;
	private AudioSource m_bgmAS;
	public AudioSource bgmAS
	{
		get	
		{
			if (m_bgmAS == null)
			{
				m_bgmAS = new GameObject("BGM_Player").AddComponent<AudioSource>();
				m_bgmAS.transform.SetParent(transform);
			}
			return m_bgmAS;
		}
	}
	private AudioSource m_sfxAS;
	public AudioSource sfxAS
	{
		get
		{
			if (m_sfxAS == null)
			{
				m_sfxAS = new GameObject("SFX_Player").AddComponent<AudioSource>();
				m_sfxAS.transform.SetParent(transform);
			}
			return m_sfxAS;
		}
	}
	void Awake()
	{
		ctrl = this;
	}
	// Update is called once per frame
	void Update()
	{

	}
	public void PlayBGM(string id)
	{
		bgmAS.clip = bgmDB.SearchAudio(id);
		bgmAS.loop = true;
		//bgmAS.volume =bgmvol;
		bgmAS.Play();
	}
	public void PlaySFX(string id)
	{
		//sfxAS.volume =sfxvol;
		sfxAS.PlayOneShot(sfxDB.SearchAudio(id));
	}

	public void BGMValueCtrl(float num)
	{
		bgmAS.volume =num;
	}
	public void SFXValueCtrl(float num)
	{
		sfxAS.volume =num;
	}
}
