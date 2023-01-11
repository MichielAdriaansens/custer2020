using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public Color32 _color;
    public Color32 _newColor;

    public float setTime = 0.2f;
    public float _timer = 0;
    int _switch = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _color = GetComponent<SpriteRenderer>().color;
    }
    void ColorChange()
    {
        //red
        if(_switch == 0)
        {
            _newColor = Color.red;
        }
        else if(_switch == 1)
        {
            _newColor = Color.magenta;
        }
        else if(_switch == 2)
        {
            _newColor = Color.blue;
        }
        else if(_switch == 3)
        {
            _newColor = Color.cyan;
        }
        else if(_switch == 4)
        {
            _newColor = Color.green;
        }
        else if(_switch == 5)
        {
            _newColor = Color.yellow;
        }

        if(_timer > setTime) //reset Timer
        {
            _timer = 0;

            _color = GetComponent<SpriteRenderer>().color;
            if(_switch <= 5) //changeColor
            {
                _switch++;
            }
            else
            {
                _switch = 0;
                _newColor = Color.red;
                return;
            }
        }
        else // proceed Timer
        {
            _timer += Time.deltaTime;
        }
        
        float tPercent = _timer / setTime;
        

        GetComponent<SpriteRenderer>().color = Color32.Lerp(_color, _newColor, tPercent);

    }
    // Update is called once per frame
    void Update()
    {
        ColorChange();
    }
}
