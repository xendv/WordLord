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

namespace WordLord
{
    /// <summary>
    /// Логика взаимодействия для WordsListItem.xaml
    /// </summary>
    public partial class WordsListItem : UserControl
    {
        public WordsListItem()
        {
            InitializeComponent();
        }
        public string Word
        {
            get { return (string)GetValue(WordProperty); }
            set { SetValue(WordProperty, value); }
        }
        public static readonly DependencyProperty WordProperty= DependencyProperty.Register("Word", typeof(Word), typeof(WordsListItem), new PropertyMetadata(new Word()));
    }
}
