using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.Windows;


namespace GameController.PairGame
{
    public class SoundController
    {
        private static SoundPlayer flipSound = new SoundPlayer();
        private static SoundPlayer matchSound = new SoundPlayer();
        private static SoundPlayer winSound = new SoundPlayer();
        private static SoundPlayer gameOverSound = new SoundPlayer();
        private static SoundPlayer popSound = new SoundPlayer();

        private Dictionary<SoundType, SoundPlayer> soundPlayerDic = new Dictionary<SoundType, SoundPlayer>()
        {
          {SoundType.Flip, flipSound},
          {SoundType.Match, matchSound},
          {SoundType.Win, winSound},
          {SoundType.GameOver, gameOverSound},
          {SoundType.Pop, popSound},
        };

        public SoundController()
        {try
            {
                flipSound.SoundLocation = "../../Sounds/flipCard.wav";
                flipSound.Load();

                matchSound.SoundLocation = "../../Sounds/match.wav";
                matchSound.Load();

                winSound.SoundLocation = "../../Sounds/Win.wav";
                winSound.Load();

                gameOverSound.SoundLocation = "../../Sounds/LevelComplete.wav";
                gameOverSound.Load();

                popSound.SoundLocation = "../../Sounds/pop.wav";
                popSound.Load();
            }
            catch(Exception ex) { throw new Exception(ex.Message); }
        }


        public void Play(SoundType soundType)
        {
            soundPlayerDic[soundType].Play();
        }

        //public void Play(byte[] sound)
        //{
        //    soundPlayerDic
        //}
    }
}
