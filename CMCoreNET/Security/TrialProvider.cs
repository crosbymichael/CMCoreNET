using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCoreNET.Security
{
    public interface ITrial
    {
        bool IsValidTrial { get; }
    }

    public class TrialProvider : ITrial
    {
        private const int DAYS_VALID = 30;
        private DateTime INVALID_DATE;

        public TrialProvider(DateTime invalidDate)
        {
            this.INVALID_DATE = invalidDate;
        }

        public bool IsValidTrial
        {
            get 
            {
                return DateTime.Now < INVALID_DATE;
            }
        }
    }
}
