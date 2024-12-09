using System.ComponentModel.DataAnnotations;

namespace HumanResourcesMVC.CustomValidations
{
	public class IdentificationNumberValidation : ValidationAttribute
	{
        public string ErrorMesage { get; set; }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
            if (value == null)
            {
                return new ValidationResult(ErrorMessage ?? "T.C Kimlik Numarası Boş Olamaz!");
            }

            string tc = (string)value;



			// 1. Kontrol
			if (!tc.All(Char.IsDigit))
			{
				return new ValidationResult(ErrorMessage ?? "T.C Kimlik Sadece Rakamlardan Oluşmalıdır!");
			}

			// 2. Kontrol
			if (tc.Length != 11)
			{
				return new ValidationResult(ErrorMessage ?? "T.C Kimlik 11 Haneli Olmak Zorundadır!");
			}

			// 3. Kontrol
			if (tc.StartsWith('0'))
			{
				return new ValidationResult(ErrorMessage ?? "T.C Kimlik No '0' İle Başlayamaz!");
			}


			// 4. Kontrol
			int ilkToplam = 0;
			int ikinciToplam = 0;

			for (int i = 0; i < tc.Length - 2; i++) // 9 Dedik Çünkü Burdaki Tekler ve Çiftler Toplamıyla İş kalmıyo
			{

				if (i % 2 == 0)
				{
					ilkToplam += int.Parse(tc[i].ToString());
				}
				else
				{
					ikinciToplam += int.Parse(tc[i].ToString());
				}
			}

			int basamakKontrolu = ((7 * ilkToplam) - ikinciToplam) % 10;

			if (char.Parse(basamakKontrolu.ToString()) != tc[9])
			{
				return new ValidationResult(ErrorMessage ?? "Geçersiz bir T.C Kimlik girdiniz.");
			}


			// 5. Kontrol

			int toplamKontrol = 0;

			for (int i = 0; i < tc.Length - 1; i++) // Son Basamak Toplansın İstemiyoruz
			{
				toplamKontrol += int.Parse(tc[i].ToString());
			}

			toplamKontrol = toplamKontrol % 10;

			if (char.Parse(toplamKontrol.ToString()) != tc[10])
			{
				return new ValidationResult(ErrorMessage ?? "Geçersiz bir T.C Kimlik girdiniz.");
			}

			return ValidationResult.Success;
		}
	}
}
