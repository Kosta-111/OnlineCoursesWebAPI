using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Core.Services;

public class AccountsService(
    IMapper mapper,
    UserManager<User> userManager
    ) : IAccountsService
{
    public async Task Register(RegisterModel model)
    {
        var user = mapper.Map<User>(model);
        var result = await userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            throw new Exception(string.Join("; ", result.Errors.Select(x => x.Description)));
    }
}
