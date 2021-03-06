﻿using System.Security.Claims;

namespace LinFx.Security.Users
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }

        string Id { get; }

        string UserName { get; }

        string PhoneNumber { get; }

        bool PhoneNumberVerified { get; }

        string Email { get; }

        bool EmailVerified { get; }

        string TenantId { get; }

        string[] Roles { get; }

        Claim FindClaim(string claimType);

        Claim[] FindClaims(string claimType);

        Claim[] GetAllClaims();

        bool IsInRole(string roleName);
    }
}
