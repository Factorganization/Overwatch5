using TMPro;
using UnityEngine;

namespace RunTimeContent.Utility
{
    public class FPSCounter : MonoBehaviour
    {
        #region methodes
        
        private void Update()
        {
            _timeLeft -= Time.deltaTime;
            _accum    += Time.timeScale / Time.deltaTime;
            ++_frames;

            if (_timeLeft > 0)
                return;
            
            _fps = _accum / _frames;

            text.color = _fps switch
            {
                < BadValue => _badColor,
                < NeutralValue => _neutralColor,
                _ => _goodColor
            };

            text.text = _fps.ToString("f1");
            _timeLeft  = UpdateInterval;
            _accum     = 0;
            _frames    = 0;
        }
        
        #endregion
        
        #region fields
        
        [SerializeField] private TextMeshProUGUI text;
  
        private readonly Color _badColor = Color.red;
        private readonly Color _neutralColor = Color.yellow;
        private readonly Color _goodColor = Color.green;

        private const float BadValue = 50;
        private const float NeutralValue = 60;

        private float _fps;

        private const float UpdateInterval = .1f;

        private float _accum;
        private float _frames;
        private float _timeLeft;
        
        #endregion
    }
}