﻿using LibrarySystem.Application.Contracts.Services;

namespace LibrarySystem.Application.Contracts;

public interface IServiceManager
{
    IUserService UserService { get; }
    IStaffService StaffService { get; }
}
