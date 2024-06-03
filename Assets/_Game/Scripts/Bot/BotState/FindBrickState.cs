using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindBrickState : IState<BotCtrl>
{
    public void OnEnter(BotCtrl bot)
    {
        bot.ChangeAnim("IsRunning", true);
        bot.MoveToNextBrick();
        Debug.Log("Enter FindBrickState");
    }

    public void OnExecute(BotCtrl bot)
    {
        if (bot.IsEnoughBrickToBuild())
        {
            bot.TransitionToState(bot.buildBridgeState);
            Debug.Log("Transition to BuildBridgeState");
        }
        else
        {
            bot.CheckArrival();
        }
    }

    public void OnExit(BotCtrl bot)
    {
        bot.ChangeAnim("IsRunning", false);
    }
}
