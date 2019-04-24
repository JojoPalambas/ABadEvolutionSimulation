using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundDisplayer : MonoBehaviour
{
    public TextMeshProUGUI display;
    public int round;

    public void SetDisplay(int n)
    {
        round = n;
        display.text = n.ToString();
    }
}
