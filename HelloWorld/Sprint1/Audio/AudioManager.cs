using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint1.Trackers;
using System.Diagnostics;

namespace Sprint1.Audio
{
    public class AudioManager
    {
        private Song soundtrack;
        private IDictionary<string, SoundEffect> SoundEffectMap = new Dictionary<string, SoundEffect>();
        public bool IsMuted { get; set; }
        public AudioManager()
        {
            IsMuted = false;
            Console.WriteLine("isSoundEffectMuted + " + SoundEffect.MasterVolume);
        }

        public void LoadAudioFiles(ContentManager contentManager)
        {
            soundtrack = contentManager.Load<Song>("music1");

            SoundEffect breakblock = contentManager.Load<SoundEffect>("soundEffects/smb_breakblock");
            SoundEffect bump = contentManager.Load<SoundEffect>("soundEffects/smb_bump");
            SoundEffect coin = contentManager.Load<SoundEffect>("soundEffects/smb_coin");
            //SoundEffect gameover = contentManager.Load<SoundEffect>("soundEffects/smb_gameover");
            SoundEffect jumpsuper = contentManager.Load<SoundEffect>("soundEffects/smb_jump-super");
            SoundEffect jumpsmall = contentManager.Load<SoundEffect>("soundEffects/smb_jumpsmall");
            SoundEffect mariodie = contentManager.Load<SoundEffect>("soundEffects/smb_mariodie");
            SoundEffect pipe = contentManager.Load<SoundEffect>("soundEffects/smb_pipe");
            SoundEffect powerup = contentManager.Load<SoundEffect>("soundEffects/smb_powerup");
            SoundEffect powerup_appears = contentManager.Load<SoundEffect>("soundEffects/smb_powerup_appears");
            SoundEffect stomp = contentManager.Load<SoundEffect>("soundEffects/smb_stomp");
            //---------------------
            SoundEffect failToSteal = contentManager.Load<SoundEffect>("soundEffects/fail_to_steal");
            SoundEffect gameover = contentManager.Load<SoundEffect>("soundEffects/game_over");
            SoundEffect steal = contentManager.Load<SoundEffect>("soundEffects/steal");

            SoundEffectMap.Add("breakblock", breakblock); // done
            SoundEffectMap.Add("bump", bump); // done
            SoundEffectMap.Add("coin", coin); // done !
            SoundEffectMap.Add("jumpsuper", jumpsuper); //done
            SoundEffectMap.Add("jumpsmall", jumpsmall); //done
            SoundEffectMap.Add("mariodie", mariodie); //done
            SoundEffectMap.Add("pipe", pipe); //in progress!!
            SoundEffectMap.Add("powerup", powerup); //done
            SoundEffectMap.Add("powerup_appears", powerup_appears); // done
            SoundEffectMap.Add("stomp", stomp); //done

            SoundEffectMap.Add("gameover", gameover);
            SoundEffectMap.Add("failToSteal", failToSteal);
            SoundEffectMap.Add("steal", steal);
        }

        public void PlaySoundtrack()
        {
            MediaPlayer.Play(soundtrack);
            MediaPlayer.Volume = 0.5f;
            MediaPlayer.IsRepeating = true;
        }

        /*
         * Mutes or un-mutes the game LOOK AT THIS
         */
        public void Mute()
        {
            IsMuted = !IsMuted;
            MediaPlayer.IsMuted = IsMuted;
            SoundEffect.MasterVolume = IsMuted ? 0 : 1;
        }

        public void PlaySoundEffect(object o, SoundEffectEventArgs a)
        {
            if (SoundEffectMap.ContainsKey(a.effect))
            {
                try
                {
                    SoundEffectMap[a.effect].Play();
                }
                catch(InstancePlayLimitException e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }           
        }
    }

    public class SoundEffectEventArgs : EventArgs
    {
        public String effect { get; set; }
    }
}
