using System.Collections.Generic;
using System.Collections.Immutable;

namespace SIS.MvcFramework.Validation
{
    public class ModelStateDictionary
    {
        private readonly IDictionary<string, List<string>> errorMessages;

        public ModelStateDictionary()
        {
            this.IsValid = true;
            this.errorMessages = new Dictionary<string, List<string>>();
        }

        public bool IsValid { get; set; }

        public IReadOnlyDictionary<string, List<string>> ErrorMessages => this.errorMessages.ToImmutableDictionary();

        public void Add(string propertyName, string errorMessage)
        {
            if (!this.errorMessages.ContainsKey(propertyName))
            {
                this.errorMessages.Add(propertyName, new List<string>());
            }

            this.errorMessages[propertyName].Add(errorMessage);
        }
    }
}
