using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ufoTransfer.Models;
using ufoTransfer.HelperFunctions;

namespace ufoTransfer.Pages
{
    public class UsersModel : PageModel
    {
        private readonly ufoTransfer.Models.UfoTransferContext _context;

        public UsersModel(ufoTransfer.Models.UfoTransferContext context)
        {
            _context = context;
        }

        public IList<User> Users { get; set; }

        public async Task OnGetAsync()
        {
            Users = await _context.Users.ToListAsync();
        }

        [BindProperty]
        public User Gebruiker { get; set; }

        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string pw = Gebruiker.PASSWORD;

            var hashed = PasswordStorage.HashPassword(Encoding.UTF8.GetBytes(pw), PasswordStorage.GenerateSalt(), 50000);

            pw = Convert.ToBase64String(hashed);

            Gebruiker.PASSWORD = pw;

            _context.Users.Add(Gebruiker);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }

    }