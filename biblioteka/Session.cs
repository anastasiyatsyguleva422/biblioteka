using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biblioteka
{
    /// <summary>
    /// Класс <c>Session</c> содержит данные текущей сессии пользователя.
    /// Используется для хранения информации о текущем авторизованном библиотекаре.
    /// </summary>
    internal class Session
    {
        /// <summary>
        /// Идентификатор авторизованного библиотекаря в текущей сессии.
        /// </summary>
        public static int БиблиотекарьID { get; set; }
    }
}
