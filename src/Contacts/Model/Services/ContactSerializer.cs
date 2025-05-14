using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Model.Services
{
    /// <summary>
    /// Сохраняет и загружает данные контактов
    /// </summary>
    public class ContactSerializer
    {
        /// <summary>
        /// Хранит путь к файлу.
        /// </summary>
        private readonly string _filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Contacts",
            "contacts.json"
        );

        /// <summary>
        /// Сохраняет коллекцию контактов в файл
        /// </summary>
        public void SaveContacts(ObservableCollection<Contact> contacts)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
                string json = JsonConvert.SerializeObject(contacts, Formatting.Indented);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка сохранения: {ex.Message}");
            }
        }

        /// <summary>
        /// Загружает коллекцию контактов из файла
        /// </summary>
        public ObservableCollection<Contact> LoadContacts()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return new ObservableCollection<Contact>();
                }

                string json = File.ReadAllText(_filePath);
                return JsonConvert.DeserializeObject<ObservableCollection<Contact>>(json)
                    ?? new ObservableCollection<Contact>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка загрузки: {ex.Message}");
            }
        }
    }
}
