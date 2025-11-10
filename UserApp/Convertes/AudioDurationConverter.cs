using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using NAudio;
using NAudio.Wave;
namespace UserApp.Convertes
{
    public class AudioDurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string filePath && !string.IsNullOrEmpty(filePath))
            {
                try
                {
                    using (var audioFile = new AudioFileReader(filePath))
                    {
                        TimeSpan duration = audioFile.TotalTime;

                        // Форматируем в "мм:сс" (если меньше часа) или "чч:мм:сс"
                        return duration.TotalHours >= 1
                            ? duration.ToString(@"hh\:mm\:ss")
                            : duration.ToString(@"mm\:ss");
                    }
                }
                catch (Exception ex)
                {
                    // Логирование ошибки (если файл не найден или не поддерживается)
                    System.Diagnostics.Debug.WriteLine($"Ошибка чтения аудио: {ex.Message}");
                    return "00:00"; // или "N/A"
                }
            }
            return "00:00"; // Если путь пустой
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Обратное преобразование не поддерживается");
        }
    }
}
