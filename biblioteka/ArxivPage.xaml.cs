using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace biblioteka
{
    public partial class ArxivPage : Page
    {
        private readonly студенческая_библиотекаEntities1 _context;

        public ArxivPage()
        {
            InitializeComponent();
            _context = new студенческая_библиотекаEntities1();
            LoadBooks();
            LoadYears();
            LoadCountries();
        }

        private void LoadBooks()
        {
            BookComboBox.ItemsSource = _context.Книги.ToList();
        }

        private void LoadYears()
        {
            for (int year = DateTime.Now.Year; year >= 1900; year--)
            {
                YearComboBox.Items.Add(year);
            }
        }

        private void LoadCountries()
        {
            var countries = _context.Авторы
                .Select(a => a.Страна)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            CountryComboBox.ItemsSource = countries;
        }

        private void BookComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookComboBox.SelectedItem is Книги selectedBook)
            {
                TitleTextBox.Text = selectedBook.Название;

                var author = _context.Авторы.FirstOrDefault(a => a.ID_Автора == selectedBook.ID_Автора);
                if (author != null)
                {
                    AuthorTextBox.Text = author.ФИО;
                    CountryComboBox.SelectedItem = CountryComboBox.Items
     .Cast<string>()
     .FirstOrDefault(c => c != null && c.Equals(author.Страна, StringComparison.OrdinalIgnoreCase));

                }
                else
                {
                    AuthorTextBox.Text = "";
                    CountryComboBox.SelectedIndex = -1;
                }

                var genre = _context.Жанры.FirstOrDefault(g => g.ID_Жанра == selectedBook.ID_Жанра);
                GenreTextBox.Text = genre != null ? genre.Название : "";

                YearComboBox.SelectedItem = selectedBook.Год_Издания;
                QuantityTextBox.Text = selectedBook.Количество.ToString();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (BookComboBox.SelectedItem is Книги selectedBook)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(CountryComboBox.Text))
                    {
                        MessageBox.Show("Выберите страну автора!");
                        return;
                    }

                    selectedBook.Название = TitleTextBox.Text;

                    // Работа с автором
                    var authorName = AuthorTextBox.Text.Trim();
                    var country = CountryComboBox.Text.Trim();

                    var author = _context.Авторы.FirstOrDefault(a =>
                        a.ФИО == authorName &&
                        a.Страна == country);

                    if (author == null)
                    {
                        author = new Авторы
                        {
                            ФИО = authorName,
                            Страна = country
                        };
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

                    LoadCountries(); // Обновляем список стран
                    LoadBooks();     // Обновляем список книг
                    BookComboBox.SelectedItem = selectedBook;

                    MessageBox.Show("Изменения успешно сохранены!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите книгу для редактирования.");
            }
        }

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
