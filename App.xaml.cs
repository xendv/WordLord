using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls.Page;

namespace WordLord
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            InitializeComponent();
        }
        [STAThreadAttribute]
        static void main(string[] args)
        {
            App app = new App();
            // Создание главного окна.
            //NavigationWindow win = (NavigationWindow)Window.GetWindow(this);
            // Запуск приложения и отображение главного окна,
            //NavigationWindow win = new NavigationWindow();
                //win.Content = new Page1();
            //win.Show();
            //app.Run(window);
            string s = "Слово";
            Word w = new Word(s);


            //w.PrintLetters();
            //Window_1 w = new Window_1();
            //w.Show();
        }

    }
}
