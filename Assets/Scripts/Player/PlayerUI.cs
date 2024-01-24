using System;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private RectTransform healthFill;

    private Player player;

    private void SetHealthAmout(float _pv)
    {
        healthFill.localScale = new Vector3(1f, _pv, 1f);
    }

    public void SetPlayer(Player _player)
    {
        player = _player;
    }
    

    private void Update()
    {
        SetHealthAmout(player.getCurrentHealth());
    }
    
    
}
