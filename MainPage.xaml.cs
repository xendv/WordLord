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

using System.Windows.Xps.Packaging;
using System.Windows.Xps;
using System.Collections;
using System.IO;
using System.Xml;

namespace WordLord
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainWindow mainWin;
        public bool dictionaryErrors=false;
        //Page curr_page;
        public MainPage()
        {
            InitializeComponent();
        }
        public MainPage(MainWindow win)
        {
            InitializeComponent();
            this.mainWin = win;
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            this.mainWin.Content = new GamePage();
        }

        private void ButtonContinue_Click(object sender, RoutedEventArgs e)
        {
            this.mainWin.Content = new GamePage();
        }

        private void ButtonDictionary_Click(object sender, RoutedEventArgs e)
        {
            DictionaryWindow dictionaryWindow = new DictionaryWindow(this.mainWin);
            dictionaryWindow.Owner = this.mainWin;



            /*string tempFileName = System.IO.Path.GetTempPath();

            tempFileName = System.IO.Path.Combine(tempFileName, Guid.NewGuid().ToString() + ".xps");
            string filePath = "C:\\Users\\xendv\\source\\repos\\WordLord\\Dictionary.txt";

            XpsDocument doc = new XpsDocument(tempFileName, System.IO.FileAccess.ReadWrite);
            //var doc = System.Windows.Application.LoadComponent(new Uri("/DictionaryWindow.xaml", UriKind.Relative)) as FixedDocument;
            //doc.AddPages();
            //XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
            //writer.Write(dictionaryWindow.documentViewer.Document as FixedDocument);
            //doc.Close();

            //FileInfo fileInfo = new FileInfo(filePath);
            //StreamReader doc = new StreamReader(fileInfo.Open(FileMode.Open, FileAccess.Read), Encoding.GetEncoding(1251));

            dictionaryWindow.documentViewer.Document = doc.GetFixedDocumentSequence();
            //doc.Close();
            //File.Delete(tempFileName);*/
        }
        public void OpenFile() { 
        }
        public static void AddPages(FixedDocument fixedDocument)
        {
            var enumerator = fixedDocument.Resources.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var pageContent = ((DictionaryEntry)enumerator.Current).Value as PageContent;
                if (pageContent != null)
                {
                    fixedDocument.Pages.Add(pageContent);
                }
            }
        }
    }
}
