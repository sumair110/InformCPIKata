using InformCPIKata.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InformCPIKata.Controllers
{
	public class ContactController : Controller
	{
		#region Variable 

		private readonly ContactDbContext _context;
		private readonly ILogger<ContactController> _logger;

		public ContactController(ContactDbContext context, ILogger<ContactController> logger)
		{
			_context = context;
			_logger = logger;
		}

		#endregion

		#region Methods

		#region Index (Listing of Contacts)

		// GET: Contact
		public async Task<IActionResult> Index()
		{
			try
			{
				var contacts = await _context.Contacts.ToListAsync();
				return View(contacts);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving contacts from database");
				return RedirectToAction("Error", "Home");
			}
		}

		#endregion

		#region CRUD (CRUD for Contacts)

		// GET: Contact/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Contact/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("FirstName,LastName,Address,Email,Phone")] Contact contact)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var existingContact = await _context.Contacts.FirstOrDefaultAsync(c => c.Phone == contact.Phone);

					var existingEmail = await _context.Contacts.FirstOrDefaultAsync(c => c.Email.ToLower() == contact.Email.ToLower());

					if (existingContact != null)
					{
						ModelState.AddModelError(string.Empty, "Contact with the same phone number already exists.");
						return View(contact);
					}

					if (existingEmail != null)
					{
						ModelState.AddModelError(string.Empty, "Contact with this email already exists.");
						return View(contact);
					}

					_context.Add(contact);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
				return View(contact);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error creating contact: {0}", ex.Message);
				return RedirectToAction("Error", "Home");
			}
		}

		// GET: Contact/Edit/Id
		public async Task<IActionResult> Edit(int? id)
		{
			try
			{
				if (id == null)
				{
					return NotFound();
				}

				var contact = await _context.Contacts.FindAsync(id);
				if (contact == null)
				{
					return NotFound();
				}
				return View(contact);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving contact for editing: {0}", ex.Message);
				return RedirectToAction("Error", "Home");
			}
		}

		// POST: Contact/Edit/Id
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Address,Email,Phone")] Contact contact)
		{
			try
			{
				if (id != contact.Id)
				{
					return NotFound();
				}

				if (ModelState.IsValid)
				{
					try
					{
						// Check for duplicate Email and phone number
						var isPhoneExists = _context.Contacts.Any(c => c.Id != contact.Id && c.Phone == contact.Phone);
						var isEmailExists = _context.Contacts.Any(c => c.Id != contact.Id && c.Email.ToLower() == contact.Email.ToLower());

						if (isEmailExists)
						{
							ModelState.AddModelError(string.Empty, "Contact with same name already exists.");
							return View(contact);
						}

						if (isPhoneExists)
						{
							ModelState.AddModelError(string.Empty, "Contact with same phone number already exists.");
							return View(contact);
						}

						if (ModelState.IsValid)
						{
							_context.Update(contact);
							_context.SaveChanges();
							return RedirectToAction(nameof(Index));
						}
					}
					catch (DbUpdateConcurrencyException)
					{
						if (!ContactExists(contact.Id))
						{
							return NotFound();
						}
						else
						{
							throw;
						}
					}
					return RedirectToAction(nameof(Index));
				}
				return View(contact);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error editing contact: {0}", ex.Message);
				return RedirectToAction("Error", "Home");
			}
		}

		// GET: Contact/Delete/Id
		public async Task<IActionResult> Delete(int? id)
		{
			try
			{
				if (id == null)
				{
					return NotFound();
				}

				var contact = await _context.Contacts
					.FirstOrDefaultAsync(m => m.Id == id);
				if (contact == null)
				{
					return NotFound();
				}

				return View(contact);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving contact for deletion: {0}", ex.Message);
				return RedirectToAction("Error", "Home");
			}
		}

		// POST: Contact/Delete/Id
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			try
			{
				var contact = await _context.Contacts.FindAsync(id);
				_context.Contacts.Remove(contact);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error deleting contact: {0}", ex.Message);
				return RedirectToAction("Error", "Home");
			}
		}

		#endregion

		#endregion

		#region Validations

		private bool ContactExists(int id)
		{
			return _context.Contacts.Any(e => e.Id == id);
		}

		#endregion
	}
}
