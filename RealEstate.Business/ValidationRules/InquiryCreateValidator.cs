using FluentValidation;
using RealEstate.Business.Dtos.InquiryDtos;

namespace RealEstate.Business.ValidationRules;

public class InquiryCreateValidator : AbstractValidator<InquiryCreateDto>
{
    public InquiryCreateValidator()
    {
        // AD SOYAD
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("İsim zorunludur.")
            .MinimumLength(2).WithMessage("İsim en az 2 karakter olmalıdır.");

        // E-POSTA
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta zorunludur.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

        // MESAJ İÇERİĞİ
        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Mesaj içeriği boş olamaz.")
            .MinimumLength(10).WithMessage("Mesajınız çok kısa.")
            .MaximumLength(1000).WithMessage("Mesajınız çok uzun.");
            
        // İLAN ID
        RuleFor(x => x.PropertyId)
            .GreaterThan(0).WithMessage("Geçersiz İlan ID.");
    }
}