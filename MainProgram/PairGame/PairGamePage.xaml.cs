using DataAccess;
using GameController.PairGame;
using MainProgram.PickAndAskGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainProgram.PairGame
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class PairGamePage : Page
    {
        public PairGamePage(int CategoryID)
        {
            InitializeComponent();
            gameController = new PairGameController(this.gameGrid, CategoryID);
            list = gameController.getWordByCategory(CategoryID);
            NewGame(16);
        }
        PairGameController gameController;
        public static GameState gameState = GameState.Running;
        public static int Level = 1;
        public List<Word> list; // Word List
        public void NewGame(int cardNumber)
        {
            gameState = GameState.Running;
            gameGrid.Children.Clear();
           
            //gameGrid.Background = SetGameBackground();
            int row=2, column=2;
            
            if (cardNumber == 8)
            {
                row = 2; column = 4;
            }
            if (cardNumber == 16)
            {
                row = 4; column = 4;
            }
            if (cardNumber == 32)
            {
                row = 4; column = 8;
            }
            if (cardNumber == 4)
            {
                row = 8; column = 8;
            }
            // InitiailizeGamePanel(row,column);
            for (int i = 0; i < row; i++)
            {
                RowDefinition r = new System.Windows.Controls.RowDefinition();
                r.Height = new GridLength(85, GridUnitType.Star);
                gameGrid.RowDefinitions.Add(r);
            }
                
            for (int j = 0; j < column; j++)
            {
                ColumnDefinition col = new System.Windows.Controls.ColumnDefinition();
                col.Width = new GridLength(135, GridUnitType.Star);
                gameGrid.ColumnDefinitions.Add(col);
            }
                
            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                    CreateRectangle(i,j);
            gameController.StartGame(cardNumber);
            //StartTimer();
        }

        //private void StartTimer()
        //{
        //    DoubleAnimation daValueProperty = new DoubleAnimation();
        //    daValueProperty.From = 200;
        //    daValueProperty.To = 0;
        //    daValueProperty.Duration = TimeSpan.FromSeconds(200);
        //    daValueProperty.SetValue(Storyboard.TargetNameProperty, "progressBarTimeLeft");
        //    daValueProperty.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Value"));

        //    ColorAnimation caForegroundProperty = new ColorAnimation();
        //    caForegroundProperty.To = Colors.Red;
        //    caForegroundProperty.BeginTime = TimeSpan.FromSeconds(170);
        //    caForegroundProperty.SetValue(Storyboard.TargetNameProperty, "progressBarTimeLeft");
        //    caForegroundProperty.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Foreground.Color"));

        //    DoubleAnimation daOpacityProperty = new DoubleAnimation();
        //    daOpacityProperty.From = 1;
        //    daOpacityProperty.To = 0;
        //    daOpacityProperty.Duration = TimeSpan.FromMilliseconds(900);
        //    daOpacityProperty.AutoReverse = true;
        //    daOpacityProperty.RepeatBehavior = RepeatBehavior.Forever;
        //    daOpacityProperty.BeginTime = TimeSpan.FromSeconds(185);
        //    daOpacityProperty.SetValue(Storyboard.TargetNameProperty, "progressBarTimeLeft");
        //    daOpacityProperty.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Opacity"));


        //    App.TimerStoryboard = new Storyboard();
        //    App.TimerStoryboard.Children.Add(daValueProperty);
        //    App.TimerStoryboard.Children.Add(caForegroundProperty);
        //    App.TimerStoryboard.Children.Add(daOpacityProperty);
        //    App.TimerStoryboard.Duration = TimeSpan.FromSeconds(200);

        //    //sb.Completed += new EventHandler(GameOver);

        //    App.TimerStoryboard.Begin(gameGrid, true);

        //}
        private void InitiailizeGamePanel(int row,int column)
        {
           // int pair = cardNumber / 2;
            gameGrid.MaxWidth = 1080;
            gameGrid.MaxHeight = 680;
            gameGrid.Width = row * 90;
            gameGrid.Height = column * 140;
        }

        //private ImageBrush SetGameBackground()
        //{
        //    ImageBrush Background = new ImageBrush();
        //    System.Windows.Controls.Image image = new System.Windows.Controls.Image();
        //    image.Source = new BitmapImage(new Uri(
        //           String.Format("../../Images/Animals/Background{0}.jpg", Level), UriKind.Relative));
        //    Background.ImageSource = image.Source;
        //    return Background;
        //}
        public void CreateRectangle(int i,int j)
        {
            Rectangle rec = new Rectangle();
            rec.Height=140;
            rec.Width =90;
            rec.HorizontalAlignment = HorizontalAlignment.Center;
            rec.VerticalAlignment = VerticalAlignment.Center;
            rec.Stroke = new SolidColorBrush(Colors.Black);
            rec.StrokeThickness = 2;
            // Set rectangle style
            Style style = this.FindResource("RectangleStyle") as Style;
            
            rec.Style = style;
            // Set rectangle position inside the grid
            gameGrid.Children.Add(rec);

            Grid.SetRow(rec, i);
            Grid.SetColumn(rec, j);
        }


        private void rectangleLeftMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            Rectangle rec = sender as Rectangle;
            if (rec == null) return;
            gameController.PickCard(rec);
            if (gameController.gameState == GameState.GameOver)
                NextLevel();
        }

        public void NextLevel()
        {
            //if (MessageBox.Show("Next level ?", "Notification", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            //{
            //    NewGame(16);
            //}
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new PickAndAskGamePage(list));
        }

        private void gameGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            InitiailizeGamePanel(4,4);
        }
    }
}
