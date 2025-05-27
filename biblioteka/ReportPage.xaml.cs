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
    /// Логика взаимодействия для ReportPage.xaml
    /// </summary>
    public partial class ReportPage : Page
    {
        private студенческая_библиотекаEntities1 _context = new студенческая_библиотекаEntities1();

        public ReportPage()
        {
            InitializeComponent();
            LoadReport();
        }

        private void LoadReport()
        {
            var rawData = (from v in _context.Выдачи
                           join s in _context.Студенты on v.ID_Студента equals s.ID_Студента
                           join k in _context.Книги on v.ID_Книги equals k.ID_Книги
                           join r in _context.Возвраты on v.ID_Выдачи equals r.ID_Выдачи into vr
                           from возврат in vr.DefaultIfEmpty()
                           select new
                           {
                               Студент = s.ФИО,
                               Книга = k.Название,
                               Дата_Выдачи = v.Дата_Выдачи,
                               Срок_Возврата = v.Срок_Возврата,
                               Дата_Возврата = возврат != null ? (DateTime?)возврат.Дата_Возврата : null,
                               Состояние = возврат != null ? возврат.Состояние : null
                           }).ToList();

            var reportData = rawData.Select(x => new
            {
                x.Студент,
                x.Книга,
                Дата_Выдачи = x.Дата_Выдачи.ToShortDateString(),
                Срок_Возврата = x.Срок_Возврата.ToShortDateString(),
                Дата_Возврата = x.Дата_Возврата.HasValue
                    ? x.Дата_Возврата.Value.ToShortDateString()
                    : "Не возвращена",
                Состояние = x.Состояние ?? "-"
            }).ToList();


            ReportDataGrid.ItemsSource = reportData;
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
                NavigationService.GoBack();
            else
                MessageBox.Show("Невозможно вернуться назад.");
        }
    }
}