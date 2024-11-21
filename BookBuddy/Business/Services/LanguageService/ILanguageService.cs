using System;

namespace BookBuddy.Business.Services.LanguageService;

public interface ILanguageService
{
    List<string> GetEnabledLanguages();
}
