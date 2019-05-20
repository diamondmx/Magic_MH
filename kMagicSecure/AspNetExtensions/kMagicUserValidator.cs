using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace kMagicSecure.AspNetExtensions
{
	public class kMagicUserValidator<TUser> : UserValidator<TUser> where TUser : class, IUser<string>
	{
		public kMagicUserValidator(UserManager<TUser> manager)
			: base(manager)
		{ }

		public override async Task<IdentityResult> ValidateAsync(TUser item)
		{
			var result = await base.ValidateAsync(item);
			if (!item.UserName.ToLowerInvariant().Contains("@kcura.com") && !item.UserName.ToLowerInvariant().Contains("@relativity.com"))
			{
				var newErrors = result.Errors.ToList();
				newErrors.Add("Email address is not within an accepted domain");
				result = IdentityResult.Failed(newErrors.ToArray());
			}

			return result;
		}



	}
}