using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoleBase", menuName = "Werewolf Demo/RoleBase", order = 0)]
public class RoleBase : ScriptableObject
{
    public bool included;
    public Sprite icon;

    public int turnPriority;

    public int balanceValue;


}


