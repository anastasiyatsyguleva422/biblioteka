using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace biblioteka
{
    /// <summary>
    /// Главное окно приложения. Реализует авторизацию пользователя (библиотекаря).
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Инициализирует новый экземпляр окна <see cref="MainWindow"/>.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Вычисляет хэш SHA-256 для строки, используя кодировку Unicode.
        /// </summary>
        /// <param name="rawData">Входная строка (обычно — пароль).</param>
        /// <returns>Хэш-строка в шестнадцатеричном формате.</returns>
        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.Unicode.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (var t in bytes)
                {
                    builder.Append(t.ToString("X2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Обрабатывает событие нажатия кнопки входа в систему.
        /// Проверяет введённые логин и пароль, выполняет аутентификацию и переходит на страницу <see cref="ActionsPage"/>.
        /// </summary>
        /// <param name="sender">Источник события (кнопка).</param>
        /// <param name="e">Аргументы события.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = log.Text.Trim();
            string password = pas.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль");
                return;
            }

            string passwordHash = ComputeSha256Hash(password);

            using (var db = new студенческая_библиотекаEntities1())
            {
                var user = db.Библиотекари
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Логин == login && u.Пароль == passwordHash);

                if (user == null)
                {
                    MessageBox.Show("Пользователь с такими данными не найден");
                    return;
                }

                Session.БиблиотекарьID = user.ID_Библиотекаря;

                MessageBox.Show($"Вход выполнен! Добро пожаловать, {user.ФИО}!");

                MainFrame.Navigate(new ActionsPage());
            }
        }
    }
}
