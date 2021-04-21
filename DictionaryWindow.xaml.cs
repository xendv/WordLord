﻿using System;
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
using System.Windows.Shapes;

namespace WordLord
{
    /// <summary>
    /// Логика взаимодействия для DictionaryWindow.xaml
    /// </summary>
    public partial class DictionaryWindow : Window
    {
        public DictionaryWindow(MainWindow win)
        {
            InitializeComponent();
            WordsLoader wordsLoader = new WordsLoader(this);
            if (!win.DisplayDictionaryErrors(wordsLoader.errorType))
            {
                PrintExistingWordsList(ref wordsLoader.GetWords());
            }
        }
        public void PrintExistingWordsList(ref List<Word> words)
        {
            this.ShowDialog();
        }
        public static readonly DependencyProperty wordsListProperty =
           DependencyProperty.Register("wordsList", typeof(List<Word>), typeof(DictionaryWindow), new PropertyMetadata(null));
    }
}
