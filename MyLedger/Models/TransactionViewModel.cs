using System.ComponentModel.DataAnnotations;

namespace MyLedger.Models
{
	public class TransactionViewModel
	{
		public int Id { get; set; }

		[Required]
		public TransactionType Type { get; set; }  // Enum burada kullanılıyor
		[Required]
		public IncomeCategory? IncomeCategory { get; set; }
		[Required]
		public ExpenseCategory? ExpenseCategory { get; set; }
	}
}
