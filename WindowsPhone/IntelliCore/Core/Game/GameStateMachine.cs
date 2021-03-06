﻿using Intelli.Config;
using Intelli.Core.Game.Board;
using Intelli.Core.Game.Board.Notifies;
using Intelli.Core.Game.Events;
using Intelli.Core.Game.Player;
using Intelli.Core.Game.Player.Events;
using Intelli.Core.Game.Player.States;
using Intelli.Core.Game.States;
using IntelliCore.Core.Game.Exceptions;
using IntelliCore.Core.Game.Player.Notifies;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelli.Core.Game
{
    /// <summary>
    /// Bigest machine consist of two sub-machines: PlayerStateMachine and BoardStateMachine
    /// </summary>
    public class GameStateMachine : IStateMachine
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private IGameState currentState;// Also mean IState (because it was inherited from IState)

        private List<IGameState> states;

        private PlayerStateMachine[] players;// One is sub-machine

        private BoardStateMachine boardMachine;// One is sub-machine

        public GameStateMachine()
        {
            _initialize();
        }

        private void _initialize()
        {
            IGameState initializing = new GameInitializingState(this);
            IGameState playing = new GamePlayingState(this);
            IGameState played = new GamePlayedState(this);
            IGameState redoing = new GameRedoingState(this);
            IGameState redo = new GameRedoState(this);
            IGameState undoing = new GameUndoingState(this);
            IGameState undoed = new GameUndoedState(this);
            IGameState ended = new GameEndedState(this);

            // From intitializing
            initializing.getTransitionableState().Add(GameInitializedEvent.NAME, playing);

            // From playing
            playing.getTransitionableState().Add(GamePlayedEvent.NAME, played);
            playing.getTransitionableState().Add(GameUndoEvent.NAME, undoing);

            // From played
            played.getTransitionableState().Add(GameEndEvent.NAME, ended);
            played.getTransitionableState().Add(GameUndoEvent.NAME, undoing);

            // From undoing
            undoing.getTransitionableState().Add(GameUndoedEvent.NAME, undoed);

            // From undoed
            undoed.getTransitionableState().Add(GamePlayEvent.NAME, playing);
            undoed.getTransitionableState().Add(GameRedoEvent.NAME, redoing);

            // From redoing
            redoing.getTransitionableState().Add(GameRedoedEvent.NAME, redo);

            // From redo
            redo.getTransitionableState().Add(GamePlayEvent.NAME, playing);
            redo.getTransitionableState().Add(GameUndoEvent.NAME, undoed);

            this.currentState = initializing;
            this.currentState.run(null);
        }

        public bool consumeEvent(IEvent e)
        {
            if (currentState.getTransitionableState().Keys.Contains(e.getEventName()))
            {
                LOG.Info("Consuming game event name=" + e.getEventName());
                this.currentState = (IGameState)this.currentState.getTransitionableState()[e.getEventName()];
                this.currentState.run(e);
                return true;
            }
            else if (currentState.isSubmachineEvent(e))
            {
                // Transport event to submachine like PlayerMachine or BoardMachine
                LOG.Info("Transport submachine event with name=" + e.getEventName());
                try
                {
                    currentState.submachineConsumeEvent(e);
                }
                catch (EventNotAcceptableException ex)
                {
                    return false;
                }
                return true;
            }
            else
            {
                // Reject unvalid event
                LOG.Info("Unvalid event in current state (" + this.currentState.getStateName() + ") event name=" + e.getEventName());
                return false;
            }

        }

        public void fireStateChangedNotification(INotify notify)
        {
            LOG.Info("Received notification '" + notify.GetType() + "'");
            if (notify.GetType().Equals(typeof(PlayerReadyNotify)))
            {
                bool allReady = true;
                for (int i = 0; i < 2; i++)
                {
                    if (!this.players[i].getCurrentState().GetType().Equals(typeof(PlayerReadyState)))
                    {
                        allReady = false;
                        break;
                    }
                }
                if (allReady)
                {
                    LOG.Info("All players is ready");
                    for (int i = 0; i < 2; i++)
                    {
                        PlayerPlayEvent playerPlayEvent = new PlayerPlayEvent();
                        this.players[i].consumeEvent(playerPlayEvent);
                    }
                    int first = 1; // Hard code
                    this.players[1 - first].consumeEvent(new PlayerWaitEvent());

                    GameInitializedEvent _initializedEvent = new GameInitializedEvent();
                    this.consumeEvent(_initializedEvent);
                }
                else
                {
                    LOG.Info("All player not ready");
                }
            }
            else if (notify.GetType().Equals(typeof(BoardMovedNotify)))
            {
                for (int i = 0; i < 2; i++)
                {
                    if (this.players[i].getCurrentState().GetType().Equals(typeof(PlayerPlayingState)))
                    {
                        LOG.Info("Change player " + i + "  Playing to Waiting");
                        this.players[i].consumeEvent(new PlayerWaitEvent());
                    }
                    else if (this.players[i].getCurrentState().GetType().Equals(typeof(PlayerWaitingState)))
                    {
                        LOG.Info("Change player " + i +" Waiting to Turning");
                        this.players[i].consumeEvent(new PlayerTurnEvent());
                    }
                    else
                    {
                        LOG.Info("Current state: " + players[i].getCurrentState().GetType());
                    }
                }
            }
        }



        public PlayerStateMachine[] getPlayers()
        {
            return this.players;
        }

        public IGameState getCurrentState()
        {
            return this.currentState;
        }

        public void setPlayers(PlayerStateMachine[] players)
        {
            this.players = players;
        }

        public BoardStateMachine getBoardMachine()
        {
            return this.boardMachine;
        }

        public void setBoardMachine(BoardStateMachine boardMachine)
        {
            this.boardMachine = boardMachine;
        }
    }
}
