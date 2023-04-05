﻿namespace DiscordSecretSanta.Core.ViewModels.AdminUsers;

public class AdminUsersViewModel
{
    public bool Authorised { get; set; }
    public IReadOnlyList<UserViewModel> Users { get; set; } = new List<UserViewModel>();
}