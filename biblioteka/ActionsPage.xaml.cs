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

namespace biblioteka
{
    /// <summary>
    /// Представляет страницу с действиями, доступными пользователю библиотеки.
    /// </summary>
    public partial class ActionsPage : Page
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ActionsPage"/>.
        /// </summary>
        public ActionsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки «Выдача книг» и переходит на страницу <see cref="VidachaPage"/>.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события <see cref="RoutedEventArgs"/>.</param>
        private void GoToVidacha_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new VidachaPage());
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки «Возврат книг» и переходит на страницу <see cref="VozvratPage"/>.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события <see cref="RoutedEventArgs"/>.</param>
        private void GoToVozvrat_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new VozvratPage());
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки «Архив книг» и переходит на страницу <see cref="ArxivPage"/>.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события <see cref="RoutedEventArgs"/>.</param>
        private void GoToArxiv_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ArxivPage());
        }

        private void GoToReport_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ReportPage());

        }
    }
}
