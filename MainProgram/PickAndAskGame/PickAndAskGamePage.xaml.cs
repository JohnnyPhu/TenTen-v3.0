using DataAccess;
using MainProgram.WordGuessingGame;
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

namespace MainProgram.PickAndAskGame
{
    /// <summary>
    /// Interaction logic for PickAndAskGamePage.xaml
    /// </summary>
    public partial class PickAndAskGamePage : Page
    {
        ImageDAL idal;
        WordDAL wdal;
        private int categoryId=1;
        List<Word> wordList;
        Word ChosenWord;
        Word RandomWord1;
        Word RandomWord2;
        int ChosenPosition;
        List<Word> list;
        int count = 0; // The time that player answer right
        public PickAndAskGamePage(List<Word> list)
        {
            InitializeComponent();
            idal = new ImageDAL();
            wdal = new WordDAL();
            this.list = list;
            NewGame();
        }

        public void NewGame()
        {
            if (count >= 5)
            {
                NavigationService ns = NavigationService.GetNavigationService(this);
                ns.Navigate(new WordGuessingGamePage(list));
            }
            wordList = new List<Word>();
            foreach (Word w in list)
                wordList.Add(w);
            ChosenWord = getRandomWord();
            RandomWord1 = getRandomWord();
            RandomWord2 = getRandomWord();
            lblChooseWord.Content = "Hãy chọn hình cho từ "+ChosenWord.Word1.ToString();
            Random ran = new Random();
            ChosenPosition = ran.Next(1,3);
            switch (ChosenPosition)
            {
                case 1:
                    {
                        setImageSource(image_1, ChosenWord);
                        setImageSource(image_2, RandomWord1);
                        setImageSource(image_3, RandomWord2);
                        setLabelPictureContent(lbl_Pic1, ChosenWord.Word1);
                        setLabelPictureContent(lbl_Pic2, RandomWord1.Word1);
                        setLabelPictureContent(lbl_Pic3, RandomWord2.Word1);
                        break;
                    }
                case 2:
                    {
                        setImageSource(image_1, RandomWord1);
                        setImageSource(image_2, ChosenWord);
                        setImageSource(image_3, RandomWord2);
                        setLabelPictureContent(lbl_Pic1, RandomWord1.Word1);
                        setLabelPictureContent(lbl_Pic2, ChosenWord.Word1);
                        setLabelPictureContent(lbl_Pic3, RandomWord2.Word1);
                        break;
                    }
                case 3:
                    {
                        setImageSource(image_1, RandomWord1);
                        setImageSource(image_2, RandomWord2);
                        setImageSource(image_3, ChosenWord);
                        setLabelPictureContent(lbl_Pic1, RandomWord1.Word1);
                        setLabelPictureContent(lbl_Pic2, RandomWord2.Word1);
                        setLabelPictureContent(lbl_Pic3, ChosenWord.Word1);
                        break;
                    }
            }
        }

        public Word getRandomWord()
        {
            if (wordList.Count > 0)
            {
                Random ran = new Random();
                int ChosenPosition = ran.Next(wordList.Count);
                Word word = wordList[ChosenPosition];
                wordList.Remove(word);
                return word;
            }
            else
                return null;
        }

        //public void LoadWordByCategory(int categoryId)
        //{
        //    Word word = wdal.geRandomtWordByCategory(categoryId);
        //    lblChooseWord.Content = "Hãy chọn hình cho từ " + word.Word1;
        //    DataAccess.Image img = idal.getImageOfWord(word.Word1);
        //    byte[] i = img.Image1.ToArray();
        //    if (img == null) throw new Exception("#113 Từ " + word.Word1 + "chưa có hình.");
        //    BitmapImage image = LoadImage(i);
        //    image_1.Source = image;
        //    //LoadImage(img.Image1);
        //}

        public void setImageSource(System.Windows.Controls.Image imgControl,Word word)
        {
            try {
                DataAccess.Image img = idal.getImageOfWord(word.Word1);
                if (img == null)
                {
                    return;
                }
                byte[] i = img.Image1.ToArray();
                BitmapImage image = LoadImage(i);
                imgControl.Source = image;
                imgControl.Stretch = Stretch.Fill;
            }
            catch { }
        }

        public void setLabelPictureContent(Label lblControl,string word)
        {
            lblControl.Content = word;
            lblControl.Visibility = Visibility.Hidden;
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

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string guess="";
            if (btn.Name == "btn_Pic1")
                guess = lbl_Pic1.Content.ToString();
            if (btn.Name == "btn_Pic2")
                guess = lbl_Pic2.Content.ToString();
            if (btn.Name == "btn_Pic3")
                guess = lbl_Pic3.Content.ToString();
            if (guess == RandomWord1.Word1)
                PlayThePronunciation(RandomWord1);
            if (guess == RandomWord2.Word1)
                PlayThePronunciation(RandomWord2);
            if (guess == ChosenWord.Word1)
            {
                PlayThePronunciation(ChosenWord);
                count++;
                NewGame();
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
    }
}
