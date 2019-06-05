using DataAccess;
using MainProgram.UI.MainPage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainProgram.LearningWord
{
    /// <summary>
    /// Interaction logic for LearningWordPage.xaml
    /// </summary>
    public partial class LearningWordPage : Page
    {
        WordDAL wdal;
        ImageDAL idal;
        private int _CategoryID = 0;
        private int _index = 0;
        public List<Word> lstW;
        private Word _currentW;
        public LearningWordPage(int CategoryID)
        {
            InitializeComponent();
            wdal = new WordDAL();
            idal = new ImageDAL();
            _CategoryID = CategoryID;
            //LoadRandomWordByCategory(_CategoryID);
            //LoadRandomListWordByCategory(_CategoryID);
            lstW = new List<Word>();
            lstW = wdal.getRandomWordForLearningWord(_CategoryID);

            //lấy từ đầu tiên trong danh sách
            _currentW = lstW[0];
            lblWord.Content = lstW[0].Word1;
            lblWord.HorizontalContentAlignment = HorizontalAlignment.Center;
            lblWord.VerticalContentAlignment = VerticalAlignment.Center;
            lblTranslate.Content = lstW[0].Translate;
            lblTranslate.HorizontalContentAlignment = HorizontalAlignment.Center;
            lblTranslate.VerticalContentAlignment = VerticalAlignment.Center;
            DataAccess.Image img = idal.getImageOfWord(lstW[0].Word1);
            byte[] i = img.Image1.ToArray();
            if (img == null) throw new Exception("#113 Từ " + lstW[0].Word1 + "chưa có hình.");
            BitmapImage image = LoadImage(i);
            img_word.Source = image;
            img_word.Stretch = Stretch.Fill;
            PlayThePronunciation(lstW[0]);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (_index < lstW.Count - 1)
            {
                _index += 1;
                _currentW = lstW[_index];
                lblWord.Content = lstW[_index].Word1;
                lblTranslate.Content = lstW[_index].Translate;
                //convert image from binary to image
                DataAccess.Image img = idal.getImageOfWord(lstW[_index].Word1);
                byte[] i = img.Image1.ToArray();
                if (img == null) throw new Exception("#113 Từ " + lstW[_index].Word1 + "chưa có hình.");
                BitmapImage image = LoadImage(i);
                img_word.Source = image;
                //play audio of word
                PlayThePronunciation(lstW[_index]);

            }
        }

        private static BitmapImage LoadImage(byte[] imageData)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            var ms = new System.IO.MemoryStream(imageData);
            //image.StreamSource = Application.GetResourceStream(new Uri(path, UriKind.Relative)).Stream;
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }



        private void BtnPlayAudio_Click(object sender, RoutedEventArgs e)
        {
            if (_currentW != null)
            {
                PlayThePronunciation(_currentW);
            }
        }


        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (_index > 0)
            {
                _index -= 1;
                _currentW = lstW[_index];
                lblWord.Content = lstW[_index].Word1;
                lblTranslate.Content = lstW[_index].Translate;
                DataAccess.Image img = idal.getImageOfWord(lstW[_index].Word1);
                byte[] i = img.Image1.ToArray();
                if (img == null) throw new Exception("#113 Từ " + lstW[_index].Word1 + "chưa có hình.");
                BitmapImage image = LoadImage(i);
                img_word.Source = image;
                PlayThePronunciation(lstW[_index]);
            }
        }
        private void PlayThePronunciation(Word word)
        {
            try
            {
                byte[] sound = wdal.getWord(word.Word1).Pronunciation.ToArray();
                MemoryStream ms = new MemoryStream(sound);
                SoundPlayer player = new SoundPlayer(ms);
                player.Play();
            }
            catch { }
        }

        //private void SplitWord(string wordsplling)
        //{
            
        //}
    }
}
