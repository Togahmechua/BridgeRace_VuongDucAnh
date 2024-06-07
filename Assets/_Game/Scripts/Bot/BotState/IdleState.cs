using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState<BotCtrl>
{
    public void OnEnter(BotCtrl bot)
    {
        bot.ChangeAnim("Idle", true);
        bot.TransitionToState(bot.findBrickState);
        // Debug.Log("Idle");
    }

    public void OnExecute(BotCtrl bot)
    {
        // bot.TransitionToState(bot.findBrickState);
    }

    public void OnExit(BotCtrl bot)
    {
        
    }
}
