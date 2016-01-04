using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class BeginCapitalLetterAttribute : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return string.Format("Поле {0} должно начинаться с заглавной буквы.", new[] { name });
        }

        public override bool IsValid(object value)
        {
            // Если объект пустой, то не проверяем.
            if (value == null) return true;

            string strVal = value.ToString();
            return Char.IsUpper(strVal, 0);
        }
    }
}