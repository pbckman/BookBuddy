using System;

namespace BookBuddy.Business.Services.LanguageService;

public class LanguageService(ILanguageBranchRepository languageBranchRepository) : ILanguageService
{
    public interface ILanguageService
    {
        List<string> GetEnabledLanguages();
    }

    private readonly ILanguageBranchRepository _languageBranchRepository = languageBranchRepository;

    public List<string> GetEnabledLanguages()
    {
        return _languageBranchRepository.ListEnabled().Select(x => x.LanguageID).ToList();
    }
}
