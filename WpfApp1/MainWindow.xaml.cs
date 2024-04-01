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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
    
namespace WpfApp1
{
    using System.Windows.Threading;

    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /* pour créer un chrono + conserve le temps écoulé + nbr d'animaux validée */
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            SetUpGame();
        }

        /* modifie le nouveau TextBlock avec chrono et
         * arrête le chrono si tout les animeaux sont trouvé
          avec affichage du nbr de matchesFound */
        private void Timer_Tick(object sender, EventArgs e)
        {   
            //incrémenter le chrono
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text =  (tenthsOfSecondsElapsed / 10F).ToString("0.0S");
            timeTextBlock.Text += ("- Identiques: " + matchesFound);
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + "\n Nouveau jeux ?";
            }
           
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()
            {
                " 🦉 " , " 🦉 ",
                " 🦘 " , " 🦘 ",
                " 🦨 " , " 🦨 ",
                " 🦔 " , " 🦔 ",
                " 🐠 " , " 🐠 ",
                " 🦑 " , " 🦑 ",
                " 🐋 " , " 🐋 ",
                " 🐩 " , " 🐩 ",
            }; 
            Random random = new Random();
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;

                    //donner à index un nbr entre 0 et la fin du liste animalEmoji
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];

                    //aprés que textBlock reçoit index equivalent dans la liste
                    textBlock.Text = nextEmoji;

                    //en le supprime de la liste animalEmoji
                    animalEmoji.RemoveAt(index);
                }
            }
            /* démarrer le chrono et Réinitialiser les champs*/
            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;

        }

        TextBlock lastTextBlockClicked;
        bool findingMatch = false;
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;

            // pour la premiere clike ou findingMatch reste false
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden; 
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            // si user a  trouvé les animeaux identiques
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            // si les animeaux choisis ne sont pas identiques
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        /* réinitialisation du jeu si tout est trouvé */
        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {   if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
