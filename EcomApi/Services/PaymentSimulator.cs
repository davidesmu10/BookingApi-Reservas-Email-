namespace EcomApi.Services;

public class PaymentSimulator
{
    // Simple: aprueba si la suma de dígitos es múltiplo de 3
    public (bool approved, string reference) Charge(string cardNumber, decimal amount)
    {
        var sum = cardNumber.Where(char.IsDigit).Select(c => c - '0').Sum();
        var ok = sum % 3 == 0 && amount > 0;
        var refCode = $"PAY-{DateTime.UtcNow:yyyyMMddHHmmss}-{sum}";
        return (ok, refCode);
    }
}
