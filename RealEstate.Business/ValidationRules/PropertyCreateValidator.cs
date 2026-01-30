using FluentValidation;
using RealEstate.Business.Dtos.PropertyDtos;

namespace RealEstate.Business.ValidationRules;

public class PropertyCreateValidator : AbstractValidator<PropertyCreateDto>
{
    public PropertyCreateValidator()
    {
        // BAŞLIK KURALLARI
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("İlan başlığı boş geçilemez.")
            .MinimumLength(5).WithMessage("Başlık en az 5 karakter olmalı.")
            .MaximumLength(200).WithMessage("Başlık 200 karakteri geçemez.");

        // FİYAT KURALLARI
        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Fiyat boş geçilemez.")
            .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır.");

        // AÇIKLAMA
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Açıklama zorunludur.")
            .MinimumLength(10).WithMessage("Açıklama çok kısa, en az 10 karakter yazın.");

        // ŞEHİR
        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Şehir seçimi zorunludur.");

        // ODA SAYISI
        RuleFor(x => x.Rooms)
            .GreaterThanOrEqualTo(0).WithMessage("Oda sayısı negatif olamaz.");
            
        // METREKARE
        RuleFor(x => x.Area)
            .GreaterThan(0).WithMessage("Metrekare 0'dan büyük olmalıdır.");
    }
}