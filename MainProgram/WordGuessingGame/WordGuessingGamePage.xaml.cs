using DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
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

namespace MainProgram.WordGuessingGame
{
    /// <summary>
    /// Interaction logic for WordGuessingGamePage.xaml
    /// </summary>
    public partial class WordGuessingGamePage : Page
    {
        #region Parameter
        /// <summary>
        /// The length of a word
        /// </summary>
        private int wordLength;
        /// <summary>
        /// The current puzzle word to be solved
        /// </summary>
        private Word currentPuzzleWord;
        /// <summary>
        /// The list of words in the puzzle
        /// </summary>
        private int wordIndex;

        /// <summary>
        /// The list of words attempted in a game
        /// </summary>
        private List<string> history = new List<string>();

        /// <summary>
        /// The index of the last word added to the hisory list
        /// </summary>
        private int historyIndex;

      

        List<string> words = new List<string>();
        //private static GameState gameState = GameState.InitialDisplay;
        List<Word> list;
        private int currentAttempt = GameState.FirstAttempt;
        #endregion


        public WordGuessingGamePage(List<Word> list)
        {
            InitializeComponent();
            this.list = list;
            StartGame();
        }

        public void DisplayGamePanel(int wordLength)
        {
            InitiailizeGamePanel(wordLength);
            for (int i = 0; i < wordLength*5; i++)
            {
                CreateTextBox(i);
            }
            //ConfigTheFirstRow();
            ConfigGuessBox();
        }

        private void InitiailizeGamePanel(int wordLength)
        {
            GamePanel.Width = wordLength * 50;
            GamePanel.Height = wordLength * 70;
        }

        public void CreateTextBox(int i)
        {
            TextBox txt = new TextBox();
            txt.SetValue(Grid.ColumnSpanProperty, 1);
            txt.Width = 50;
            txt.Height = 50;
            txt.HorizontalContentAlignment = HorizontalAlignment.Center;
            txt.VerticalContentAlignment = VerticalAlignment.Center;
            txt.Margin = new Thickness(0, 0, 0, 4);
            txt.Padding = new Thickness(0);
            txt.Foreground = Brushes.Black;
            txt.MaxLength = 1;
            txt.FontSize = 20;
            txt.IsReadOnly = true;
            txt.Name = "txt" + i.ToString();
            // there are the suggestion row
            //if (i < wordLength)
            //{
            //    txt.Visibility = Visibility.Hidden;
            //    txt.Foreground = new SolidColorBrush(Colors.White);
            //    txt.Background = new SolidColorBrush(Colors.LightSteelBlue);
            //}
            GamePanel.Children.Add(txt);
        }

        public void ConfigGuessBox()
        {
            //if (currentAttempt == GameState.FirstAttempt)
            //{
            //    txtGuess.MaxLength = wordLength;
            //    txtGuess.FontSize = 15;
            //    //txtGuess.IsReadOnly = true;
            //    txtGuess.HorizontalContentAlignment = HorizontalAlignment.Center;
            //    txtGuess.VerticalContentAlignment = VerticalAlignment.Center;
            //    txtGuess.Text = "Press any key to start";
            //    txtGuess.KeyDown += new KeyEventHandler(txtGuess_KeyDown);
            //}
            //else
            //{
            //    // txtGuess.Clear();
            //    //  txtGuess.IsReadOnly = false;
            //}
            //txtGuess.Focus();
        }
        string shuffle;
        private void StartGame()
        {
            getPuzzleWord();
            //Random r = new Random(DateTime.Now.Millisecond);
            //for (int i = 0; i < wordLength; i++)
            //{
            //    TextBox txt = (TextBox)GamePanel.Children[i];
            //    txt.Clear();
            //}

            shuffle = Shuffle(currentPuzzleWord.Word1);
            //MessageBox.Show(shuffle);
            // Determine two random positions in the word to guess
            // NOTE: I had earlier used a naive approach to get the random
            // positions. However, that method meant randomPos2 = randomPos1 + 1
            // 20% of the time. The following snippet improves it to 16%.
            //int randomPos1 = r.Next(0, wordLength);
            //int randomPos2 = r.Next(0, wordLength - 1);
            //if (randomPos2 == randomPos1)
            //    randomPos2++;

            // Get the random word to guess making sure its of
            // the right difficulty
            //wordIndex = r.Next(0, words.Count);
            //while (!IsWordMatchForDifficultyLevel(words[wordIndex], gameDifficulty))
            //{
            //    wordIndex = r.Next(0, words.Count);
            //}
            //_currentPuzzleWord = words[wordIndex].Substring(2);

            // Reset the grid and display the inital random letters
            //ResetGrid();

            //TextBox txtFirstSuggest = System.Windows.LogicalTreeHelper.FindLogicalNode(GamePanel, "txt" + randomPos1) as TextBox;
            //TextBox txtSecondSuggest = System.Windows.LogicalTreeHelper.FindLogicalNode(GamePanel, "txt" + randomPos2) as TextBox;
            //txtFirstSuggest.Text = currentPuzzleWord[randomPos1].ToString();
            //txtSecondSuggest.Text = currentPuzzleWord[randomPos2].ToString();
            //txtFirstSuggest.Visibility = Visibility.Visible;
            //txtSecondSuggest.Visibility = Visibility.Visible;
            DisplayGamePanel(wordLength);
            CreateLetter();
        }

        private void getPuzzleWord()
        {
            Random rand = new Random();
            int toSkip = rand.Next(0, list.Count);
            currentPuzzleWord = list[toSkip];
            wordLength = currentPuzzleWord.Word1.Length;
            lblDescription.Content = currentPuzzleWord.Translate;
        }

