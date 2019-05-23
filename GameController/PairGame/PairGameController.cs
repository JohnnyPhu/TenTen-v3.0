using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Threading;
using DataAccess;
using System.IO;

namespace GameController.PairGame
{
    public class PairGameController
    {


        //protected GameState state = GameState.Running;

        protected List<Card> gameCards;
        private List<Card> cardMemory = new List<Card>();
        private List<Word> wordList;
        private List<DataAccess.Image> imgList;
        private Stack<KeyValuePair<Card, Rectangle>> candidateStack = new Stack<KeyValuePair<Card, Rectangle>>();
        protected Random random = new Random(DateTime.Now.Millisecond);
        protected Grid gameGrid;

        public GameState gameState;
        public SoundController soundController = new SoundController();
        WordDAL wordDAL;
        ImageDAL imgDAL;
        List<Word> list;
        int CategoryID;
        public PairGameController(Grid grid,int CategoryID)
        {
            if (grid == null) throw new ArgumentNullException("gameGrid can not be null.");
            // if (String.IsNullOrEmpty(iconSet)) throw new ArgumentException("iconSet can not be null or empty.");

            this.gameGrid = grid;
            // this.iconSet = iconSet;
            wordDAL = new WordDAL();
            imgDAL = new ImageDAL();
            this.CategoryID = CategoryID;
            list = getWordByCategory(CategoryID);
        }

        public List<Word> getWordByCategory(int CategoryID)
        {
            return wordDAL.getWordByCategory(CategoryID);
        }

        public void StartGame(int cardNumbers)
        {
            getWordList(CategoryID);
            gameGrid.Children.OfType<Rectangle>().ToList().ForEach(rec => rec.DataContext = null);
            soundController.Play(SoundType.Pop);
            int pair = cardNumbers / 2;
            List<Card> initialCards = CreateCards(pair);
            gameCards = AssignCardsTogameGrid(gameGrid, cardNumbers, initialCards);
            gameState = GameState.Running;
        }

        public void getWordList(int Category)
        {
            if(Category !=0)
            {
                List<Word> lst = list;
                wordList = new List<Word>();
                Random rand = new Random();
                for (int i = 0; i < 8; i++)
                {
                    int toSkip = rand.Next(0, list.Count);
                    Word w = list.Where(x => x.CategoryID == CategoryID).Skip(toSkip).Take(1).First();
                    lst.Remove(w);
                    wordList.Add(w);
                }
            }
        }

        public void getImageList()
        {
            if(wordList.Count>0)
            {
                imgList = imgDAL.getImageOfWordList(wordList);
            }
        }

        protected void PushCardOnCandidateStack(Rectangle cardRectangle)
        {
            candidateStack.Push(new KeyValuePair<Card, Rectangle>((Card)cardRectangle.DataContext, cardRectangle));
        }

        protected int CardsOnStack
        {
            get
            {
                return candidateStack.Count;
            }
        }

        private List<Card> AssignCardsTogameGrid(Grid gameGrid, int cardNumbers, List<Card> initialCardCollection)
        {
            List<Card> gameCardCollection = new List<Card>();

            foreach (Rectangle rectangle in gameGrid.Children)
            {
                //Rectangle rectangle = gameGrid.Children.OfType<Rectangle>().Single(
                //    x =>
                //        (int)x.GetValue(Grid.RowProperty) == row &&
                //        (int)x.GetValue(Grid.ColumnProperty) == col);

               
                int randomCardNumber = random.Next(0, initialCardCollection.Count);
                Card card = initialCardCollection[randomCardNumber];

                gameCardCollection.Add(card);
                rectangle.Name = card.Name.ToString();
                rectangle.DataContext = card;

                initialCardCollection.RemoveAt(randomCardNumber);
                Wait(200);
            }

            return gameCardCollection;
        }

        public void PickCard(Rectangle cardRectangle)
        {

            //  if (state == GameState.GameOver) return;

            Card card = cardRectangle.DataContext as Card;

            // Check if this card can still be played.
            if (card.Status != CardState.Covered)
            {
                return;
            }

            //Inform 

            soundController.Play(SoundType.Flip);
            FlipCard(cardRectangle);

            PushCardOnCandidateStack(cardRectangle);


            if (CardsOnStack == 2)
            {
                string cardName = candidateStack.Peek().Key.Name;
                bool isMatch = CheckIfCardsOnCandiateStackMatch();

                if (isMatch)
                {
                    OnMatch(cardName);
                }
                else
                {
                    OnMiss();
                }
            }


            if (!gameCards.Exists(c => c.Status == CardState.Covered))
            {
                
                //soundController.Play(SoundType.GameOver);
                gameState = GameState.GameOver;
                return;
            }
        }

        protected virtual void OnGameStart()
        {
            //if (App.TimerStoryboard != null)
            //{
            //    App.TimerStoryboard.Stop(gameGrid);

            //}
            //Nothing else to do here. A child can overide this.
        }

        protected virtual void OnCardPicked(Card card)
        {
            if (cardMemory.Contains(card)) return;

            if (cardMemory.Count() > 6)
            {
                cardMemory.RemoveAt(random.Next(0, cardMemory.Count));
            }

            cardMemory.Add(card);
        }

