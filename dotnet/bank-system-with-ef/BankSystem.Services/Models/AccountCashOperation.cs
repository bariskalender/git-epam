namespace BankSystem.Services.Models;

public class AccountCashOperation
{
    public AccountCashOperation(decimal amount, DateTime date, string? note)
    {
        this.Amount = amount;
        this.Date = date;
        this.Note = note;
    }

    public decimal Amount { get; }

    public DateTime Date { get; }

    public string? Note { get; }

    public override string ToString()
    {
        string action = this.Amount >= 0
            ? $"Credited to account {this.Amount}."
            : $"Debited from account {this.Amount}.";

        return $"{this.Date.ToString("MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)} {this.Note} : {action}";
    }
}
