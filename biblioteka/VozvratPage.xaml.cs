using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace biblioteka
{
    /// <summary>
    /// Страница возврата книг.
    /// Позволяет регистрировать возврат ранее выданной книги.
    /// </summary>
    public partial class VozvratPage : Page
    {
        private студенческая_библиотекаEntities1 _context = new студенческая_библиотекаEntities1();

        /// <summary>
        /// Инициализирует новый экземпляр страницы <see cref="VozvratPage"/>.
        /// Загружает список активных выдач.
        /// </summary>
        public VozvratPage()
        {
            InitializeComponent();
            LoadIssues();
        }

        /// <summary>
        /// Загружает список всех активных (не возвращённых) выдач.
        /// </summary>
        private void LoadIssues()
        {
            var activeIssues = _context.Выдачи
                .Where(v => !_context.Возвраты.Any(r => r.ID_Выдачи == v.ID_Выдачи))
                .ToList()
                .Select(v => new
                {
                    v.ID_Выдачи,
                    Описание = v.Книги.Название + " - " + v.Студенты.ФИО + " (" + v.Дата_Выдачи.ToShortDateString() + ")"
                })
                .ToList();

            IssueComboBox.ItemsSource = activeIssues;
            IssueComboBox.DisplayMemberPath = "Описание";
            IssueComboBox.SelectedValuePath = "ID_Выдачи";
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки возврата книги.
        /// Добавляет запись о возврате, обновляет количество книг.
        /// </summary>
        /// <param name="sender">Источник события (кнопка).</param>
        /// <param name="e">Аргументы события.</param>
        private void Vozvrat_Click(object sender, RoutedEventArgs e)
        {
            if (IssueComboBox.SelectedValue == null || string.IsNullOrWhiteSpace(BookConditionTextBox.Text))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            int issueId = (int)IssueComboBox.SelectedValue;
            var issue = _context.Выдачи.FirstOrDefault(v => v.ID_Выдачи == issueId);

            if (issue == null)
            {
                MessageBox.Show("Выдача не найдена.");
                return;
            }

            var возврат = new Возвраты
            {
                ID_Выдачи = issueId,
                Дата_Возврата = DateTime.Now,
                Состояние = BookConditionTextBox.Text
            };

            _context.Возвраты.Add(возврат);

            var книга = _context.Книги.FirstOrDefault(b => b.ID_Книги == issue.ID_Книги);
            if (книга != null)
            {
                книга.Количество++;
            }

            _context.SaveChanges();

            MessageBox.Show("Книга успешно возвращена.");
            BookConditionTextBox.Text = string.Empty;
            LoadIssues();
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Назад".
        /// Переходит к предыдущей странице, если это возможно.
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