        protected virtual void OnMatch(string cardName)
        {


            //Remove card from the cards list
            RemoveCardsFromMemory(cardName);

            //Remove the rectangle that show the card on grid
            Rectangle rec1 = System.Windows.LogicalTreeHelper.FindLogicalNode(gameGrid, cardName) as Rectangle;
            gameGrid.Children.Remove(rec1);
            Rectangle rec2 = System.Windows.LogicalTreeHelper.FindLogicalNode(gameGrid, cardName) as Rectangle;
            gameGrid.Children.Remove(rec2);
            PlayThePronunciation(cardName);
            Wait(3000);
        }

        private void PlayThePronunciation(string word)
        {
            try
            {
                byte[] sound = wordList.Single(x => x.Word1 == word).Pronunciation.ToArray();
                MemoryStream ms = new MemoryStream(sound);
                SoundPlayer player = new SoundPlayer(ms);
                player.Play();
            }
            catch { }
        }

        private void RemoveCardsFromMemory(string cardName)
        {
            cardMemory.RemoveAll(card => card.Name == cardName);
        }

        protected void OnMiss()
        {
            // activate when two selected cards are not match
            //minute point or somethings. Save for further development
        }

        protected bool CheckIfCardsOnCandiateStackMatch()
        {
            var evalResult = EvalCards();

            if (evalResult.Key) // It is a match
            {
                //soundController.Play(SoundType.Match);
                
                MatchCard(evalResult.Value[0]);
                MatchCard(evalResult.Value[1]);
                return true;
            }
            else // No match
            {
                soundController.Play(SoundType.Flip);
                FlipCard(evalResult.Value[0]);
                FlipCard(evalResult.Value[1]);
                return false;
            }
        }

        protected void Wait(long milliseconds)
        {

            long dtEnd = DateTime.Now.AddTicks(milliseconds * 1000).Ticks;

            while (DateTime.Now.Ticks < dtEnd)
            {
                Grid g = new Grid();
                g.Dispatcher.Invoke(DispatcherPriority.Background, (DispatcherOperationCallback)delegate (object unused) { return null; }, null);
            }

        }

        protected KeyValuePair<bool, List<Rectangle>> EvalCards()
        {
            var a = candidateStack.Pop();
            var b = candidateStack.Pop();

            if (a.Key.Name == b.Key.Name) // Match Found
            {
                Wait(3500);
                return new KeyValuePair<bool, List<Rectangle>>(true, new List<Rectangle>() { a.Value, b.Value });
            }
            else // No Match
            {
                Wait(6000);

                return new KeyValuePair<bool, List<Rectangle>>(false, new List<Rectangle>() { a.Value, b.Value });
            }

        }


        protected void FlipCard(Rectangle cardRectangle)
        {
            Card card = cardRectangle.DataContext as Card;

            FlipCardRectangle(cardRectangle, 1, 0);

            if (card.Status == CardState.Covered)
            {
                card.Uncover();
            }
            else
            {
                card.Cover();
            }

            FlipCardRectangle(cardRectangle, 0, 1);
        }

        protected List<Card> CreateCards(int pairs)
        {
            gameState = GameState.Running;
            List<Card> cards = new List<Card>();

            BitmapImage backgroundImage = GetBackImage();
            getImageList();
            //for (int x = 0; x < pairs; x++)
            //{

            //    BitmapImage frontImage = GetImage(String.Format("../../Images/Animals/Animal{0}.jpg", x));
            //    Card a = new Card("Animal" + x.ToString(), frontImage, backgroundImage);

            //    frontImage = GetImage(String.Format("../../Images/Animals/Animal{0}.jpg", x));
            //    Card b = new Card("Animal" + x.ToString(), frontImage, backgroundImage);

            //    cards.Add(a);
            //    cards.Add(b);

            //}

            foreach(Word w in wordList)
            {
                DataAccess.Image img = imgList.Where(x => x.WordID == w.Word1).SingleOrDefault();
                byte[] i = img.Image1.ToArray(); 
                if (img == null) break;
                BitmapImage frontImage = GetImage(i);
                Card a = new Card(w.Word1, frontImage, backgroundImage);
                Card b = new Card(w.Word1, frontImage, backgroundImage);

                cards.Add(a);
                cards.Add(b);
            }
            return cards;
        }

        protected void MatchCard(Rectangle rectangle)
        {
            Card card = rectangle.DataContext as Card;
            card.Match();
        }

        private BitmapImage GetBackImage()
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = Application.GetResourceStream(new Uri("../../Images/CardBackground.png", UriKind.Relative)).Stream;
            image.EndInit();
            return image;
        }

        private BitmapImage GetImage(byte[] array)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            var ms = new System.IO.MemoryStream(array);
            //image.StreamSource = Application.GetResourceStream(new Uri(path, UriKind.Relative)).Stream;
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        private void FlipCardRectangle(Rectangle cardRectangle, int from, int to)
        {
            cardRectangle.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform flipTrans = new ScaleTransform();
            flipTrans.ScaleY = 1;
            cardRectangle.RenderTransform = flipTrans;

            DoubleAnimation da = new DoubleAnimation();
            da.From = from;
            da.To = to;
            da.Duration = TimeSpan.FromMilliseconds(200);

            flipTrans.BeginAnimation(ScaleTransform.ScaleYProperty, da);
        }
    }
}
