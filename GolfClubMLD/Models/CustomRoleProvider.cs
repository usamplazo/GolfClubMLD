﻿using GolfClubMLD.Models.EFRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace GolfClubMLD.Models
{
    public class CustomRoleProvider : RoleProvider
    {
		AuthentificationRepository _authRepo = new AuthentificationRepository();
		public CustomRoleProvider() { }

		public override bool IsUserInRole(string username, string roleName)
		{
			List<string> roles = _authRepo.GetRoles(username);
			return roles.Count != 0 && roles.Contains(roleName);
		}

		public override string[] GetRolesForUser(string username)
		{
			List<string> roles = _authRepo.GetRoles(username);
			return roles.ToArray();
		}

		#region Not Implemented Methods

		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override string ApplicationName
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override void CreateRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			throw new NotImplementedException();
		}

		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			throw new NotImplementedException();
		}

		public override string[] GetAllRoles()
		{
			throw new NotImplementedException();
		}

		public override string[] GetUsersInRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override bool RoleExists(string roleName)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
