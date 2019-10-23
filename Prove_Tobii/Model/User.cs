using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Prove_Tobii.Model
{
    public class User : IDataErrorInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [XmlIgnore]
        public DateTime Age { get; set; }
        public string EducationLevel { get; set; }
        private int ageInYears;

        public User()
        {
            Age = DateTime.Now;
        }
        public string Error
        {
            get { return null; }
        }

        public int AgeInYears { get => getAgeFromDate(Age); set => ageInYears = value; }

        int GetDifferenceInYears(DateTime startDate, DateTime endDate)
        {
            return (endDate.Year - startDate.Year - 1) +
                (((endDate.Month > startDate.Month) ||
                ((endDate.Month == startDate.Month) && (endDate.Day >= startDate.Day))) ? 1 : 0);
        }

        public string this[string columnName]
        {
            get
            {
                Debug.WriteLine("entro");
                string result = null;
                var regexItem = new Regex("[^A-Za-z]");
                var regexCombo = new Regex("Seleziona.*");
                if (columnName == "FirstName")
                {
                    if (string.IsNullOrEmpty(FirstName))
                    {
                        result = "Per piacere inserisci un nome.";
                        return result;
                    }
                    else if (regexItem.IsMatch(FirstName))
                    {
                        result = "Inserisci solo caratteri validi, senza numeri spazi o caratteri speciali";
                        return result;
                    }
                }
                if (columnName == "LastName")
                {
                    if (string.IsNullOrEmpty(LastName))
                    {
                        result = "Per piacere inserisci un cognome";
                        return result;
                    }
                    else if (regexItem.IsMatch(LastName))
                    {
                        result = "Inserisci solo caratteri validi, senza numeri spazi o caratteri speciali";
                        return result;
                    }
                }
                if (columnName == "Age")
                {
                    var ageInYears = GetDifferenceInYears(Age, DateTime.Today);
                    if (ageInYears < 8)
                    {
                        result = "Inserire un'età maggiore di 8 anni.";
                        return result;
                    }
                }
                if(columnName == "EducationLevel")
                {
                    if (string.IsNullOrEmpty(EducationLevel) || regexCombo.IsMatch(EducationLevel) )
                    {
                        result = "Per piacere inserisci il tuo livello di educazione.";
                        return result;
                    }
                }
                return result;
            }
        }
        public int getAgeFromDate(DateTime birthday)
        {
            // Save today's date.
            var today = DateTime.Today;
            // Calculate the age.
            var age = today.Year - birthday.Year;
            // Go back to the year the person was born in case of a leap year
            if (birthday.Date > today.AddYears(-age)) age--;
            return age;

        }
    }
}
