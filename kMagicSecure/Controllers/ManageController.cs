using IdentitySample.Models;
using Magic.Domain;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IdentitySample.Controllers
{
	[RequireHttps]
	[Authorize]
	public class ManageController : Controller
	{
		private Magic.Data.IDataContextWrapper _dataContext = null;
		private Magic.Core.IPlayerManager _playerManager = null;
		private TelemetryClient _telemetryClient = null;

		public ManageController()
		{
			Setup();
		}

		public ManageController(ApplicationUserManager userManager)
		{
			UserManager = userManager;
			Setup();
		}

		private void Setup()
		{
			var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
			_dataContext = new Magic.Data.DataContextWrapper(connectionString);
			var playerRepo = new Magic.Data.PlayerRepository(_dataContext);
			_playerManager = new Magic.Core.PlayerManager(playerRepo);
			_telemetryClient = new TelemetryClient();
		}

		private ApplicationUserManager _userManager;
		public ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				_userManager = value;
			}
		}

		//
		// GET: /Account/Index
		public async Task<ActionResult> Index(ManageMessageId? message)
		{
			_telemetryClient.TrackTrace(new TraceTelemetry("ManageController::Index"));
			ViewBag.StatusMessage =
					message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
					: message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
					: message == ManageMessageId.Error ? "An error has occurred."
					: message == ManageMessageId.AddPhoneSuccess ? "The phone number was added."
					: message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
					: message == ManageMessageId.SetDisplayNameSuccess ? "Your display name was updated"
					: "";

			var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
			var confirmed = user.EmailConfirmed;
			var player = _playerManager.GetPlayerByEmail(user.Email);
			

			var model = new IndexViewModel
			{
				DisplayName = player?.Name,
				HasPassword = HasPassword(),
				PhoneNumber = await UserManager.GetPhoneNumberAsync(User.Identity.GetUserId()),
				TwoFactor = await UserManager.GetTwoFactorEnabledAsync(User.Identity.GetUserId()),
				Logins = await UserManager.GetLoginsAsync(User.Identity.GetUserId()),
				BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(User.Identity.GetUserId()),
				IsConfirmed = confirmed
			};
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SetPlayerName(IndexViewModel model)
		{
			_telemetryClient.TrackTrace(new TraceTelemetry("Start DisplayName set"));
			var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

			// Check Mode: Pair Player or Change Name
			var allPlayers = _playerManager.GetAllPlayers();
			if(allPlayers.Any(p=>p.Email==user.Email))
			{
				try
				{
					_telemetryClient.TrackTrace(new TraceTelemetry("Mode: Changing DisplayName"));

					var oldPlayerObj = allPlayers.First(p => p.Email == user.Email);

					var oldPlayer = new dbPlayer
					{
						ID = oldPlayerObj.ID,
						Name = oldPlayerObj.Name,
						Email = oldPlayerObj.Email
					};
					var newPlayer = new dbPlayer
					{
						ID = oldPlayerObj.ID,
						Name = model.NewDisplayName,
						Email = oldPlayerObj.Email
					};

					_playerManager.UpdatePlayer(oldPlayer, newPlayer);

					return await Index(ManageMessageId.SetDisplayNameSuccess);
				}
				catch(Exception ex)
				{
					_telemetryClient.TrackException(ex);
					return await Index(ManageMessageId.Error);
				}
			}
			else
			{
				try
				{
					_telemetryClient.TrackTrace(new TraceTelemetry("Mode: Pairing DisplayName"));
					var oldPlayerObj = allPlayers.First(p => p.Name == model.NewDisplayName);

					var oldPlayer = new dbPlayer
					{
						ID = oldPlayerObj.ID,
						Name = oldPlayerObj.Name,
						Email = oldPlayerObj.Email
					};

					var newPlayer = new dbPlayer
					{
						ID = oldPlayerObj.ID,
						Name = oldPlayer.Name,
						Email = user.Email
					};

					_playerManager.UpdatePlayer(oldPlayer, newPlayer);

					return await Index(ManageMessageId.SetDisplayNameSuccess);
				}
				catch(Exception ex)
				{
					_telemetryClient.TrackException(ex);
					return await Index(ManageMessageId.Error);
				}
			}

			return RedirectToAction("Index",	new { message = ManageMessageId.SetDisplayNameSuccess});
		}

		//
		// GET: /Manage/ChangePassword
		public ActionResult ChangePassword()
		{
			return View();
		}

		//
		// POST: /Account/Manage
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			_telemetryClient.TrackTrace(new TraceTelemetry("ChangePassword"));
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
			if (result.Succeeded)
			{
				var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
				if (user != null)
				{
					await SignInAsync(user, isPersistent: false);
				}
				return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
			}
			AddErrors(result);
			return View(model);
		}

		//
		// GET: /Manage/SetPassword
		public ActionResult SetPassword()
		{
			return View();
		}

		//
		// POST: /Manage/SetPassword
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
		{
			_telemetryClient.TrackTrace(new TraceTelemetry("SetPassword"));
			if (ModelState.IsValid)
			{
				var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
				if (result.Succeeded)
				{
					var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
					if (user != null)
					{
						await SignInAsync(user, isPersistent: false);
					}
					return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
				}
				AddErrors(result);
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		#region Helpers
		// Used for XSRF protection when adding external logins
		private const string XsrfKey = "XsrfId";

		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		private async Task SignInAsync(ApplicationUser user, bool isPersistent)
		{
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
			AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		private bool HasPassword()
		{
			var user = UserManager.FindById(User.Identity.GetUserId());
			if (user != null)
			{
				return user.PasswordHash != null;
			}
			return false;
		}

		public enum ManageMessageId
		{
			AddPhoneSuccess,
			ChangePasswordSuccess,
			SetPasswordSuccess,
			RemovePhoneSuccess,
			Error,
			SetDisplayNameSuccess
		}

		#endregion
	}
}