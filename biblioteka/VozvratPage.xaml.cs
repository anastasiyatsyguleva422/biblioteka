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
    public partial class VozvratPage : Page
    {
        private студенческая_библиотекаEntities1 _context = new студенческая_библиотекаEntities1();
        public VozvratPage()
        {
            InitializeComponent();
            LoadIssues();
        }
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
