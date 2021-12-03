using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public AudioSource MainTheme;

    public RawImage ScreenOverlay;
    public event Action SceneLoaded;

    private AudioSource _currentMusic;
    private AudioSource _transitionMusic;

    private bool _isTransiting;
    private float _endVolume;
    private float _fadeInRate;
    private float _fadeOutRate;

    private bool _isOverlayFading;
    private float _overlayAlpha;
    private float _overlayFadingRate;
    private string _sceneToLoad;
    private string _startScene;

    public void CrossFadeTo(AudioSource audioSource, float duration)
    {
        if (_currentMusic == audioSource) return;
        if (_isTransiting) FinishAudioFade();

        _isTransiting = true;
        _endVolume = audioSource.volume;
        _fadeInRate = _endVolume / duration;
        _fadeOutRate = _currentMusic.volume / duration;
        _transitionMusic = audioSource;

        audioSource.volume = 0;
        audioSource.Play();
    }

    public void FadeToScene(string scene, float duration)
    {
        if (_isOverlayFading) FinishFade(true);

        _isOverlayFading = true;
        ScreenOverlay.gameObject.SetActive(true);
        ScreenOverlay.color = new Color(0, 0, 0, 0);
        _overlayFadingRate = 1 / duration;
        _overlayAlpha = 0;
        _sceneToLoad = scene;
        _startScene = SceneManager.GetActiveScene().name;
    }

    public void FadeOut(float duration)
    {
        if (_isOverlayFading) FinishFade(true);

        _isOverlayFading = true;
        ScreenOverlay.gameObject.SetActive(true);
        ScreenOverlay.color = new Color(0, 0, 0, 1f);
        _overlayFadingRate = -1 / duration;
        _overlayAlpha = 1;
        _sceneToLoad = null;
        _startScene = SceneManager.GetActiveScene().name;
    }

    private void Awake()
    {
        if (PublicVars.TransitionManager != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        PublicVars.TransitionManager = this;

        if (MainTheme != null)
        {
            _currentMusic = MainTheme;
            MainTheme.Play();
        }
    }

    private void FixedUpdate()
    {
        if (_isTransiting && _transitionMusic != null)
        {
            _currentMusic.volume -= _fadeOutRate * Time.deltaTime;
            _transitionMusic.volume += _fadeInRate * Time.deltaTime;
            if (_transitionMusic.volume >= _endVolume)
            {
                FinishAudioFade();
            }
        }

        if (_startScene != SceneManager.GetActiveScene().name) _isOverlayFading = false;

        if (_isOverlayFading)
        {
            _overlayAlpha += _overlayFadingRate * Time.deltaTime;
            ScreenOverlay.color = new Color(0, 0, 0, _overlayAlpha > 0 ? (_overlayAlpha < 1 ? _overlayAlpha : 1) : 0);
            if (_overlayAlpha > 1 || _overlayAlpha < 0)
            {
                FinishFade(false);
            }
        }
    }

    private void FinishAudioFade()
    {
        _isTransiting = false;
        _currentMusic.Stop();
        _currentMusic.volume = _endVolume;
        _transitionMusic.volume = _endVolume;

        _currentMusic = _transitionMusic;
        _transitionMusic = null;
    }

    private void FinishFade(bool quickLoad)
    {
        _isOverlayFading = false;
        if (_sceneToLoad != null)
        {
            if (quickLoad)
            {
                SceneManager.LoadScene(_sceneToLoad);
            }
            else SceneManager.LoadSceneAsync(_sceneToLoad).completed += HandleComplete;
        }
        else if (_overlayAlpha < 0) ScreenOverlay.gameObject.SetActive(false);
    }

    void HandleComplete(AsyncOperation operation)
    {
        FadeOut(PublicVars.GENERAL_FADE_TIME);
    }
}
