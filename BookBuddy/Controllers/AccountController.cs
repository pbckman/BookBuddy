using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddy.Controllers;

[Authorize]
public class AccountController : Controller
{

}
