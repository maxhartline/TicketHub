using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TicketHub
{
    public class Purchase : IValidatableObject
    {
        [Required]
        public int concertId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Quantity must be between 1 and 10.")]
        public int Quantity { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 15, ErrorMessage = "Credit card number must be between 15 and 16 digits.")]
        public string CreditCard { get; set; }

        [Required]
        [StringLength(5, ErrorMessage = "Expiration must be formatted MM/YY.")]
        public string Expiration { get; set; }
        public bool cardExpired()
        {
            // Parse expiration date
            int month = int.Parse(Expiration.Substring(0, 2));
            int year = int.Parse(Expiration.Substring(3, 2)) + 2000;

            // Create DateTime object for the first day of the month following expiration
            DateTime nextMonthFirstDay = new DateTime(year, month, 1).AddMonths(1);

            // Set expiration date to last day the expiration month
            DateTime expirationDate = nextMonthFirstDay.AddDays(-1);

            // DateTime object for current date
            DateTime currentDate = DateTime.Now;

            return expirationDate < currentDate;
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (cardExpired())
            {
                yield return new ValidationResult(
                    "Card is expired.",
                    new[] { "Expiration" }
                );
            }
        }

        [Required]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "Security code must be between 3 and 4 digits.")]
        public string SecurityCode { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
        public string Address { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
        public string City { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Province cannot exceed 50 characters.")]
        public string Province { get; set; }

        [Required]
        [RegularExpression(@"^[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ]\s?[0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]$", ErrorMessage = "Postal code format must be A1A 1A1")]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Country cannot exceed 100 characters.")]
        public string Country { get; set; }
    }
}
