using System;
using System.Timers;
using Microsoft.Xna.Framework.Media;
using Sprint1.Audio;
using Sprint1.Entities;
using Sprint1.Yannstempclasses;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using Sprint1.LevelLoader;
using System.Diagnostics;

namespace Sprint1.Trackers
{
    public class RoundTracker
    {
        public int marioWinningRounds { get; private set; }
        public int luigiWinningRounds { get; private set; }
        public int totalRounds { get; private set; }
        private System.Timers.Timer timer;
        private System.Timers.Timer winTimer;

        private MapPicker mapPicker;
        private List<string> mapList;

        public event EventHandler<SoundEffectEventArgs> SetEffect;
        SoundEffectEventArgs SoundEffectArgs;

        public event EventHandler<EventArgs> AllTimerStop;

        private Game1 reciever;

        public RoundTracker(Game1 game)
        {
            marioWinningRounds = 0;
            luigiWinningRounds = 0;
            totalRounds = 1;
            winTimer = new System.Timers.Timer(3000);

            mapList = new List<string>();
            mapList.Add("RedGiant.xml");
            mapList.Add("IceAge.xml");
            mapList.Add("DeepSpace.xml");

            mapPicker = new MapPicker(mapList);

            timer = new System.Timers.Timer(2000); //interval is 2s
            timer.Elapsed += PlayEffect;
            winTimer.Elapsed += WinScreen;
            

            reciever = game;

            this.SetEffect += reciever.audioManager.PlaySoundEffect;
        }

        public void MarioWon(object o, EventArgs e)
        {
            marioWinningRounds++;
            if (marioWinningRounds == 2)
            {
               
                timer.Start();
            }

            totalRounds++;

            if (luigiWinningRounds == 0 && marioWinningRounds == 0)
            {
                reciever.roundTransition.displayed00 = false;
            }
            else if (luigiWinningRounds == 0 && marioWinningRounds == 1)
            {
               // onAllTimerStop();
               // Console.WriteLine(mapPicker.Next());
               // reciever.LoadLevel(mapPicker.Next()); 
                reciever.roundTransition.displayed01 = false;

                //reciever.ResetGame();
                //reciever.isGameOver = false;
                //reciever.isWon = false;
                //reciever.timeTracker.time = 0;
            }
            else if (luigiWinningRounds == 1 && marioWinningRounds == 0)
            {
                reciever.roundTransition.displayed10 = false;
            }
            else if (luigiWinningRounds == 1 && marioWinningRounds == 1)
            {
                reciever.roundTransition.displayed11 = false;
            }
            else if (luigiWinningRounds == 1 && marioWinningRounds == 2)
            {
                reciever.roundTransition.displayed12 = false;
                Console.WriteLine("(from RoundTracker) Winning screen");
                onAllTimerStop();
                winTimer.Start();
            }
            else if (luigiWinningRounds == 2 && marioWinningRounds == 1)
            {
                reciever.roundTransition.displayed21 = false;
                Console.WriteLine("(from RoundTracker) Winning screen");
                onAllTimerStop();
                winTimer.Start();
            }
            else if (luigiWinningRounds == 2 && marioWinningRounds == 0)
            {
                reciever.roundTransition.displayed20 = false;
                Console.WriteLine("(from RoundTracker) Winning screen");
                onAllTimerStop();
                winTimer.Start();
            }
            else if (luigiWinningRounds == 0 && marioWinningRounds == 2)
            {
                reciever.roundTransition.displayed02 = false;
                Console.WriteLine("(from RoundTracker) Winning screen");
                onAllTimerStop();
                winTimer.Start();
               // reciever.isLuigiWon = false;
                //reciever.isMarioWon = true;
            }
            onAllTimerStop();
            reciever.progressionBar.marioHoldTime = 0;
            reciever.progressionBarLuigi.luigiHoldTime = 0;
            reciever.NewRound();
            //Console.WriteLine("(from RoundTracker)roundTransition.displayed00 is: " + reciever.roundTransition.displayed00);
            //Console.WriteLine("(from RoundTracker)roundTransition.displayed01 is: " + reciever.roundTransition.displayed01);
            //Console.WriteLine("(from RoundTracker)roundTransition.displayed10 is: " + reciever.roundTransition.displayed10);
            //Console.WriteLine("(from RoundTracker)roundTransition.displayed11 is: " + reciever.roundTransition.displayed11);
            //Console.WriteLine("(from RoundTracker)roundTransition.displayed20 is: " + reciever.roundTransition.displayed20);
            //Console.WriteLine("(from RoundTracker)roundTransition.displayed02 is: " + reciever.roundTransition.displayed02);
            //Console.WriteLine("(from RoundTracker)roundTransition.displayed21 is: " + reciever.roundTransition.displayed21);
            //Console.WriteLine("(from RoundTracker)roundTransition.displayed12 is: " + reciever.roundTransition.displayed12);
            
        }

