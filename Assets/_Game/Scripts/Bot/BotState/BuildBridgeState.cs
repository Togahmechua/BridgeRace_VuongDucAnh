using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBridgeState : IState<BotCtrl>
{
    public void OnEnter(BotCtrl bot)
    {
        bot.GoToFinishPoint();
        bot.BricksToFind = bot.RandomBrickCount();
        // Debug.Log("Enter BuildBridgeState");
        if (bot.stackBricks.Count <= 0)
        {
            bot.TransitionToState(bot.findBrickState);
        }
    }

    public void OnExecute(BotCtrl bot)
    {
        if (bot.stackBricks.Count <= 0)
        {
            bot.TransitionToState(bot.findBrickState);
            // Debug.Log("Not enough bricks, transition to FindBrickState");
        }
    }

    public void OnExit(BotCtrl bot)
    {
        bot.ChangeAnim("IsRunning", false);
    }
}
