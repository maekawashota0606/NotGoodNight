using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// BGM��SE�̊Ǘ�������}�l�[�W���B�V���O���g���B
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
	//�{�����[���ۑ��p��key�ƃf�t�H���g�l
	private const string BGM_VOLUME_KEY = "BGM_VOLUME_KEY";
	private const string SE_VOLUME_KEY = "SE_VOLUME_KEY";
	private const float BGM_VOLUME_DEFULT = 1.0f;
	private const float SE_VOLUME_DEFULT = 1.0f;

	//BGM���t�F�[�h����̂ɂ����鎞��
	public const float BGM_FADE_SPEED_RATE_HIGH = 0.9f;
	public const float BGM_FADE_SPEED_RATE_LOW = 0.3f;
	private float _bgmFadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH;

	//������BGM���ASE��
	private string _nextBGMName;
	private string _nextSEName;

	//BGM���t�F�[�h�A�E�g����
	private bool _isFadeOut = false;

	//BGM�p�ASE�p�ɕ����ăI�[�f�B�I�\�[�X������
	public AudioSource AttachBGMSource, AttachSESource;

	//�SAudio��ێ�
	private Dictionary<string, AudioClip> _bgmDic, _seDic;


	private void Awake()
	{
		if (this != Instance)
		{
			Destroy(this);
			return;
		}

		DontDestroyOnLoad(this.gameObject);

		//���\�[�X�t�H���_����SSE&BGM�̃t�@�C����ǂݍ��݃Z�b�g
		_bgmDic = new Dictionary<string, AudioClip>();
		_seDic = new Dictionary<string, AudioClip>();

		object[] bgmList = Resources.LoadAll("Audio/BGM");
		object[] seList = Resources.LoadAll("Audio/SE");

		foreach (AudioClip bgm in bgmList)
		{
			_bgmDic[bgm.name] = bgm;
		}
		foreach (AudioClip se in seList)
		{
			_seDic[se.name] = se;
		}
	}

	private void Start()
	{
		AttachBGMSource.volume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFULT);
		AttachSESource.volume = PlayerPrefs.GetFloat(SE_VOLUME_KEY, SE_VOLUME_DEFULT);
	}

	//SE

	/// <summary>
	/// �w�肵���t�@�C������SE�𗬂��B��������delay�Ɏw�肵�����Ԃ����Đ��܂ł̊Ԋu���󂯂�
	/// </summary>
	public void PlaySE(string seName, float delay = 0.0f)
	{
		if (!_seDic.ContainsKey(seName))
		{
			Debug.Log(seName + "�Ƃ������O��SE������܂���");
			return;
		}

		_nextSEName = seName;
		Invoke("DelayPlaySE", delay);
	}

	private void DelayPlaySE()
	{
		AttachSESource.PlayOneShot(_seDic[_nextSEName] as AudioClip);
	}

	//BGM

	/// <summary>
	/// �w�肵���t�@�C������BGM�𗬂��B���������ɗ���Ă���ꍇ�͑O�̋Ȃ��t�F�[�h�A�E�g�����Ă���B
	/// ��������fadeSpeedRate�Ɏw�肵�������Ńt�F�[�h�A�E�g����X�s�[�h���ς��
	/// </summary>
	public void PlayBGM(string bgmName, float fadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH)
	{
		if (!_bgmDic.ContainsKey(bgmName))
		{
			Debug.Log(bgmName + "�Ƃ������O��BGM������܂���");
			return;
		}

		//����BGM������Ă��Ȃ����͂��̂܂ܗ���
		if (!AttachBGMSource.isPlaying)
		{
			_nextBGMName = "";
			AttachBGMSource.clip = _bgmDic[bgmName] as AudioClip;
			AttachBGMSource.Play();
		}
		//�ႤBGM������Ă��鎞�́A����Ă���BGM���t�F�[�h�A�E�g�����Ă��玟�𗬂��B����BGM������Ă��鎞�̓X���[
		else if (AttachBGMSource.clip.name != bgmName)
		{
			_nextBGMName = bgmName;
			FadeOutBGM(fadeSpeedRate);
		}

	}

	/// <summary>
	/// ���ݗ���Ă���Ȃ��t�F�[�h�A�E�g������
	/// fadeSpeedRate�Ɏw�肵�������Ńt�F�[�h�A�E�g����X�s�[�h���ς��
	/// </summary>
	/// <param name="fadeSpeedRate"></param>
	public void FadeOutBGM(float fadeSpeedRate = BGM_FADE_SPEED_RATE_LOW)
	{
		_bgmFadeSpeedRate = fadeSpeedRate;
		_isFadeOut = true;
	}

	private void Update()
	{
		if (!_isFadeOut)
		{
			return;
		}

		//���X�Ƀ{�����[���������Ă����A�{�����[����0�ɂȂ�����{�����[����߂����̋Ȃ𗬂�
		AttachBGMSource.volume -= Time.deltaTime * _bgmFadeSpeedRate;
		if (AttachBGMSource.volume <= 0)
		{
			AttachBGMSource.Stop();
			AttachBGMSource.volume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFULT);
			_isFadeOut = false;

			if (!string.IsNullOrEmpty(_nextBGMName))
			{
				PlayBGM(_nextBGMName);
			}
		}

	}

	/*���ʕύX*/
	/// <summary>
	/// BGM��SE�̃{�����[����ʁX�ɕύX&�ۑ�
	/// </summary>
	public void ChangeVolume(float BGMVolume, float SEVolume)
	{
		AttachBGMSource.volume = BGMVolume;
		AttachSESource.volume = SEVolume;

		PlayerPrefs.SetFloat(BGM_VOLUME_KEY, BGMVolume);
		PlayerPrefs.SetFloat(SE_VOLUME_KEY, SEVolume);
	}
}