        public void LuigiWon(object o, EventArgs e)
        {
            luigiWinningRounds++;
            if (luigiWinningRounds == 2)
            {
                timer.Start();
            }
            totalRounds++;
            if (luigiWinningRounds == 0 && marioWinningRounds == 0)
            {
                reciever.roundTransition.displayed00 = false;
            }
            else if (luigiWinningRounds == 0 && marioWinningRounds == 1)
            {
                reciever.roundTransition.displayed01 = false;
            }
            else if (luigiWinningRounds == 1 && marioWinningRounds == 0)
            {
                reciever.roundTransition.displayed10 = false;
            }
            else if (luigiWinningRounds == 1 && marioWinningRounds == 1)
            {
                reciever.roundTransition.displayed11 = false;
            }
            else if (luigiWinningRounds == 1 && marioWinningRounds == 2)
            {
                reciever.roundTransition.displayed12 = false;
                Console.WriteLine("(from RoundTracker) Winning screen");
                onAllTimerStop();
                winTimer.Start();
            }
            else if (luigiWinningRounds == 2 && marioWinningRounds == 1)
            {
                reciever.roundTransition.displayed21 = false;
                Console.WriteLine("(from RoundTracker) Winning screen");
                onAllTimerStop();
                winTimer.Start();
            }
            else if (luigiWinningRounds == 2 && marioWinningRounds == 0)
            {
                reciever.roundTransition.displayed20 = false;
                Console.WriteLine("(from RoundTracker) Winning screen");
                onAllTimerStop();
                winTimer.Start();
            }
            else if (luigiWinningRounds == 0 && marioWinningRounds == 2)
            {
                reciever.roundTransition.displayed02 = false;
                Console.WriteLine("(from RoundTracker) Winning screen");
                onAllTimerStop();
                winTimer.Start();
            }
            onAllTimerStop();
            reciever.progressionBar.marioHoldTime = 0;
            reciever.progressionBarLuigi.luigiHoldTime = 0;
            reciever.NewRound();
            //reciever.ResetGame();
            //Console.WriteLine("(from RoundTracker)roundTransition.displayed00 is: " + reciever.roundTransition.displayed00);
            //Console.WriteLine("(from RoundTracker)roundTransition.displayed01 is: " + reciever.roundTransition.displayed01);
            //Console.WriteLine("(from RoundTracker)roundTransition.displayed10 is: " + reciever.roundTransition.displayed10);
            //Console.WriteLine("(from RoundTracker)roundTransition.displayed11 is: " + reciever.roundTransition.displayed11);
            //Console.WriteLine("(from RoundTracker)roundTransition.displayed20 is: " + reciever.roundTransition.displayed20);
            //Console.WriteLine("(from RoundTracker)roundTransition.displayed02 is: " + reciever.roundTransition.displayed02);
            //Console.WriteLine("(from RoundTracker)roundTransition.displayed21 is: " + reciever.roundTransition.displayed21);
            //Console.WriteLine("(from RoundTracker)roundTransition.displayed12 is: " + reciever.roundTransition.displayed12);
        }

        public void PlayEffect(object o, ElapsedEventArgs e)
        {
            //reciever.audioManager.MediaPlayer.IsMuted
            MediaPlayer.IsMuted = true;
            SoundEffectArgs = new SoundEffectEventArgs { effect = "gameover" };
            onSetEffect(SoundEffectArgs);

            timer.Stop();
        }

        public void ResetAll()
        {
            marioWinningRounds = 0;
            luigiWinningRounds = 0;
            totalRounds = 0;
        }

        public void WinScreen(object o, ElapsedEventArgs e)
        {
            if (luigiWinningRounds==2)
            {
                reciever.isLuigiWon = true;
                reciever.isMarioWon = false;
            }
            else if (marioWinningRounds==2)
            {
                reciever.isLuigiWon = false;
                reciever.isMarioWon = true;
            }
            ResetAll();
            winTimer.Stop();
        }

        protected virtual void onAllTimerStop()
        {
            EventHandler<EventArgs> handler = AllTimerStop;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        protected virtual void onSetEffect(SoundEffectEventArgs e)
        {
            EventHandler<SoundEffectEventArgs> handler = SetEffect;
            if (handler != null)
                handler(this, e);
        }

    }


}