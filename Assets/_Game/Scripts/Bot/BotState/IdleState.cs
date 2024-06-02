using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState<BotCtrl>
{
    public void OnEnter(BotCtrl bot)
    {
        bot.ChangeAnim("Idle",true);
    }

    public void OnExecute(BotCtrl bot)
    {
        
    }

    public void OnExit(BotCtrl bot)
    {

    }
}
