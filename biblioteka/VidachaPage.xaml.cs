using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace biblioteka
{
    /// <summary>
    /// Страница выдачи книг студентам.
    /// </summary>
    public partial class VidachaPage : Page
    {
        private readonly студенческая_библиотекаEntities1 _context;

        /// <summary>
        /// Инициализирует новый экземпляр страницы <see cref="VidachaPage"/>.
        /// Загружает список книг и студентов.
        /// </summary>
        public VidachaPage()
        {
            InitializeComponent();
            _context = new студенческая_библиотекаEntities1();
            LoadData();
        }

        /// <summary>
        /// Загружает данные в комбобоксы: список книг и студентов.
        /// </summary>
        private void LoadData()
        {
            BookComboBox.ItemsSource = _context.Книги.ToList();
            StudentComboBox.ItemsSource = _context.Студенты.ToList();
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Выдать книгу".
        /// Выполняет проверку и добавляет новую запись о выдаче.
        /// </summary>
        /// <param name="sender">Источник события (кнопка).</param>
        /// <param name="e">Аргументы события.</param>
        private void Book_Click(object sender, RoutedEventArgs e)
        {
            if (BookComboBox.SelectedValue == null || StudentComboBox.SelectedValue == null || !DataPicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            int selectedBookId = (int)BookComboBox.SelectedValue;
            var selectedBook = _context.Книги.FirstOrDefault(b => b.ID_Книги == selectedBookId);

            if (selectedBook == null)
            {
                MessageBox.Show("Выбранная книга не найдена.");
                return;
            }

            if (selectedBook.Количество <= 0)
            {
                MessageBox.Show("Книг нет в наличии для выдачи.");
                return;
            }

            DateTime today = DateTime.Now.Date;
            DateTime returnDate = DataPicker.SelectedDate.Value.Date;

            if ((returnDate - today).TotalDays > 14)
            {
                MessageBox.Show("Срок возврата не может превышать двух недель (14 дней).");
                return;
            }

            if (returnDate < today)
            {
                MessageBox.Show("Срок возврата не может быть раньше сегодняшнего дня.");
                return;
            }

            var выдача = new Выдачи
            {
                ID_Книги = selectedBookId,
                ID_Студента = (int)StudentComboBox.SelectedValue,
                ID_Библиотекаря = Session.БиблиотекарьID,
                Дата_Выдачи = today,
                Срок_Возврата = returnDate
            };

            _context.Выдачи.Add(выдача);
            selectedBook.Количество--;

            _context.SaveChanges();

            MessageBox.Show("Книга успешно выдана.");
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Назад".
        /// Переходит на предыдущую страницу, если это возможно.
        /// </summary>
        /// <param name="sender">Источник события (кнопка).</param>
        /// <param name="e">Аргументы события.</param>
        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
            {
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Невозможно вернуться назад.");
            }
        }
    }
}

