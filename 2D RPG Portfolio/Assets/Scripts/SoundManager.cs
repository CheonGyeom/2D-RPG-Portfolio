using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // �̱����� �Ҵ��� ��������

    public AudioMixer mainMixer; // ����� �ͼ�
    public Slider bgmBar; // BGM ���� ���� �����̴�
    public Slider sfxBar; // SFX ���� ���� �����̴�



    public List<AudioClip> audioClips; // ����� ����� Ŭ�� ����Ʈ
    private Dictionary<string, AudioClip> audioDictionary; // ����� Ŭ���� ������ ��ųʸ�

    private AudioSource bgmSource; // ������� ����� �ҽ�
    private AudioSource sfxSource; // ȿ���� ����� �ҽ�

    public AudioMixerGroup bgmMixerGroup; // ������� ����� �ͼ� �׷�
    public AudioMixerGroup sfxMixerGroup; // ȿ���� ����� �ͼ� �׷�

    private string currentBGM; // ���� ������� ������� �̸�


    // �̱��� ����
    private void Awake()
    {
        // �̱��� ������ ���������
        if (instance == null)
        {
            // �ڽ��� �Ҵ�
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� SoundManager�� �������� ����
        }
        // �̱��� ������ ������� ������
        else
        {
            // �ڽ��� �ı�
            Destroy(gameObject);
        }



        // ��ųʸ� �Ҵ�
        audioDictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in audioClips)
        {
            // ����� Ŭ���� �̸��� Ű�� ����� Ŭ���� ��ųʸ��� ���
            audioDictionary.Add(clip.name, clip);
        }

        // ���� �Ŵ����� ����� �ҽ� ������Ʈ �ο�
        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        // ���� �����̴� �� �ʱ�ȭ
        bgmBar.value = PlayerPrefs.GetFloat("BGM_Volume", 0.5f); // �� ��° �μ��� �ʱ� ��
        sfxBar.value = PlayerPrefs.GetFloat("SFX_Volume", 0.5f);

        // ���� �ʱ�ȭ
        mainMixer.SetFloat("BGM_Volume", Mathf.Log10(PlayerPrefs.GetFloat("BGM_Volume", 0.5f)) * 20);
        mainMixer.SetFloat("SFX_Volume", Mathf.Log10(PlayerPrefs.GetFloat("SFX_Volume", 0.5f)) * 20);
    }

    public void SetVolume_BGM(float volume)
    {
        mainMixer.SetFloat("BGM_Volume", Mathf.Log10(volume) * 20); // ���� ����
        PlayerPrefs.SetFloat("BGM_Volume", volume); // ���� ���� �� ����
    }

    public void SetVolme_SFX(float volume)
    {
        mainMixer.SetFloat("SFX_Volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFX_Volume", volume);
    }

    // ȿ���� ��� �Լ�
    public void PlaySound(string soundName)
    {
        // Key�� �����Ѵٸ� ����� ���
        if (audioDictionary.ContainsKey(soundName))
        {
            AudioClip clip = audioDictionary[soundName];

            sfxSource.outputAudioMixerGroup = sfxMixerGroup; // ����� �ͼ� �׷� �Ҵ�

            sfxSource.PlayOneShot(clip); // ȿ���� ���
        }
        else
        {
            Debug.LogWarning("���� �̸��� ȿ������ ã�� �� ����: " + soundName);
        }
    }

    // ������� ��� �Լ�
    public void PlayBackgroundMusic(string musicName)
    {
        // Key�� �����Ѵٸ� ����� ���
        if (audioDictionary.ContainsKey(musicName))
        {
            // ���� ��� ���� BGM�� �ٸ� BGM�� ���� ���
            if (currentBGM != musicName)
            {
                AudioClip clip = audioDictionary[musicName];
                bgmSource.clip = clip;
                bgmSource.loop = true; // ������� ����

                bgmSource.outputAudioMixerGroup = bgmMixerGroup; // ����� �ͼ� �׷� �Ҵ�

                currentBGM = musicName; // ���� ��� ���� BGM �̸� ����

                bgmSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("���� �̸��� ��������� ã�� �� ����: " + musicName);
        }
    }

    // ������� �ߴ� �Լ�
    public void StopBackgroundMusic()
    {
        bgmSource.Stop();
    }
}
