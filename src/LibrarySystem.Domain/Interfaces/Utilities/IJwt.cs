﻿using System.Security.Claims;

namespace LibrarySystem.Domain.Interfaces.Utilities;

public interface IJwt
{
    string Issuer { get; }
    string Key { get; }

    string Generate(IEnumerable<Claim> claims);
}