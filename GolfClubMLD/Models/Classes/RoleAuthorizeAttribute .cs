using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GolfClubMLD.Models.Classes
{
	public enum Roles
	{
		Customer,
		Manager,
		Admin
	}

	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
	public class RoleAuthorizeAttribute : AuthorizeAttribute
	{
		public RoleAuthorizeAttribute(params object[] roles)
		{
			if (roles.Any(r => r.GetType().BaseType != typeof(Enum)))
				throw new ArgumentException("The roles parameter may only contain enums", "roles");

			var temp = roles.Select(r => Enum.GetName(r.GetType(), r)).ToList();
			Roles = string.Join(",", temp);
		}

		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			var request = filterContext.HttpContext.Request;
			var url = new UrlHelper(filterContext.RequestContext);
			var accessDeniedUrl = url.Action("AccessDenied", "Error");

			if (!string.IsNullOrEmpty(base.Roles))
			{
				var isRoleError = true;
				var rolesAllowed = base.Roles.Split(',');

				var user = filterContext.HttpContext.User;
				if (user != null && rolesAllowed.Any())
				{
					foreach (var role in rolesAllowed)
						if (user.IsInRole(role))
							isRoleError = false;
				}

				if (isRoleError)
				{
					if (request.IsAjaxRequest())
						filterContext.Result = new JsonResult { Data = new { error = true, signinerror = true, message = "Access denied", url = accessDeniedUrl }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
					else
						filterContext.Result = new RedirectResult(accessDeniedUrl);
				}
			}
		}
	}
}