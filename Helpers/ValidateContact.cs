using System;
using System.Linq;

namespace Sicem_Blazor.Helpers;

public class ValidateContact
{
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;

        return phoneNumber.All(char.IsDigit) && phoneNumber.Length == 10;
    }
}
