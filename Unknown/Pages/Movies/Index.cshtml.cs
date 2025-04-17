using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Models;
using Unknown.Data;

namespace Unknown.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly Unknown.Data.UnknownContext _context;

        public IndexModel(Unknown.Data.UnknownContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Movie = await _context.Movie.ToListAsync();
        }
    }

}
