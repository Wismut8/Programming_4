using Newtonsoft.Json;
using System;
using System.IO;

namespace View.Model.Services
{
    /// <summary>
    /// Сохраняет и загружает данные.
    /// </summary>
    public class ContactSerializer
    {
        /// <summary>
        /// Хранит путь по умолчанию.
        /// </summary>
        private string _filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Contacts",
            "contacts.json"
        );

        /// <summary>
        /// Сохранение контакта в файл.
        /// </summary>
        /// <param name="contact">Объект контакта. <see cref="Contact"/></param>
        /// <exception cref="Exception"></exception>
        public void SaveContact(Contact contact)
        {
            try
            {
                string json = JsonConvert.SerializeObject(contact, Formatting.Indented);
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Загрузка контакта из файла.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Contact LoadContact()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return new Contact();
                }
                string json = File.ReadAllText(_filePath);
                Contact contact = JsonConvert.DeserializeObject<Contact>(json);
                return contact;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
