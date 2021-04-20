using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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
            MainWindow win = new MainWindow();
            //SetPage(win,"Main");
            win.Show();
            app.Run(win);
            //NavigationWindow win = (NavigationWindow)Window.GetWindow(this);
            // Запуск приложения и отображение главного окна,
            //NavigationWindow win = new NavigationWindow();
            //win.Content = new MainPage();
            //win.Show();
            //app.Run(window);

            //string s = "Слово";
            //Word w = new Word(s);


            //w.PrintLetters();
            //Window_1 w = new Window_1();
            //w.Show();
        }
        /*public void SetPage(MainWindow win, string page_name)
        {
            switch (page_name)
            {
                case "Main":
                    win.Content = new MainPage();
                    break;
                case "StartGame":
                    win.Content = new GamePage();
                    break;
                default:
                    MessageBox.Show("Запрашиваемая страница не найдена");
                    break;
            }
        }*/

    }
}
