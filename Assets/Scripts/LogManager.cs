using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    public PlayerManager actor;

    public PlayerManager target;


    public Image actorImage, targetImage;
    public Text actorText, targetText;


    private void Start()
    {

        actorImage.sprite = actor.role.icon;
        actorText.text = actor.playerNameText.text;

        if (target)
        {
            targetText.text = target.playerNameText.text;
            if(targetImage.sprite)
            targetImage.sprite = target.role.icon;
        }
        else
        {
            targetImage.sprite = null;
            targetText.text = "";
        }
    }
}
