﻿using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Contracts.Services;

public interface IUserService
{
    Task<User> RegisterUser(RegisterUserDto registerUserDto);
}
