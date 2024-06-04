using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinState : IState<BotCtrl>
{
    public void OnEnter(BotCtrl bot)
    {
        bot.ChangeAnim("IsWinning");
        bot.isWinning = true;  
    }

    public void OnExecute(BotCtrl bot)
    {
        // bot.TransitionToState(bot.findBrickState);
    }

    public void OnExit(BotCtrl bot)
    {
        
    }
}
