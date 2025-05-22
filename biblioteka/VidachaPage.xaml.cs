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
using static System.Collections.Specialized.BitVector32;

namespace biblioteka
{
    /// <summary>
    /// Логика взаимодействия для VidachaPage.xaml
    /// </summary>
    public partial class VidachaPage : Page
    {
        private readonly студенческая_библиотекаEntities1 _context;
        public VidachaPage()
        {
            InitializeComponent();
            _context = new студенческая_библиотекаEntities1();
            LoadData();
        }
        private void LoadData()
        {
            BookComboBox.ItemsSource = _context.Книги.ToList();
            StudentComboBox.ItemsSource = _context.Студенты.ToList();
        }

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
