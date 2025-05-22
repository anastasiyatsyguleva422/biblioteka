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
    /// Представляет страницу архива книг в приложении библиотеки.
    /// Позволяет просматривать и редактировать информацию о книгах.
    /// </summary>
    public partial class ArxivPage : Page
    {
        /// <summary>
        /// Контекст базы данных библиотеки.
        /// </summary>
        private readonly студенческая_библиотекаEntities1 _context;

        /// <summary>
        /// Инициализирует новый экземпляр страницы <see cref="ArxivPage"/>.
        /// Загружает список книг и годов издания.
        /// </summary>
        public ArxivPage()
        {
            InitializeComponent();
            _context = new студенческая_библиотекаEntities1();
            LoadBooks();
            LoadYears();
        }

        /// <summary>
        /// Загружает список книг из базы данных и устанавливает его источником данных для <c>BookComboBox</c>.
        /// </summary>
        private void LoadBooks()
        {
            BookComboBox.ItemsSource = _context.Книги.ToList();
        }

        /// <summary>
        /// Загружает список годов от текущего до 1900 года в <c>YearComboBox</c>.
        /// </summary>
        private void LoadYears()
        {
            for (int year = DateTime.Now.Year; year >= 1900; year--)
            {
                YearComboBox.Items.Add(year);
            }
        }

        /// <summary>
        /// Обрабатывает выбор книги из списка. 
        /// Загружает и отображает данные книги, автора, жанра и года издания.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события выбора элемента.</param>
        private void BookComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookComboBox.SelectedItem is Книги selectedBook)
            {
                TitleTextBox.Text = selectedBook.Название;

                var author = _context.Авторы.FirstOrDefault(a => a.ID_Автора == selectedBook.ID_Автора);
                if (author != null)
                {
                    AuthorTextBox.Text = author.ФИО;
                    CountryTextBox.Text = author.Страна;
                }
                else
                {
                    AuthorTextBox.Text = "";
                    CountryTextBox.Text = "";
                }

                var genre = _context.Жанры.FirstOrDefault(g => g.ID_Жанра == selectedBook.ID_Жанра);
                GenreTextBox.Text = genre != null ? genre.Название : "";

                YearComboBox.SelectedItem = selectedBook.Год_Издания;
                QuantityTextBox.Text = selectedBook.Количество.ToString();
            }
        }

        /// <summary>
        /// Сохраняет изменения в выбранной книге, включая автора, жанр, год и количество.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события нажатия кнопки.</param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (BookComboBox.SelectedItem is Книги selectedBook)
            {
                try
                {
                    selectedBook.Название = TitleTextBox.Text;

                    // Работа с автором
                    var authorName = AuthorTextBox.Text.Trim();
                    var author = _context.Авторы.FirstOrDefault(a => a.ФИО == authorName);
                    if (author == null)
                    {
                        author = new Авторы { ФИО = authorName };
                        _context.Авторы.Add(author);
                        _context.SaveChanges();
                    }
                    selectedBook.ID_Автора = author.ID_Автора;

                    // Работа с жанром
                    var genreName = GenreTextBox.Text.Trim();
                    var genre = _context.Жанры.FirstOrDefault(g => g.Название == genreName);
                    if (genre == null)
                    {
                        genre = new Жанры { Название = genreName };
                        _context.Жанры.Add(genre);
                        _context.SaveChanges();
                    }
                    selectedBook.ID_Жанра = genre.ID_Жанра;

                    // Год и количество
                    if (YearComboBox.SelectedItem != null)
                        selectedBook.Год_Издания = (int)YearComboBox.SelectedItem;

                    selectedBook.Количество = int.Parse(QuantityTextBox.Text);

                    _context.SaveChanges();
                    MessageBox.Show("Изменения успешно сохранены!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Выберите книгу для редактирования.");
            }
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки «Назад» и возвращает пользователя на предыдущую страницу.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события нажатия кнопки.</param>
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
