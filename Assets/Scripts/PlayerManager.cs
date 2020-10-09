using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    public RoleBase role;
    public bool dead;

    public bool acted;

    public Text playerNameText;
    public Image roleIcon;
    public Image bgImage;
    public Image targetImage;
    public Text deathImage;
    public Button killButton;
    public Button selectButton;

    public Dropdown roleDropdown;




    GameManager gm;


    private void Start()
    {
        gm = GameManager.instance;
        killButton.onClick.AddListener(() =>
        {
            Kill();
        });

        selectButton.onClick.AddListener(() =>
        {
            Select();
        });
    }

    public void Kill()
    {
        dead = !dead;
        deathImage.enabled = dead;
        acted = true;
    }

    public void Select()
    {
        foreach (PlayerManager p in gm.players)
        {
            p.targetImage.enabled = false;
        }
        if (gm.selectedPlayer == this)
        {
            gm.selectedPlayer = null;
            targetImage.enabled = false;
        }
        else
        {
            gm.selectedPlayer = this;
            targetImage.enabled = true;
        }
    }

    public void UpdateIcon(Sprite icon)
    {
        roleIcon.sprite = icon;
    }

}
