using ClearBank.DeveloperTest.Types;
using System;

namespace ClearBank.DeveloperTest.Validation
{
    public abstract class SafeValidator
    {
        public bool AccountCanMakePayment(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            return IsValid(account);
        }

        protected abstract bool IsValid(Account account);
    }
}