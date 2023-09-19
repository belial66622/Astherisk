using UnityEngine;
using UnityEngine.Audio;
using System.Linq;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
    {
        [SerializeField] private ScriptableSounds _savedSounds;
        [SerializeField] private AudioMixerGroup _masterAudio;
        private Sound[] _sounds;
        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
            }

            _sounds = new Sound[_savedSounds.GetSounds().Length];
            System.Array.Copy(_savedSounds.GetSounds(), _sounds, _sounds.Length);

            foreach(Sound _sound in _sounds)
            {
                _sound.AudioSource = gameObject.AddComponent<AudioSource>();
                AudioSourceInit(_sound);
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            LoadAudioSetting();
            //PlayBGM(_savedSounds.MainMenuBGM); // Only called once when application is opened
        }
    /*
        private void UpdateMixerVolume(bool _isMute)
        {
            _masterAudio.audioMixer.SetFloat("Master", _isMute ? -80f : 0f);
        }
      */  

        private void UpdateMixerBGM(bool _isMute)
        {
            _masterAudio.audioMixer.SetFloat("BGM", _isMute ? -80f : PlayerPrefs.GetFloat("_BGMVolume")==0? -80:Mathf.Log10(PlayerPrefs.GetFloat("_BGMVolume")/100)*20);
        }
        
        private void UpdateVolumeBGM(float volume)
        {
            if (PlayerPrefs.HasKey("_isBGMMute"))
            {
                if (PlayerPrefs.GetInt("_isBGMMute") == 1)
                { 
                    return; 
                }
                else
                {
                    _masterAudio.audioMixer.SetFloat("BGM", PlayerPrefs.GetFloat("_BGMVolume") == 0 ? -80 : Mathf.Log10(PlayerPrefs.GetFloat("_BGMVolume") / 100) * 20);
                }
            }
        }

        private void UpdateMixerSFX(bool _isMute)
        {
            _masterAudio.audioMixer.SetFloat("SFX", _isMute ? -80f : PlayerPrefs.GetFloat("_SFXVolume") == 0 ? -80 : Mathf.Log10(PlayerPrefs.GetFloat("_SFXVolume") / 100) * 20);
        }
        
        private void UpdateVolumeSFX(float volume)
        {
            if (PlayerPrefs.HasKey("_isSFXMute"))
            {
                if (PlayerPrefs.GetInt("_isSFXMute") == 1)
                { 
                    return; 
                }
                else
                {
                    _masterAudio.audioMixer.SetFloat("SFX", PlayerPrefs.GetFloat("_SFXVolume") == 0 ? -80 : Mathf.Log10(PlayerPrefs.GetFloat("_SFXVolume") / 100) * 20);
                }
            }
        }


    public void LoadAudioSetting()
        {
            bool _BGMtemp;
            if (!PlayerPrefs.HasKey("_isBGMMute"))
            {
                PlayerPrefs.SetInt("_isBGMMute", 0);
                PlayerPrefs.Save();
            }
            else
            { 
                _BGMtemp = PlayerPrefs.GetInt("_isBGMMute") == 0;
                UpdateMixerBGM(!_BGMtemp);
            }
            bool _SFXtemp;
            if (!PlayerPrefs.HasKey("_isSFXMute"))
            {
                PlayerPrefs.SetInt("_isSFXMute", 0);
                PlayerPrefs.Save();
            }
            else
            {
                _SFXtemp = PlayerPrefs.GetInt("_isSFXMute") == 0;
            UpdateMixerSFX(!_SFXtemp);
            }
            float _VBGMtemp;
            if (!PlayerPrefs.HasKey("_BGMVolume"))
            {
                PlayerPrefs.SetFloat("_BGMVolume", 0);
                PlayerPrefs.Save();
            }
            else
            {
               _VBGMtemp = PlayerPrefs.GetFloat("_BGMVolume");
            UpdateVolumeBGM(_VBGMtemp);
            }
            float _VSFXtemp;
            if (!PlayerPrefs.HasKey("_SFXVolume"))
            {
                PlayerPrefs.SetFloat("_SFXVolume", 0);
                PlayerPrefs.Save();
            }
            else
            { 
                _VSFXtemp = PlayerPrefs.GetFloat("_SFXVolume");
            UpdateVolumeSFX(_VSFXtemp);
            }

            //_temp = !(PlayerPrefs.GetInt("_isMute") == 0);
           // UpdateMixerVolume(_temp);
            //return _temp;
        }
        
        public void BGMVolume(float volume)
        {
        if (!PlayerPrefs.HasKey("_BGMVolume"))
        {
            PlayerPrefs.SetFloat("_BGMVolume", 0);
        }
            PlayerPrefs.SetFloat("_BGMVolume", volume);
            UpdateVolumeBGM(volume);
        }

        public void SFXVolume(float volume) 
        {

            if (!PlayerPrefs.HasKey("_SFXVolume"))
            {
                PlayerPrefs.SetFloat("_SFXVolume", 0);
            }
            PlayerPrefs.SetFloat("_SFXVolume", volume);
            UpdateVolumeSFX(volume);
        }

        public void UpdateBGMSetting(bool _isMute)
        {
            if (!PlayerPrefs.HasKey("_isBGMMute"))
            {
                PlayerPrefs.SetInt("_isBGMMute", 0);
            }
            PlayerPrefs.SetInt("_isBGMMute", _isMute ? 1 : 0);
            UpdateMixerBGM(_isMute);
        }

        public void UpdateSFXSetting(bool _isMute)
        {
            if (!PlayerPrefs.HasKey("_isSFXMute"))
            {
                PlayerPrefs.SetInt("_isSFXMute", 0);
            }
            PlayerPrefs.SetInt("_isSFXMute", _isMute ? 1 : 0);
            UpdateMixerSFX(_isMute);
        }

        private void AudioSourceInit(Sound _sound)
        {
            _sound.AudioSource.clip = _sound.AudioClip;
            _sound.AudioSource.volume = 1.0f;
            _sound.AudioSource.pitch = 1.0f;
            _sound.AudioSource.loop = _sound.IsBGM;
            _sound.AudioSource.playOnAwake = false;
            _sound.AudioSource.outputAudioMixerGroup = _sound.AudioMixerGroup;
            _sound.AudioSource.spatialBlend = _sound.Spatialblend;
        }

        public void PlayBGM(string _bgmName)
        {
            Sound _searchedBGM = System.Array.Find(_sounds, sound => sound.Name == _bgmName 
            && sound.IsBGM);

            if (_searchedBGM?.AudioSource.isPlaying == true) return;

            foreach(Sound s in System.Array.FindAll(_sounds, sound => sound.IsBGM &&
            sound.AudioSource.isPlaying))
            {
                s.AudioSource.Stop();
            }

            _searchedBGM?.AudioSource.Play();
        }

        public void PlaySFX(string _sfxName)
        {
            Sound _searchedSFX = System.Array.Find(_sounds, sound => sound.Name == _sfxName
            && !sound.IsBGM && !sound.AudioSource.isPlaying);

            if (_searchedSFX == null)
            {
                _searchedSFX = (Sound)System.Array.Find(_savedSounds.GetSounds(), sound =>
                sound.Name == _sfxName && !sound.IsBGM)?.Clone();

                _searchedSFX.AudioSource = gameObject.AddComponent<AudioSource>();
                AudioSourceInit(_searchedSFX);
                _sounds = _sounds.Concat(new Sound[] { _searchedSFX }).ToArray();
            }

            _searchedSFX.AudioSource.Play();
        }


         public Sound PlaySFXChecked(string _sfxName)
         {
             Sound _searchedSFX = System.Array.Find(_sounds, sound => sound.Name == _sfxName
             && !sound.IsBGM && !sound.AudioSource.isPlaying);

             if (_searchedSFX == null)
             {
                 _searchedSFX = (Sound)System.Array.Find(_savedSounds.GetSounds(), sound =>
                 sound.Name == _sfxName && !sound.IsBGM)?.Clone();

                 _searchedSFX.AudioSource = gameObject.AddComponent<AudioSource>();
                 AudioSourceInit(_searchedSFX);
                 _sounds = _sounds.Concat(new Sound[] { _searchedSFX }).ToArray();
             }

             _searchedSFX.AudioSource.Play();
        return _searchedSFX;
         }

    public AudioSource GetSfx(string _sfxName)
        { 
        Sound _searchedSFX = System.Array.Find(_sounds, sound => sound.Name == _sfxName
        && !sound.IsBGM && !sound.AudioSource.isPlaying);

        if (_searchedSFX == null)
        {
            _searchedSFX = (Sound)System.Array.Find(_savedSounds.GetSounds(), sound =>
            sound.Name == _sfxName && !sound.IsBGM)?.Clone();

            _searchedSFX.AudioSource = gameObject.AddComponent<AudioSource>();
            AudioSourceInit(_searchedSFX);
            _sounds = _sounds.Concat(new Sound[] { _searchedSFX }).ToArray();
        }

            return _searchedSFX.AudioSource;
        }


    public Sound PlayLoopingSFX(string _sfxName)
    {
        Sound _searchedSFX = System.Array.Find(_sounds, sound => sound.Name == _sfxName
        && !sound.AudioSource.isPlaying);

        if (_searchedSFX == null)
        {
            _searchedSFX = (Sound)System.Array.Find(_savedSounds.GetSounds(), sound =>
            sound.Name == _sfxName && !sound.IsBGM)?.Clone();

            _searchedSFX.AudioSource = gameObject.AddComponent<AudioSource>();
            AudioSourceInit(_searchedSFX);
            _sounds = _sounds.Concat(new Sound[] { _searchedSFX }).ToArray();
        }

        _searchedSFX.AudioSource.loop = true;
        _searchedSFX.AudioSource.Play();
        return _searchedSFX;
    }
    public void StopLoopingSFX(Sound _loopingSFX)
    {
        _loopingSFX.AudioSource.Stop();
        _loopingSFX.AudioSource.loop = _loopingSFX.IsBGM;
    }

    public void SetVolume(Sound _sound, float _volume, float distance)
    {
        _sound.AudioSource.volume = _volume;
        //Debug.Log("Manager volume" + _volume);
        _sound.AudioSource.maxDistance= distance;
        _sound.AudioSource.minDistance = distance;
    }
    public void StopAllSFX()
        {
            foreach(Sound s in _sounds)
            {
                if (s.IsBGM) continue;
                if (!s.AudioSource.loop) continue;
                if (!s.AudioSource.isPlaying) continue;
                s.AudioSource.Stop();
            }
        }
    }


