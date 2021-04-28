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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool hasErrors = false;
        public bool gameStarted = false;
        public GamePage gamePageChild;
        public MainWindow()
        {
            InitializeComponent();
            SetPage("Main");
        }
        public void SetPage(string page_name)
        {
            switch (page_name)
            {
                case "Main":
                    Content = new MainPage(this);
                    break;
                case "StartGame":
                    Content = new GamePage(this);
                    break;
                case "ContinueGame":
                    Content = new GamePage(this, true);
                    break;
                case "AboutGame":
                    Content = new AboutGamePage(this);
                    break;
                case "AboutAuthor":
                    Content = new AboutAuthorPage(this);
                    break;
                default:
                    MessageBox.Show("Запрашиваемая страница не найдена");
                    break;
            }
        }
        ///<summary>
        ///Возвращает true и отображает ошибки в файле словаря, если они есть
        ///и возвращает false, если их нет
        ///</summary>
        public bool DisplayDictionaryErrors(string error)
        {
            switch (error)
            {
                case "Wrong Content":
                    MessageBox.Show("Похоже, файл поврежден!\nИсправьте его содержимое", "Ошибка содержания файла", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    break;
                case "No File":
                    MessageBox.Show("Файл словаря пустой, создаю новый...", "Словарь не найден", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case "":
                    {
                        return false;
                        //PrintExistingWordsList(ref wordsLoader.GetWords());
                        //this.ShowDialog();
                    }
                    //break;
                default:
                    MessageBox.Show("Необработанное исключение!", "ОЙ", MessageBoxButton.OK, MessageBoxImage.Error);
                    /*Difference between Show() and ShowDialog() methods
                     * is in restricting access to the window that opened new window
                     * in the second method*/
                    break;
            }
            return true;
        }

        public bool DisplayDBErrors(string error)
        {
            if (error.Length != 0)
            {
                MessageBox.Show(error, "DBError", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            return false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (gameStarted)
            {
                MessageBoxResult result = MessageBox.Show("Сохранить игру?", "Выход", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                CloseGameMessageBox(result, e);
            }
        }

        public void CloseGameMessageBox(MessageBoxResult result, System.ComponentModel.CancelEventArgs e)
        {
            switch (result)
            {
                case MessageBoxResult.Yes:
                    gamePageChild.SaveGame();
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    break;
                default:
                    MessageBox.Show("Некорректный обработчик кнопок!");
                    break;
            }
        }
        /*
                 private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!wordsLoader.HasSameContentAsCurrentList(wordsFromDictionary))
            {
                MessageBoxResult result = MessageBox.Show("В словаре есть несохраненные изменения.\n\nСохранить?", "Подтвердить изменения", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                CloseDictionaryMessageBox(result, e);
            }
        }
        public void CloseDictionaryMessageBox(MessageBoxResult result, System.ComponentModel.CancelEventArgs e)
        {

            switch (result)
            {
                case MessageBoxResult.Yes:
                    wordsLoader.RewriteFileContent(wordsFromDictionary);
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    break;
                default:
                    MessageBox.Show("Некорректный обработчик кнопок!");
                    break;
            }
        }

         */
    }
}
