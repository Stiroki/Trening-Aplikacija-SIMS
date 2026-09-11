using System;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using TreningAplikacija.Repositories;
using TreningAplikacija.Models;

namespace TreningAplikacija.Converters
{
    public class GuidToExerciseNameConverter : IValueConverter
    {
        private static readonly JsonRepository<Exercise> _exerciseRepo = new("exercises.json");

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Guid exerciseId)
            {
                var exercise = _exerciseRepo.GetById(exerciseId);
                return exercise != null ? exercise.Name : "Nepoznata vežba";
            }
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}