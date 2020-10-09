using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    #region singleton
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion


    [Header("variables")]
    public int nightCount;

    public int balanceValue;

    public PlayerManager actingPlayer;
    public PlayerManager selectedPlayer;
    public List<RoleBase> playingRoles;

    public List<PlayerManager> players;

    public List<string> playerNames;

    public List<string> roleNames;

    [Header("Constants")]
    public LogManager nightLogPrefab;
    public GameObject nightLogSeparatorPrefab;
    public Transform nightLogContainer;
    public PlayerManager playerPrefab;
    public Transform playerContainer;
    public ModManager modPrefab;
    public Transform modContainer;
    public Image rolePrefab;
    public Transform roleContainer;
    public Text balanceText;
    public Text playerCountText;
    public Text unassignedCountText;
    public InputField playerNameInputfield;
    public Transform playerNameContainer;
    public GameObject playerNamePrefab;
    public List<RoleBase> allRoles;


    private void Start()
    {

        PopulateRoleSelector();
        UpdatePlayerCount();
        UpdateUnassigendCount();
    }



    public void AddPlayer(string s)
    {

        if (s == "")
            return;
        playerNameInputfield.text = "";
        playerNames.Add(s);
        UpdatePlayerCount();
        GameObject tmp = Instantiate(playerNamePrefab, playerNameContainer);
        tmp.GetComponentInChildren<Text>().text = s;
        tmp.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            playerNames.Remove(s);
            Destroy(tmp);
            UpdatePlayerCount();
        });

    }

    public void PopulateRoleSelector()
    {
        foreach (RoleBase c in allRoles)
        {
            if (c.included)
            {
                Image tmp = Instantiate(rolePrefab, roleContainer);
                tmp.sprite = c.icon;
                tmp.GetComponent<Toggle>().onValueChanged.AddListener((b) =>
                {
                    UpdateBalanceValue(b ? c.balanceValue : -c.balanceValue);
                    if (b)
                    {
                        playingRoles.Add(c);
                    }
                    else
                    {
                        playingRoles.Remove(c);
                    }
                    UpdateUnassigendCount();
                });
            }
        }
    }
    public void UpdatePlayerCount()
    {
        playerCountText.text = "Players: " + playerNames.Count;
    }

    public void UpdateUnassigendCount()
    {
        unassignedCountText.text = playerNames.Count - playingRoles.Count + "";
    }

    public void UpdateBalanceValue(int v)
    {
        balanceValue += v;
        balanceText.text = balanceValue + "";
    }

    public void AddMod()
    {
        Instantiate(modPrefab, modContainer).dayText.text = modContainer.childCount + "";
    }



    public void EndRoleSelection()
    {
        nightCount = 0;

        foreach (Transform child in nightLogContainer)
        {
            Destroy(child.gameObject);
        }
        SortRoleNames();
        SpawnPlayers();
    }

    public void SpawnPlayers()
    {
        foreach (Transform child in playerContainer)
        {
            Destroy(child.gameObject);
        }
        players = new List<PlayerManager>();
        foreach (string pn in playerNames)
        {
            PlayerManager tmp = Instantiate(playerPrefab, playerContainer);
            players.Add(tmp);
            tmp.playerNameText.text = pn;
            tmp.roleDropdown.AddOptions(roleNames);
            tmp.roleDropdown.onValueChanged.AddListener((i) =>
            {
                if (nightCount == 0)
                {
                    ActivatePlayer(players.IndexOf(tmp));
                }
                tmp.role = playingRoles[i - 1];
                tmp.UpdateIcon(tmp.role.icon);
            });
        }

    }

    public void SortRoleNames()
    {

        playingRoles = new List<RoleBase>(playingRoles.OrderByDescending((x) => x.turnPriority));
        roleNames.Clear();
        foreach (RoleBase rb in playingRoles)
        {
            roleNames.Add(rb.name);
        }
    }

    public void ReorderRolesByTurnpriority()
    {
        players = new List<PlayerManager>(players.OrderByDescending((x) => x.role.turnPriority));
        int i = 0;
        foreach (PlayerManager p in players)
        {
            p.transform.SetSiblingIndex(i);
            i++;
        }
    }
    public void TurnStart()
    {
        ResetPlayerStatus();
        for (int i = 0; i < players.Count; i++)
        {
            if (!players[i].acted)
            {
                ActivatePlayer(i);
                return;
            }
        }
        foreach (PlayerManager p in players)
        {
            if(p.role.turnPriority>0&&!p.dead)
            {
                EndNight();
                return;
            }
        }
       
    }

    public void ActivatePlayer(int index)
    {
        ResetPlayerStatus();
        actingPlayer = players[index];
        actingPlayer.acted = true;
        actingPlayer.bgImage.color = Color.green;
    }

    public void ResetPlayerStatus()
    {
        actingPlayer = null;
        selectedPlayer = null;
        foreach (PlayerManager item in players)
        {
            item.bgImage.color = Color.gray;
            item.targetImage.enabled = false;
        }
    }

    public void SelectTarget()
    {
        if (actingPlayer != null)
        {
            LogManager tmp = Instantiate(nightLogPrefab, nightLogContainer);
            tmp.actor = actingPlayer;
            tmp.target = selectedPlayer;
            if (nightCount > 0)
                TurnStart();
            else
            {
                ResetPlayerStatus();
            }
        }
    }

    public void EndNight()
    {
        foreach (PlayerManager item in players)
        {
            if (item.role.turnPriority > 0 && !item.dead)
            {
                item.acted = false;

            }
        }
        nightCount++;
        Instantiate(nightLogSeparatorPrefab, nightLogContainer).GetComponentInChildren<Text>().text = "Night " + nightCount;
        ReorderRolesByTurnpriority();
        TurnStart();
    }
}


