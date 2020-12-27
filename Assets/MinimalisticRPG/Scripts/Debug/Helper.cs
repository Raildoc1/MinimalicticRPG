using UnityEngine;
using TMPro;

public class Helper : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI text;

    private float _fps = 0;
    private float _deltaTime = 0;
    private int _frameCounter = 0;
    private float _minFps = 9999;
    private float _maxFps = 0;
    private float _avgFps = 0;

    private void Start() {
        //FindObjectOfType<StateSwitch>().CurrentState = State.COMBAT;
    }
    public void PrintDebugLog(string s) {
        Debug.Log(s);
    }

    public void Update() {
        string debugText = "";
        _frameCounter++;
        _deltaTime += Time.deltaTime;
        _fps = 1 / Time.deltaTime;

        debugText += $"FPS: {_fps}\n";

        if (_frameCounter % 120 == 0) {
            _avgFps = 120 / _deltaTime;
            _deltaTime = 0;
        }

        if (_frameCounter % 1200 == 0) {
            _maxFps = 0;
            _minFps = 9999;
        }

        debugText += $"AVG FPS: {_avgFps}\n";

        if (_fps > _maxFps) _maxFps = _fps;
        if (_fps < _minFps) _minFps = _fps;

        debugText += $"MIN FPS: {_minFps}\n";
        debugText += $"MAX FPS: {_maxFps}\n";

        text.text = debugText;
    }
}
