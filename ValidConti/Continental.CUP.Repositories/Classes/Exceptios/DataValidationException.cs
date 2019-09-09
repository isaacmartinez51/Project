using System;

namespace Continental.CUP.Repositories.Classes.Exceptios
{
    public class DataValidationException : Exception
    {
        public string PropertyName { get; set; }

        public string ErrorMessage { get; private set; }

        public DataValidationException(string propertyName, string errorMessage)
        {
            this.PropertyName = propertyName;
            this.ErrorMessage = errorMessage;
        }
    }
}
