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

namespace Hangman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string DirectoryPath = @"Text Files\";
        string AnimalFile = @"Text Files\Animal.txt";
        string CountryFile = @"Text Files\Country.txt";
        string FruitFile = @"Text Files\Fruit.txt";

        string[] FruitData = new string[] {"Apple" , "Banana" , "Cherry" , "Dates" , "Elderberry" , "Fig" ,
                                            "Grape" , "Kiwi" , "Mango" , "Orange" , "Rassberry" , "Watermalon"} ;
        string[] AnimalData = new string[] {"Anaconda" , "Bear" , "Cat" , "Dog" , "Elephant" , "Fox" , "Giraffe" ,
                                            "Horse" , "Lion" , "Monkey" , "Tiger" , "Zebra"};
        string[] CountryData = new string[] {"Afghanistan" , "Brazil" , "Canada" , "Denmark" , "Egypt" , "Finland",
        "Germany" , "Hungary" , "India" , "Japan" , "Myanmar" , "NewZealand" , "Russia" , "SouthAfrica","Turkey","Zimbabwe"};
        string SelectedWord = "";

        Random R = new Random();

        int WrongAnswer = 0;
        SoundPlayer Music = new SoundPlayer();

        public MainWindow()
        {
            InitializeComponent();
            FileOperations();
            txtCategory.SelectedIndex = 0;
        }

        public void SetWord() {
            string Selection = txtCategory.Text;

            if (Selection == "Country Name") {
                SetText(CountryFile);
            }

            else if (Selection == "Animal Name")
            {
                SetText(AnimalFile);
            }
            else if (Selection == "Fruit Name")
            {
                SetText(FruitFile);
            }
        }

        void SetText(string FileName) {
            SelectedWord = "";
            int TotalWord = 0;
            string Line;

            if (File.Exists(FileName))
            {
                //For Counting File Contains How Many Words
                using (StreamReader SR = new StreamReader(FileName))
                {
                    while ((Line = SR.ReadLine()) != null)
                    {
                        TotalWord++;
                    }
                }

                //If FileDoes Not Contains Any Word Then, Category Will Be Changed To Next One
                if (TotalWord < 1)
                {
                    if (txtCategory.SelectedIndex == 0)
                    {
                        txtCategory.SelectedIndex = 1;
                    }
                    else if (txtCategory.SelectedIndex == 1)
                    {
                        txtCategory.SelectedIndex = 2;
                    }
                    else if (txtCategory.SelectedIndex == 2)
                    {
                        txtCategory.SelectedIndex = 0;
                    }
                }

                //If File Contain Words
                else
                {
                    int RandomNumber = R.Next(1,TotalWord);
                    TotalWord = 0;

                    using (StreamReader SR = new StreamReader(FileName))
                    {
                        while ((Line = SR.ReadLine()) != null)
                        {
                            TotalWord++;
                            if (TotalWord == RandomNumber)
                            {
                                SelectedWord = Line;
                                break;
                            }
                        }
                    }

                    int WordLength = SelectedWord.Length;
                    string LabelContent = "";

                    if (WordLength <= 8)
                    {
                        int RandomCharacterPlace = R.Next(1,WordLength);
                        char DefaultCharacter = SelectedWord[RandomCharacterPlace];


                        foreach (char CharacterInSelectedWord in SelectedWord)
                        {
                            if (CharacterInSelectedWord == DefaultCharacter)
                            {
                                LabelContent = LabelContent + CharacterInSelectedWord + " ";
                            }
                            else
                            {
                                LabelContent = LabelContent + "* ";
                            }
                        }
                    }
                    else {
                        int RandomCharacterPlace1 = R.Next(1,WordLength), RandomCharacterPlace2 = R.Next(1,WordLength);
                        goto CheckBothNumber;


                        CheckBothNumber:
                        if (RandomCharacterPlace1 == RandomCharacterPlace2) {
                            RandomCharacterPlace2 = R.Next(1,WordLength);
                            goto CheckBothNumber;
                        }

                        char DefaultCharacter1 = SelectedWord[RandomCharacterPlace1];
                        char DefaultCharacter2 = SelectedWord[RandomCharacterPlace2];


                        foreach (char CharacterInSelectedWord in SelectedWord)
                        {
                            if (CharacterInSelectedWord == DefaultCharacter1 || CharacterInSelectedWord==DefaultCharacter2)
                            {
                                LabelContent = LabelContent + CharacterInSelectedWord + " ";
                            }
                            else
                            {
                                LabelContent = LabelContent + "* ";
                            }
                        }
                    }
                    lblCharacter1.Content = LabelContent.Trim();
                }
            }
            else {
                MessageBox.Show("This Word List Is Under Process.","File Not Availabel",MessageBoxButton.OK,MessageBoxImage.Information);
            }
        }

        public void FileOperations() {
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }

            if (!File.Exists(AnimalFile))
            {
                File.Create(AnimalFile);
            }

            if (!File.Exists(FruitFile))
            {
                File.Create(FruitFile);
            }

            if (!File.Exists(CountryFile))
            {
                File.Create(CountryFile);
            }

            if (File.Exists(AnimalFile)) {
                using (StreamWriter SW = new StreamWriter(AnimalFile)) {
                    foreach (string AnimalName in AnimalData) {
                        SW.WriteLine(AnimalName.ToUpper());
                    }
                    SW.Close();
                }
            }

            if (File.Exists(CountryFile))
            {
                using (StreamWriter SW = new StreamWriter(CountryFile))
                {
                    foreach (string CountryName in CountryData)
                    {
                        SW.WriteLine(CountryName.ToUpper());
                    }
                    SW.Close();
                }
            }

            if (File.Exists(FruitFile))
            {
                using (StreamWriter SW = new StreamWriter(FruitFile))
                {
                    foreach (string FruitName in FruitData)
                    {
                        SW.WriteLine(FruitName.ToUpper());
                    }
                    SW.Close();
                }
            }

        }

        private void txtInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtInput.Text == "" || txtInput.Text == null) {
                txtInput.Text = "Enter Character";
                txtInput.Foreground = System.Windows.Media.Brushes.DarkGray;
            }
        }

        private void txtInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtInput.Text == "Enter Character") {
                txtInput.Text = "";
                txtInput.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void txtCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetNewString();
        }

        private void GetNewString() {
            Music.Stop();
            imgHangman.Source = new BitmapImage(new Uri(@"E:/Conestoga/Semester 1 Jan 2020 - May 2020/High Quality Software Programming/Assignments/Assignment 4/Hangman/Hangman/Images/1.PNG", UriKind.RelativeOrAbsolute));
            lblRemainingLives.Content = "Total Lives Remaining 3";
            lblRemainingLives.Foreground = Brushes.Black;
            lblLastInput.Content = "Last Wrong Input";
            if (txtCategory.SelectedIndex == 0)
            {
                SetText(CountryFile);
            }
            else if (txtCategory.SelectedIndex == 1)
            {
                SetText(FruitFile);
            }
            else if (txtCategory.SelectedIndex == 2)
            {
                SetText(AnimalFile);
            }
            WrongAnswer = 0;
        }

        private void btnTryAnother_Click(object sender, RoutedEventArgs e)
        {
            GetNewString();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            int InputLength = txtInput.Text.Length;
            if (txtInput.Text == "Enter Character" || txtInput.Text == null)
            {
                MessageBox.Show("Please Enter Character.", "Enter Any Character", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else {
                int Flag = 0;
                char InputedCharacter;
                Char.TryParse(txtInput.Text.Trim(), out InputedCharacter);
                foreach (char CharacterInSelectedWord in SelectedWord) {
                    if (CharacterInSelectedWord == InputedCharacter) {
                        Flag++;
                    }
                }

                char[] LabelContent = lblCharacter1.Content.ToString().ToCharArray();
                if (Flag > 0)
                {
                    Music.SoundLocation = @"E:\Conestoga\Semester 1 Jan 2020 - May 2020\High Quality Software Programming\Assignments\Assignment 4\Hangman\Hangman\Right Answer.wav";
                    Music.Play();
                    int Position = -2;
                    foreach (char CharacterInSelectedWord in SelectedWord)
                    {
                        Position++; Position++;
                        if (CharacterInSelectedWord == InputedCharacter)
                        {
                            LabelContent[Position] = CharacterInSelectedWord;
                        }
                    }
                    string NewLabelContent = new string(LabelContent);
                    lblCharacter1.Content = NewLabelContent;

                    Flag = 0;
                    foreach (char CharacterInSelectedWord in NewLabelContent)
                    {
                        if (CharacterInSelectedWord == '*')
                        {
                            Flag++;
                        }
                    }

                    if (Flag == 0)
                    {
                        Music.SoundLocation = @"E:\Conestoga\Semester 1 Jan 2020 - May 2020\High Quality Software Programming\Assignments\Assignment 4\Hangman\Hangman\Solved.wav";
                        Music.Play();
                        MessageBox.Show("You Cracked The " + txtCategory.Text , "Congratulations",MessageBoxButton.OK);
                        System.Threading.Thread.Sleep(5000);
                        GetNewString();
                    }
                }
                else {
                    if (WrongAnswer != 2) {
                        Music.SoundLocation = @"E:\Conestoga\Semester 1 Jan 2020 - May 2020\High Quality Software Programming\Assignments\Assignment 4\Hangman\Hangman\Wrong Answer.wav";
                        Music.Play();
                    }
                    WrongAnswer++;
                    if (WrongAnswer == 1) {
                        imgHangman.Source = new BitmapImage(new Uri(@"E:/Conestoga/Semester 1 Jan 2020 - May 2020/High Quality Software Programming/Assignments/Assignment 4/Hangman/Hangman/Images/5.PNG", UriKind.RelativeOrAbsolute));
                        lblRemainingLives.Content = "Total Lives Remaining 2";
                        lblRemainingLives.Foreground = Brushes.Orange;
                        lblLastInput.Content = lblLastInput.Content.ToString() + " " + txtInput.Text.Trim(); 
                    }
                    if (WrongAnswer == 2)
                    {
                        imgHangman.Source = new BitmapImage(new Uri(@"E:/Conestoga/Semester 1 Jan 2020 - May 2020/High Quality Software Programming/Assignments/Assignment 4/Hangman/Hangman/Images/6.PNG", UriKind.RelativeOrAbsolute));
                        lblRemainingLives.Content = "Total Lives Remaining 1";
                        lblRemainingLives.Foreground = Brushes.Red;
                        lblLastInput.Content = lblLastInput.Content.ToString() +", "+ txtInput.Text.Trim();
                    }
                    if (WrongAnswer == 3)
                    {
                        imgHangman.Source = new BitmapImage(new Uri(@"E:/Conestoga/Semester 1 Jan 2020 - May 2020/High Quality Software Programming/Assignments/Assignment 4/Hangman/Hangman/Images/7.PNG", UriKind.RelativeOrAbsolute));
                        lblRemainingLives.Content = "Total Lives Remaining 0";
                        lblRemainingLives.Foreground = Brushes.DarkRed;
                        lblLastInput.Content = lblLastInput.Content.ToString() +", "+ txtInput.Text.Trim();
                        Music.SoundLocation = @"E:\Conestoga\Semester 1 Jan 2020 - May 2020\High Quality Software Programming\Assignments\Assignment 4\Hangman\Hangman\GameOverSound (1).wav";
                        Music.Play();
                        MessageBox.Show("Upps! Game Over. \nThe Answer Is " + SelectedWord , "Better Luck Next Time." , MessageBoxButton.OK,MessageBoxImage.Information);
                        System.Threading.Thread.Sleep(3000);
                        GetNewString();
                    }
                }
            }

            txtInput.Foreground = System.Windows.Media.Brushes.DarkGray;
            txtInput.Text = "Enter Character";

        }

        private void txtInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-z]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