        public string Shuffle(string str)
        {
            str = str.ToUpper();
            char[] array = str.ToCharArray();
            Random rng = new Random();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            return new string(array);
        }

        #region GameState
        public static class GameState
        {
            /// <summary>
            /// The game is diplaying the initial screen
            /// </summary>
          

            /// <summary>
            /// Indicates the first attempt at solving a word
            /// </summary>
            public const int FirstAttempt = 1;

            /// <summary>
            /// Indicates the last attempt at solving a word
            /// </summary>
           // public const int LastAttempt = wordLength;

            /// <summary>
            /// The maximum number of attempts
            /// </summary>
           // public const int MaximumAttemptsOver = wordLength;

            /// <summary>
            /// Indicates the original or solved word is displayed
            /// </summary>
            public const int ViewingWord = 6;
            #endregion

        }



        public void CreateLetter()
        {
            for(int i=0;i<shuffle.Length;i++)
            {
                char c = shuffle[i];

                Button btn = new Button() {
                    Name = c.ToString(),
                    Width = 50,
                    Height = 50,
                    Content = new System.Windows.Controls.Image
                    {
                        Source = new BitmapImage(new Uri(String.Format("../../Images/Alphabet/"+c+".JPG"), UriKind.Relative)),
                        VerticalAlignment = VerticalAlignment.Center
                    }
                };
                GuessLettersPanel.Children.Add(btn);
                btn.Click += new RoutedEventHandler(WordChoose_Click);
            }
        }

        int index=0; // The index letter of the guess word
        int count = 0; // The wrong guess count
        private void WordChoose_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

           
               
            if (index < wordLength)
            {
                TextBox txt;
                if (count==0)
                    txt = GamePanel.Children[index] as TextBox;
                else
                    txt = GamePanel.Children[index+count*wordLength] as TextBox;
                txt.Text = btn.Name;
                GuessLettersPanel.Children.Remove(btn);
                index++;
            }
            if (index == wordLength)
            {
                string guess = "";
                for (int i = count*wordLength; i < count * wordLength+wordLength; i++)
                {
                    TextBox txt = (TextBox)GamePanel.Children[i];
                    guess += txt.Text;
                }
                if (guess == currentPuzzleWord.Word1.ToUpper())
                {
                    for (int i = count * wordLength; i < count * wordLength + wordLength; i++)
                        setTextBoxGreenColor(i);
                    MessageBox.Show("Conglatulation");
                    GameOver(true);
                }
                else
                {
                    count++;
                    index = 0;
                    foreach (Control i in GuessLettersPanel.Children)
                        GuessLettersPanel.Children.Remove(i);
                    for (int i = 0; i < count * wordLength; i++)
                        setTextBoxGrayColor(i);
                    CreateLetter();
                    
                }
            }

            if (count == 5)
            {
                MessageBox.Show("you lose");
                GameOver(false);
                return;
            }
        }

        private void setTextBoxGreenColor(int i)
        {
            TextBox txt = (TextBox)GamePanel.Children[i];
            txt.Background = new SolidColorBrush(Colors.LightGreen);
        }

        private void setTextBoxGrayColor(int i)
        {
            TextBox txt = (TextBox)GamePanel.Children[i];
            txt.Background = new SolidColorBrush(Colors.DarkGray);
        }

        private void GameOver(bool win)
        {
            btnHome.Visibility = Visibility.Visible;
            Label lbl = new Label();
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            lbl.FontSize = 50;

            Button btnRefresh = new Button() {
                Width = 50,
                Height = 50,
                Content = new System.Windows.Controls.Image
                {
                    Source = new BitmapImage(new Uri(string.Format("../../Images/Refresh.png"), UriKind.Relative)),
                    VerticalAlignment = VerticalAlignment.Center
                }
            };

            btnRefresh.Click += new RoutedEventHandler(btnRefresh_Click);

            Button btnPrononciation = new Button()
            {
                Width = 50,
                Height = 50,
                Content = new System.Windows.Controls.Image
                {
                    Source = new BitmapImage(new Uri(String.Format("../../Images/play-icon.jpg"), UriKind.Relative)),
                    VerticalAlignment = VerticalAlignment.Center
                }
            };
            btnPrononciation.Click += new RoutedEventHandler(btnPrononciation_Click);
            
            if (win)
            {
                lbl.Content = "Conglatulation";
            }
            else
            {
                lbl.Content = "The correct word is " + currentPuzzleWord.Word1;
            }
            
            
            GuessLettersPanel.Children.Add(lbl);
            GuessLettersPanel.Children.Add(btnRefresh);
            GuessLettersPanel.Children.Add(btnPrononciation);
        }

        private void btnPrononciation_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] sound = currentPuzzleWord.Pronunciation.ToArray();
                MemoryStream ms = new MemoryStream(sound);
                SoundPlayer player = new SoundPlayer(ms);
                player.Play();
            }
            catch { }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            btnHome.Visibility = Visibility.Hidden;
            EmptyGuessPanel();
            EmptyGamePanel();
            StartGame();
        }

        private void EmptyGamePanel()
        {
            while (GamePanel.Children.Count > 0)
                GamePanel.Children.RemoveAt(0);
        }
        private void EmptyGuessPanel()
        {
            while (GuessLettersPanel.Children.Count > 0)
                GuessLettersPanel.Children.RemoveAt(0);
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            while (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
    }
}
