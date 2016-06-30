using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

    #region Public And Protected Members
    public int m_lengthOfSoundArray = 5;
    #endregion

    #region Main Methods
    
    void Awake()
    {
        m_audioSourceArray = GetComponents<AudioSource>();
        for(int i = 0;i< m_lengthOfSoundArray;i++ )
        {
            m_audioBehaviorArray[ i ] = AudioBehavior.CreateNewAudioBehavior( m_audioSourceArray[ i ] );
        }
        m_audioBehaviorArray[ 0 ].InitLerp( 0, 1, m_lerpTime );
    }
    // Use this for initialization
    void Start()
    {
        m_loopTimer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown( KeyCode.Keypad1 ) )
        {
            m_audioBehaviorArray[ 1 ].InitLerp( 0, 1, m_lerpTime );
        }
        if( Input.GetKeyDown( KeyCode.Keypad2 ) )
        {
            m_audioBehaviorArray[ 2 ].InitLerp( 0, 1, m_lerpTime );            
        }
        if( Input.GetKeyDown( KeyCode.Keypad3 ) )
        {
            m_audioBehaviorArray[ 3 ].InitLerp( 0, 1, m_lerpTime );
        }
        if( Input.GetKeyDown( KeyCode.Keypad4 ) )
        {
            m_audioBehaviorArray[ 4 ].InitLerp( 0, 1, m_lerpTime );
        }

        if( Time.time > m_loopTimer + m_audioSourceArray[ 0 ].clip.length-.5f )
        {
            if( !m_audioBehaviorArray[ m_audioBehaviorArray.Length-1 ].m_audio.isPlaying )
            {
                m_audioBehaviorArray[ m_audioBehaviorArray.Length - 1 ].m_audio.Play();
                m_audioBehaviorArray[ m_audioBehaviorArray.Length - 1 ].InitLerp( 0, 1, .3f );
                m_audioBehaviorArray[0].InitLerp( 1, .2f, .5f );
            }
            
        }
        if( Time.time > m_loopTimer + m_audioSourceArray[ 0 ].clip.length )
        {
            m_loopTimer = Time.time;
            ResetAllAudioExceptBase();
        }

        UpdateAudioBehaviors();
    }

    public void LerpAllOut()
    {
        if( !m_isLerpingOut )
        {
            
            m_isLerpingOut = true;

            for (int i =1;i<m_audioBehaviorArray.Length-1;i++ )
            {
                m_audioBehaviorArray[i].InitLerp( m_audioBehaviorArray[ i ].Volume, 0, m_lerpTime );
            }
            
        }
        
    }
    #endregion

    #region Utils

    private void ResetAllAudioExceptBase()
    {
        for (int i =1;i<m_audioBehaviorArray.Length-1;i++ )
        {
            m_audioBehaviorArray[ i ].m_audio.Play();
        }
        m_audioBehaviorArray[ 0 ].InitLerp( .5f, 1f, .1f );
    }

    private void UpdateAudioBehaviors()
    {
        foreach(AudioBehavior audioB in m_audioBehaviorArray )
        {
            audioB.Update();
        }
    }


    private void ResetAll()
    {
        print( "restart" );
        m_loopTimer = Time.time;

        for (var i = 1;i< m_lengthOfSoundArray;i++ )
        {

        }
        
    }

    #endregion

    #region Private Members

    private float m_lerpTime =1f;
    public bool m_isLerpingOut;
    AudioSource[] m_audioSourceArray = new AudioSource[5];
    AudioBehavior[] m_audioBehaviorArray =  new AudioBehavior[5];
    
    float m_loopTimer;
    #endregion

    public class AudioBehavior
    {
        public float m_lerpTime;
        public float m_currentLerpTime;
        public AudioSource m_audio;
        public float m_startLerpValue,m_endLerpValue;
        public float m_lengthOfSource;
        public float Volume {
            get
            {
                return m_currentVolume;
            }
        }
        private float m_currentVolume;

        public AudioBehavior(AudioSource _audioSource,float _lengthOfSound)
        {
            m_audio = _audioSource;
            m_lengthOfSource = _lengthOfSound;

        }
        public static AudioBehavior CreateNewAudioBehavior(AudioSource _audio)
        {
            
            return new AudioBehavior( _audio, _audio.clip.length );
        }

        public void InitLerp(float _start, float _end, float _lerpTime)
        {
            m_startLerpValue = _start;
            m_endLerpValue = _end;
            m_currentLerpTime = 0;
            m_lerpTime = _lerpTime/Time.deltaTime;
        }

        public void Update()
        {
            if( m_audio )
            {
                Lerp( m_startLerpValue, m_endLerpValue );
            }
        }

        
        private void Lerp(float _start, float _end)
        {
            print( "lerping" );
            float perc=0;

            if( m_currentLerpTime < m_lerpTime )
            {
                perc = m_currentLerpTime / m_lerpTime;
                m_currentLerpTime++;
            }
            else
            {
                perc = 1;                
            }
            m_currentVolume = Mathf.Lerp( _start, _end, perc );
            m_audio.volume = m_currentVolume;
        }
    }
}
